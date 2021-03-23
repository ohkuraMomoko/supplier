using Infrastructure.LoggerService;
using Infrastructure.LoggerService.Services;
using SupplierPlatform.Entities;
using SupplierPlatform.Enums;
using SupplierPlatform.Extensions;
using SupplierPlatform.Helps;
using SupplierPlatform.Models;
using SupplierPlatform.Models.ViewModel;
using SupplierPlatform.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SupplierPlatform.Controllers
{
    public class HomeController : BaseController
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILoggerProvider Logger;

        /// <summary>
        /// 登入者資訊
        /// </summary>
        private readonly IOperatorContext OperatorContent;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="_operatorContent"></param>
        public HomeController(IOperatorContext _operatorContent) : base()
        {
            this.Logger = new NLogger("HomeController");
            this.OperatorContent = _operatorContent ?? throw new ArgumentNullException(nameof(_operatorContent));
        }

        public ActionResult Guideline()
        {
            return this.View();
        }

        /// <summary>
        /// 登入頁
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Login()
        {
            return this.View();
        }

        /// <summary>
        /// 登入檢核
        /// </summary>
        /// <param name="model">登入檢核 ViewModel</param>
        /// <returns>登入狀態</returns>
        [HttpPost]
        public async Task<JsonResult> CheckLogin(LoginViewModel model)
        {
            // 檢查必填項目是否有值
            this.ValidateModel(model);

            // 檢核統編輸入值是否為數字
            if (!model.UniformNumbers.Regex(RegexEnum.Numeric, 8))
            {
                this.Logger.Error($"會員帳號[{model.UniformNumbers} {model.Phone}]統一編號輸入錯誤！");
                return this.Json(new BaseResult<string>
                {
                    Data = "2",
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "統一編號輸入錯誤"
                    }
                });
            }

            // 檢核電話輸入值是否為數字(09/17改為僅檢查8-10碼數字)
            if (!(model.Phone.Regex(RegexEnum.Numeric, 8) || model.Phone.Regex(RegexEnum.Numeric, 9) || model.Phone.Regex(RegexEnum.Numeric, 10)))
            {
                this.Logger.Error($"會員帳號[{model.UniformNumbers} {model.Phone}]帳號輸入錯誤！");
                return this.Json(new BaseResult<string>
                {
                    Data = "2",
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "帳號或密碼或驗證碼錯誤"
                    }
                });
            }

            // 檢核驗證碼輸入值是否為數字
            if (!model.Code.Regex(RegexEnum.Numeric, 4))
            {
                this.Logger.Error($"會員帳號[{model.UniformNumbers} {model.Phone}]密碼輸入錯誤！");
                return this.Json(new BaseResult<string>
                {
                    Data = "2",
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "驗證碼錯誤"
                    }
                });
            }

            string CheckCodeOutput = string.Empty;
            if (!this.CheckCode(model.Code, out CheckCodeOutput))
            {
                this.Logger.Error($"會員帳號[{model.UniformNumbers} {model.Phone}]驗證碼輸入錯誤！");
                return this.Json(new BaseResult<string>
                {
                    Data = "2",
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = string.IsNullOrEmpty(CheckCodeOutput) ? "驗證碼錯誤" : CheckCodeOutput
                    }
                });
            }

            try
            {
                BaseResult<LoginResultViewModel> baseResult = await HomeService.CheckLogin(model);

                if (baseResult.Data?.RtnCd == -1)
                {
                    this.Logger.Error($"會員帳號[{model.UniformNumbers} {model.Phone}]帳號輸入錯誤！");
                    return this.Json(new BaseResult<string>
                    {
                        Data = "2",
                        Result = new ResultModel
                        {
                            ReturnCode = -1,
                            Alert = "帳號或密碼或驗證碼錯誤"
                        }
                    });
                }

                if (baseResult.Data?.VenderStatusCd != "A")
                {
                    this.Logger.Error($"VenderStatusCd [{baseResult.Data?.VenderStatusCd}]！");
                    return this.Json(new BaseResult<string>
                    {
                        Data = "2",
                        Result = new ResultModel
                        {
                            ReturnCode = -1,
                            Alert = "系統登入失敗，請洽負責業務人員"
                        }
                    });
                }

                this.OperatorContent.Operator = new VendorOperator
                {
                    IS_EP_VENDER = baseResult.Data.IsEpVender,
                    APPROVE_POLICY = baseResult.Data.ApprovePolicy,
                    DEFAULT_PROD_NME = baseResult.Data.DefaultProdName,
                    EP_RATE_TYPE_1 = baseResult.Data.EpRateType1,
                    EP_RATE_TYPE_2 = baseResult.Data.EpRateType2,
                    IS_ROLE01_TYPE = baseResult.Data.IsRole01Type,
                    IS_ROLE02_TYPE = baseResult.Data.IsRole02Type,
                    MEMBER_ID = baseResult.Data.MemberId,
                    TOP_VENDER_ID = baseResult.Data.TopVenderId,
                    TOP_VENDER_NME = baseResult.Data.TopVenderName,
                    VENDER_ID = baseResult.Data.VenderId,
                    VENDER_NME = baseResult.Data.VenderName,
                    VENDER_STATUS_CD = baseResult.Data.VenderStatusCd,
                    VEND_SALE_NME = baseResult.Data.VendSaleName,
                    VNO2 = baseResult.Data.Vno2,
                    MENU_LIST = baseResult.Data.Mnmus,
                    IsMobile = false,
                    MobileTypeNnum = MobileTypeEnum.Web,
                    EpLstOnceAmt = baseResult.Data.EpLstOnceAmt
                };

                HttpCookie StudentCookies = new HttpCookie("login_mobile_type");
                StudentCookies.Value = ((VendorOperator)OperatorContent.Operator).MobileTypeNnum.ToString().ToLower();
                StudentCookies.Expires = DateTime.Now.AddHours(12);
                Response.Cookies.Add(StudentCookies);

                return this.Json(new BaseResult<string>
                {
                    Data = baseResult.Data.ApprovePolicy,
                    Result = new ResultModel
                    {
                        ReturnCode = baseResult.Data.RtnCd,
                        Alert = baseResult.Data.AlertMsg
                    }
                });
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{model.UniformNumbers} {model.Phone}]登入失敗，原因為：{e.Message}！！！");

                return this.Json(new BaseResult<string>
                {
                    Data = "2",
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "登入失敗，請聯繫客服人員，謝謝"
                    }
                });
            }
        }

        /// <summary>
        /// 停機資訊
        /// </summary>
        /// <returns>停機公告訊息</returns>
        [HttpPost]
        public async Task<JsonResult> DowntimeCheck()
        {
            try
            {
                ResultModel result = await HomeService.DowntimeCheck();

                return this.Json(result);
            }
            catch (Exception e)
            {
                this.Logger.Error($"停機資訊取得失敗，原因為：{e.Message}！！！");

                // TODO 在後端API開發完成前，此處一律回覆停機資訊
                return this.Json(new ResultModel
                {
                    ReturnCode = 1,
                    Alert = "停機資訊：因本行為提昇服務品質，預計於5/4 (星期六) 00:30AM ~ 5/4 (星期六) 08:00AM進行系統主機例行維護作業，屆時本行將暫時停止下述金融服務項目，如造成您的不便，請您見諒。"
                });
            }
        }

        /// <summary>
        /// 系統公告
        /// </summary>
        /// <returns>系統公告訊息</returns>
        [HttpPost]
        public async Task<JsonResult> SystemNotification()
        {
            try
            {
                ResultModel result = await HomeService.SystemNotification();

                return this.Json(result);
            }
            catch (Exception e)
            {
                this.Logger.Error($"系統公告取得失敗，原因為：{e.Message}！！！");

                // TODO 在後端API開發完成前，此處一律回覆停機資訊
                return this.Json(new ResultModel
                {
                    ReturnCode = 0,
                    Alert = "系統公告：提醒您，12/14 (六）適逢中租企業尾牙，調整服務時間通知。"
                });
            }
        }

        /// <summary>
        /// 供應商忘記密碼(重設密碼)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<JsonResult> ReSetPassWord(LoginBaseModel model)
        {
            // 檢查必填項目是否有值
            this.ValidateModel(model);

            // 檢核統編輸入值是否為數字
            if (!model.UniformNumbers.Regex(RegexEnum.Numeric, 8))
            {
                this.Logger.Error($"會員帳號[{model.UniformNumbers} {model.Phone}]統一編號輸入錯誤！");
                return this.Json(new BaseResult<string>
                {
                    Data = string.Empty,
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "統一編號輸入錯誤"
                    }
                });
            }

            // 檢核電話輸入值是否為數字
            if (!model.Phone.Regex(RegexEnum.Phone))
            {
                this.Logger.Error($"會員帳號[{model.UniformNumbers} {model.Phone}]帳號輸入錯誤！");
                return this.Json(new BaseResult<string>
                {
                    Data = string.Empty,
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "帳號輸入錯誤"
                    }
                });
            }

            try
            {
                // 檢核帳號密碼
                BaseResult<ResultViewModel> result = await HomeService.ReSetPassWord(model);

                return this.Json(new BaseResult<string>
                {
                    Data = result.Data.ALERT_MSG,
                    Result = new ResultModel
                    {
                        ReturnCode = result.Data.RTN_CD,
                        Alert = result.Result.Alert
                    }
                });
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{model.UniformNumbers} {model.Phone}]密碼重置失敗，原因為：{e.Message}！！！");

                return this.Json(new BaseResult<string>
                {
                    Data = string.Empty,
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "密碼已重置失敗"
                    }
                });
            }
        }

        /// <summary>
        /// 取得客服留言
        /// </summary>
        /// <param name="model"></param>
        /// <returns>查詢結果</returns>
        [HttpPost]
        public async Task<JsonResult> GetMessage()
        {
            try
            {
                object model = new
                {
                    ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID,
                };

                BaseListResult<CustomerMessageViewModel> baseListResult = await CustomerService.GetMessage(model);

                return this.Json(baseListResult);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]取得客服留言失敗，原因為：{e.Message}！！！");

                return this.Json(new BaseListResult<CustomerMessageViewModel>
                {
                    Data = new List<CustomerMessageViewModel>(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "取得客服留言失敗"
                    }
                });
            }
        }

        /// <summary>
        /// 回覆留言
        /// </summary>
        /// <param name="message">回覆留言</param>
        /// <returns>留言狀態</returns>
        [HttpPost]
        public async Task<JsonResult> ReplyMessage(string message)
        {
            try
            {
                object model = new
                {
                    ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID,
                    MEMBER_MSG = message,
                };

                ResultViewModel result = await CustomerService.ReplyMessage(model);

                return this.Json(result);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]回覆留言失敗，原因為：{e.Message}！！！");

                return this.Json(new ResultModel
                {
                    ReturnCode = -1,
                    Alert = "回覆留言失敗"
                });
            }
        }

        /// <summary>
        /// 忘記密碼
        /// </summary>
        /// <returns></returns>
        public ActionResult ForgetPW()
        {
            return this.View();
        }

        /// <summary>
        /// 使用者資料取得
        /// </summary>
        /// <returns>停機公告訊息</returns>
        [HttpPost]
        public JsonResult GetUserProfile()
        {
            try
            {
                VendorOperator vendorOperator = (VendorOperator)this.OperatorContent.Operator;

                return this.Json(new BaseResult<VendorOperator>
                {
                    Data = vendorOperator,
                    Result = new ResultModel
                    {
                        ReturnCode = 0,
                        Alert = string.Empty
                    }
                });
            }
            catch (Exception e)
            {
                this.Logger.Error($"使用者資料取得失敗，原因為：{e.Message}！！！");

                // TODO 在後端API開發完成前，此處一律回覆停機資訊
                return this.Json(new BaseResult<VendorOperator>
                {
                    Data = new VendorOperator(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "使用者資料取得失敗。"
                    }
                });
            }
        }

        /// <summary>
        /// App 登入的查詢撥款
        /// </summary>
        /// <param name="Login_ID">會員編號</param>
        /// <param name="App_ID">CCFAPP_Vondor</param>
        /// <param name="Verify_code">登入時取得的VERIFYCODE</param>
        /// <param name="Type">登入裝置類型</param>
        /// <returns>導頁網址</returns>
        public async Task<ActionResult> GrantQuery(string Login_ID, string App_ID, string Verify_code, string from)
        {
            try
            {
                string url = await this.AppLogin(AppLoginTypeEnum.GrantQuery, Login_ID, App_ID, Verify_code, from);

                if (this.OperatorContent.Operator != null)
                {
                    HttpCookie StudentCookies = new HttpCookie("login_mobile_type");
                    StudentCookies.Value = ((VendorOperator)this.OperatorContent.Operator).MobileTypeNnum.ToString().ToLower();
                    StudentCookies.Expires = DateTime.Now.AddHours(1);
                    this.Response.Cookies.Add(StudentCookies);
                }

                return new RedirectResult(url);
            }
            catch (Exception ex)
            {
                this.Logger.Error($"APP會員登入帳號[{Login_ID}]登入失敗，原因為：{ex.Message}！！！");

                return new RedirectResult("~/Home/Error");
            }
        }

        /// <summary>
        /// App 登入的線上請款
        /// </summary>
        /// <param name="Login_ID">會員編號</param>
        /// <param name="App_ID">CCFAPP_Vondor</param>
        /// <param name="Verify_code">登入時取得的VERIFYCODE</param>
        /// <param name="Type">登入裝置類型</param>
        /// <returns>導頁網址</returns>
        public async Task<ActionResult> OnlineCharge(string Login_ID, string App_ID, string Verify_code, string from)
        {
            try
            {
                string url = await this.AppLogin(AppLoginTypeEnum.OnlineCharge, Login_ID, App_ID, Verify_code, from);

                if (this.OperatorContent.Operator != null)
                {
                    HttpCookie StudentCookies = new HttpCookie("login_mobile_type");
                    StudentCookies.Value = ((VendorOperator)this.OperatorContent.Operator).MobileTypeNnum.ToString().ToLower();
                    StudentCookies.Expires = DateTime.Now.AddHours(1);
                    this.Response.Cookies.Add(StudentCookies);
                }

                return new RedirectResult(url);
            }
            catch (Exception ex)
            {
                this.Logger.Error($"APP會員登入帳號[{Login_ID}]登入失敗，原因為：{ex.Message}！！！");

                return new RedirectResult("~/Home/Error");
            }
        }

        /// <summary>
        /// App 登入的線上填單
        /// </summary>
        /// <param name="Login_ID">會員編號</param>
        /// <param name="App_ID">CCFAPP_Vondor</param>
        /// <param name="Verify_code">登入時取得的VERIFYCODE</param>
        /// <param name="Type">登入裝置類型</param>
        /// <returns>導頁網址</returns>
        public async Task<ActionResult> OrderDetail(string Login_ID, string App_ID, string Verify_code, string from)
        {
            try
            {
                string url = await this.AppLogin(AppLoginTypeEnum.OrderDetail, Login_ID, App_ID, Verify_code, from);

                if (this.OperatorContent.Operator != null)
                {
                    HttpCookie StudentCookies = new HttpCookie("login_mobile_type");
                    StudentCookies.Value = ((VendorOperator)this.OperatorContent.Operator).MobileTypeNnum.ToString().ToLower();
                    StudentCookies.Expires = DateTime.Now.AddHours(12);
                    this.Response.Cookies.Add(StudentCookies);
                }

                return new RedirectResult(url);
            }
            catch (Exception ex)
            {
                this.Logger.Error($"APP會員登入帳號[{Login_ID}]登入失敗，原因為：{ex.Message}！！！");

                return new RedirectResult("~/Home/Error");
            }
        }

        /// <summary>
        /// applogin 檢核及取得相對應指定頁面網址
        /// </summary>
        /// <param name="appLoginTypeEnum">相對應指定葉建</param>
        /// <param name="Login_ID">會員編號</param>
        /// <param name="App_ID">CCFAPP_Vondor</param>
        /// <param name="Verify_code">登入時取得的VERIFYCODE</param>
        /// <param name="Type">登入裝置類型</param>
        /// <returns>相對應指定頁面網址</returns>
        private async Task<string> AppLogin(AppLoginTypeEnum appLoginTypeEnum, string Login_ID, string App_ID, string Verify_code, string from)
        {
            try
            {
                BaseResult<string> loginCheckResult = HomeService.AppLogin(Login_ID, App_ID, Verify_code);
                string url = "~/Home/Logout";

                if (loginCheckResult.Result.ReturnCode == 0)
                {
                    LoginViewModel model = new LoginViewModel
                    {
                        MemberId = Login_ID,
                        Token = ConfigurationManager.AppSettings["Token"]
                    };

                    BaseResult<LoginResultViewModel> baseResult = await HomeService.CheckLogin(model);

                    if (baseResult.Data?.RtnCd == 0)
                    {
                        url = "~" + EnumAttributeHelper.GetEnumDescription(appLoginTypeEnum);
                        MobileTypeEnum mobileTypeEnum = MobileTypeEnum.Web;
                        if (from.ToLower() == "ios")
                        {
                            mobileTypeEnum = MobileTypeEnum.Ios;
                        }

                        if (from.ToLower() == "android")
                        {
                            mobileTypeEnum = MobileTypeEnum.Android;
                        }

                        this.OperatorContent.Operator = new VendorOperator
                        {
                            IS_EP_VENDER = baseResult.Data.IsEpVender,
                            APPROVE_POLICY = baseResult.Data.ApprovePolicy,
                            DEFAULT_PROD_NME = baseResult.Data.DefaultProdName,
                            EP_RATE_TYPE_1 = baseResult.Data.EpRateType1,
                            EP_RATE_TYPE_2 = baseResult.Data.EpRateType2,
                            IS_ROLE01_TYPE = baseResult.Data.IsRole01Type,
                            IS_ROLE02_TYPE = baseResult.Data.IsRole02Type,
                            MEMBER_ID = baseResult.Data.MemberId,
                            TOP_VENDER_ID = baseResult.Data.TopVenderId,
                            TOP_VENDER_NME = baseResult.Data.TopVenderName,
                            VENDER_ID = baseResult.Data.VenderId,
                            VENDER_NME = baseResult.Data.VenderName,
                            VENDER_STATUS_CD = baseResult.Data.VenderStatusCd,
                            VEND_SALE_NME = baseResult.Data.VendSaleName,
                            VNO2 = baseResult.Data.Vno2,
                            MENU_LIST = baseResult.Data.Mnmus,
                            IsMobile = mobileTypeEnum != MobileTypeEnum.Web,
                            MobileTypeNnum = mobileTypeEnum,
                            EpLstOnceAmt = baseResult.Data.EpLstOnceAmt,
                        };
                    }
                }

                return url;
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{Login_ID}]登入失敗，原因為：{e.Message}！！！");

                return "~/Home/Error";
            }
        }

        public ActionResult Error()
        {
            return this.View();
        }

        public ActionResult Error500()
        {
            return this.View();
        }

        public ActionResult Terms()
        {
            return this.View();
        }

        public ActionResult Logout()
        {
            this.OperatorContent.Operator = null;
            HttpCookie CaptchaCookie = this.Request.Cookies["login_mobile_type"];
            if (CaptchaCookie != null)
            {
                this.ViewBag.Login_Mobile_Type = CaptchaCookie.Value;
            }

            return this.View();
        }

        /// <summary>
        /// 檢核驗證碼
        /// </summary>
        /// <param name="code">驗證碼</param>
        /// <returns>檢核結果</returns>
        private bool CheckCode(string code, out string output)
        {
            HttpCookie CaptchaCookie = this.Request.Cookies["SupplierPlatform"];
            string Salted = "lutrrasadf1234zxcv";
            string hashValue = code.ToUpper() + Salted;
            output = string.Empty;

            if (CaptchaCookie == null)
            {
                this.Logger.Debug("逾時過久，導致驗證碼為NULL！！！");
                output = "驗證碼逾時";

                return false;
            }

            this.Logger.Info($"本次登入驗證碼為：{CaptchaCookie.Value}！！！");

            return hashValue.SHA256() != CaptchaCookie.Values["captcha"] ? false : true;
        }
    }
}
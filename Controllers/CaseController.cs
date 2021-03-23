using Infrastructure.LoggerService;
using Infrastructure.LoggerService.Services;
using Infrastructure.UploadService;
using Newtonsoft.Json;
using SupplierPlatform.Controllers;
using SupplierPlatform.Entities;
using SupplierPlatform.Enums;
using SupplierPlatform.Extensions;
using SupplierPlatform.Models;
using SupplierPlatform.Models.Dto;
using SupplierPlatform.Models.ViewModel;
using SupplierPlatform.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SupplierPlatform.Content
{
    /// <summary>
    /// 案件相關 Controller
    /// </summary>
    public class CaseController : AuthController
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILoggerProvider Logger = new NLogger(" CaseController");

        /// <summary>
        /// 登入者資訊
        /// </summary>
        private readonly IOperatorContext OperatorContent;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="_operatorContent"></param>
        public CaseController(IOperatorContext _operatorContent)
        {
            this.OperatorContent = _operatorContent ?? throw new ArgumentNullException(nameof(_operatorContent));
        }

        // GET: Case
        public ActionResult Index()
        {
            return this.View();
        }

        public async Task<ActionResult> CaseDetail()
        {
            string caseNumber = this.Request.QueryString["case_number"];
            string orderSN = this.Request.QueryString["order_sn"];
            string merchantId = this.Request.QueryString["merchant_id"];
            CaseDetailViewModel model = new CaseDetailViewModel
            {
                MemberId = ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID,
                CaseNumber = caseNumber,
                OrderId = orderSN,
                MerchantId = merchantId
            };

            BaseResult<CaseDetailResultViewModel> detail = await CaseService.GetCaseDetail(model);
            BaseResult<AdjustCaseDtoModel> adjustInfo = new BaseResult<AdjustCaseDtoModel>();
            this.ViewBag.CaseDetail = detail;
            if ((string.Equals(detail.Data.IsAdjustPriceOrReturn, "Y", StringComparison.OrdinalIgnoreCase) 
              || string.Equals(detail.Data.IsCalculateCancellationFees, "Y", StringComparison.OrdinalIgnoreCase)) 
              && detail.Data.CaseNumber != null)
            {
                JsonResult _info = await this.AdjustCaseInfo(detail.Data.CaseNumber);
                adjustInfo = (BaseResult<AdjustCaseDtoModel>)_info.Data;
            }

            this.ViewBag.AdjustInfo = adjustInfo;
            return this.View();
        }

        public ActionResult CaseInquiry()
        {
            ViewBag.IsBranch = ((VendorOperator)this.Operator).IsBranch;
            return this.View();
        }

        public ActionResult NotOpen()
        {
            return this.View();
        }

        public ActionResult FillIn()
        {
            ViewBag.IsBranch = ((VendorOperator)this.Operator).IsBranch;
            return this.View();
        }

        public ActionResult Returns()
        {
            return this.View();
        }

        /// <summary>
        /// 最新案件
        /// </summary>
        /// <returns>最新案件訊息</returns>
        [HttpPost]
        public async Task<JsonResult> LatestCase()
        {
            try
            {
                object model = new
                {
                    ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID,
                    INDEX = 1,
                    PAGE_NUM = 20,
                    STATUS = new string[] { }
                };

                BaseResult<LatestCaseViewModel> baseListResult = await CaseService.LatestCase(model);

                baseListResult.Data.CASE_LIST.Select(o => { o.Status = o.Status.ConvertLatestCaseStatus(); return o; }).ToList();

                return this.Json(baseListResult);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]最新案件查詢失敗，原因為：{e.Message}！！！");

                return this.Json(new BaseResult<LatestCaseViewModel>
                {
                    Data = new LatestCaseViewModel(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "最新案件查詢失敗"
                    }
                });
            }
        }

        /// <summary>
        /// 案件歷程
        /// </summary>
        /// <returns>最新案件訊息</returns>
        [HttpPost]
        public async Task<JsonResult> GetHistory(string caseNumber)
        {
            try
            {
                object model = new
                {
                    ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID,
                    CASE_ID = caseNumber
                };

                BaseListResult<CaseHistoryViewModel> baseListResult = await CaseService.GetHistory(model);

                return this.Json(baseListResult);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]案件歷程取得失敗，原因為：{e.Message}！！！");

                return this.Json(new BaseListResult<CaseHistoryViewModel>
                {
                    Data = new List<CaseHistoryViewModel>(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "案件歷程取得失敗"
                    }
                });
            }
        }

        /// <summary>
        /// 案件留言
        /// </summary>
        /// <param name="message">案件留言</param>
        /// <returns>案件留言結果</returns>
        [HttpPost]
        public async Task<JsonResult> SendMessage(string message, string caseNumber)
        {
            try
            {
                object model = new
                {
                    ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID,
                    APP_NO = caseNumber,
                    CASE_MSG = message
                };

                BaseResult<CaseSendMessageViewModel> baseResult = await CaseService.SendMessage(model);

                return this.Json(baseResult);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]案件留言發送失敗，原因為：{e.Message}！！！");

                return this.Json(new BaseResult<CaseSendMessageViewModel>
                {
                    Data = new CaseSendMessageViewModel(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "案件留言發送失敗"
                    }
                });
            }
        }

        /// <summary>
        /// 取得調降金額案件資訊
        /// </summary>
        /// <param name="caseNumber">案件編號</param>
        /// <returns>案件明細</returns>
        [HttpPost]
        public async Task<JsonResult> AdjustCaseInfo(string caseNumber)
        {
            try
            {
                object model = new
                {
                    ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID,
                    APP_NO = caseNumber,
                    ABT_DT = DateTime.Now.ToString("yyyyMMdd")
                };

                BaseResult<AdjustCaseDtoModel> baseResponse = await CaseService.AdjustCaseInfo(model);

                this.Session["AdjustCase"] = JsonConvert.SerializeObject(baseResponse.Data);

                return this.Json(baseResponse);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]調降金額案件資訊取得失敗，原因為：{e.Message}！！！");

                return this.Json(new BaseResult<AdjustCaseInfoViewModel>
                {
                    Data = new AdjustCaseInfoViewModel(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "調降金額案件資訊取得失敗"
                    }
                });
            }
        }

        /// <summary>
        /// 調降金額確認
        /// </summary>
        /// <param name="caseNumber">案件編號</param>
        /// <param name="price">調降金額</param>
        /// <returns>調降結果</returns>
        [HttpPost]
        public async Task<JsonResult> AdjustPrice(int newTransactionAmount, string caseNumber, string isChange)
        {
            try
            {
                string data = (string)this.Session["AdjustCase"];

                AdjustCaseDtoModel adjustInfo = JsonConvert.DeserializeObject<AdjustCaseDtoModel>(data);

                CancellationFeeCalculationViewModel model = new CancellationFeeCalculationViewModel
                {
                    MemberId = ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID,
                    TerminateDt = DateTime.Now.ToString("yyyyMMdd"),
                    TerminateBalance = adjustInfo.TerminateBalance,
                    TerminatePaymentAmount = adjustInfo.TerminatePaymentAmount,
                    TerminatePeriod = adjustInfo.TerminatePeriod,
                    TopVondorId = ((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID,
                    Period = adjustInfo.Period,
                    AppNo = caseNumber,
                    IsChange = isChange,
                    IsMotorcycle = adjustInfo.IsMotorcycle,
                    FinancialAdvisoryAmount = adjustInfo.FinancialAdvisoryAmount,
                    AppropriationAmount = adjustInfo.AppropriationAmount,
                    ManagementAmount = adjustInfo.ManagementAmount,
                    DaysFromAppropriationDate = adjustInfo.DaysFromAppropriationDate,
                    NewTransactionAmount = newTransactionAmount,
                    EarnestMoney = adjustInfo.EarnestMoney,
                    PaidAmount = adjustInfo.PaidAmount
                };

                BaseResult<AdjustPriceResultViewModel> baseResult = await CaseService.AdjustPrice(model);

                return this.Json(baseResult);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]調降金額確認失敗，原因為：{e.Message}！！！");

                return this.Json(new BaseResult<AdjustPriceResultViewModel>
                {
                    Data = new AdjustPriceResultViewModel(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "調降金額確認失敗"
                    }
                });
            }
        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="caseNumber">案件編號</param>
        /// <param name="price">調降金額</param>
        /// <returns>調降結果</returns>
        [HttpPost]
        public async Task<JsonResult> Returns(string case_number, int price, int refund, string isChange)
        {
            try
            {
                string data = (string)this.Session["AdjustCase"];

                AdjustCaseDtoModel adjustInfo = JsonConvert.DeserializeObject<AdjustCaseDtoModel>(data);

                CancellationFeeCalculationViewModel adjustModel = new CancellationFeeCalculationViewModel
                {
                    MemberId = ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID,
                    TerminateDt = DateTime.Now.ToString("yyyyMMdd"),
                    TerminateBalance = adjustInfo.TerminateBalance,
                    TerminatePaymentAmount = adjustInfo.TerminatePaymentAmount,
                    TerminatePeriod = adjustInfo.TerminatePeriod,
                    TopVondorId = ((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID,
                    Period = adjustInfo.Period,
                    AppNo = case_number,
                    IsChange = isChange,
                    IsMotorcycle = adjustInfo.IsMotorcycle,
                    FinancialAdvisoryAmount = adjustInfo.FinancialAdvisoryAmount,
                    AppropriationAmount = adjustInfo.AppropriationAmount,
                    ManagementAmount = adjustInfo.ManagementAmount,
                    DaysFromAppropriationDate = adjustInfo.DaysFromAppropriationDate,
                    NewTransactionAmount = price,
                    EarnestMoney = adjustInfo.EarnestMoney,
                    PaidAmount = adjustInfo.PaidAmount
                };

                // 調整金額前先做規範驗證，確認是否可以進行金額調整
                BaseResult<AdjustPriceResultViewModel> baseResult = await CaseService.AdjustPrice(adjustModel);
                if (baseResult.Result.ReturnCode == 0)
                {
                    CaseReturnViewModel caseReturnViewModel = new CaseReturnViewModel
                    {
                        AppNo = case_number,
                        TerminateDt = DateTime.Now.ToString("yyyyMMdd"),
                        TerminateAmount = baseResult.Data.ABT_AMT,
                        NewTransactionAmount = price,
                        IsChange = isChange,
                        HandlingFee = baseResult.Data.HNDL_FEE,
                        CancelMethod = refund
                    };

                    // 進行退款
                    ResultModel result = await CaseService.Returns(caseReturnViewModel);

                    return this.Json(result);
                }

                return this.Json(new ResultModel
                {
                    ReturnCode = -1,
                    Alert = "輸入金額大於案件金額，請重新輸入"
                });
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]退款失敗，原因為：{e.Message}！！！");

                return this.Json(new ResultModel
                {
                    ReturnCode = -1,
                    Alert = "退款失敗"
                });
            }
        }

        /// <summary>
        /// 下載通知函
        /// </summary>
        /// <param name="caseNumber">案件編號</param>
        /// <returns>案件明細</returns>
        [HttpPost]
        public FileStreamResult DownloadDocument(string case_number)
        {
            try
            {
                if (case_number == null)
                    return null;

                (byte[] result, string filename) = CaseService.DownloadDocument(((VendorOperator)this.OperatorContent.Operator).MEMBER_ID, case_number);
                Stream filestream = new MemoryStream(result);

                Response.Headers.Add("Filename", filename);

                return new FileStreamResult(filestream, "application/pdf");
            }
            catch (Exception e)
            {
                this.Response.StatusCode = 400;
                return null;
            }
        }

        private string ConvertStatus(int status)
        {
            switch (status)
            {
                case 1: //尚未操作
                    return "001";

                case 2:  //審核中
                    return "002";

                case 3:  //婉拒(未通過)
                    return "006";

                case 4:  //取消案件
                    return "021";

                case 5:  //已核准未請款
                    return "003";

                case 6:  //請款中
                    return "004";

                case 7:  //已撥款
                    return "005";

                case 8:  //已核准處理中
                    return "024";

                case 9:  //退貨
                    return "022";

                case 10: // 調降金額處理中
                    return "023";

                default:
                    return "";
            }
        }

        private List<OrderInquiryParamsStatus> StatusMap(List<int> status)
        {
            if (status == null) return new List<OrderInquiryParamsStatus>();
            var d = status.Select(o => new OrderInquiryParamsStatus { STATUS = ConvertStatus(o) }).ToList();
            return d;
        }

        private int OrderDaysConvert(int day)
        {
            switch (day)
            {
                case 2: //近3天
                    return 3;

                case 3: //近30天
                    return 30;

                case 4: //近60天
                    return 60;

                default:
                case 1: //當日
                    return 1;
            }
        }

        /// <summary>
        /// 訂單/案件查詢
        /// </summary>
        /// <param name="model"></param>
        /// <returns>查詢結果</returns>
        [HttpPost]
        public async Task<JsonResult> OrderInquiry(CaseInquiryViewModel param)
        {
            try
            {
                OrderInquiryParamsViewModel orderInquiryParams = new OrderInquiryParamsViewModel
                {
                    MEMBER_ID = ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID,
                    INDEX = param.datas.PageNo,
                    PAGE_NUM = param.datas.PageSize,
                    ORDER_ID = null,
                    STATUS = this.StatusMap(param.datas.Status),
                    VENDER_ID = param.datas.Store,
                    CUST_ID = null,
                    CUST_NME = null,
                    APP_NO = null,
                    LINC_NO = null,
                };

                if (param.datas.DateType == 1 && param.datas.RangeDate != null)
                {
                    orderInquiryParams.ORDER_DAYS = this.OrderDaysConvert((int)param.datas.RangeDate);
                }

                // 分公司自帶 VenderId
                if (((VendorOperator)this.OperatorContent.Operator).IsBranch)
                {
                    orderInquiryParams.VENDER_ID = ((VendorOperator)this.OperatorContent.Operator).VENDER_ID;
                }

                if (param.datas.DateType == 2)
                {
                    orderInquiryParams.ORDER_DT_START = param.datas.StartDate?.Replace("-", "");
                    orderInquiryParams.ORDER_DT_END = param.datas.EndDate?.Replace("-", "");
                }

                switch (param.datas.SearchType)
                {
                    case 1: //訂單編號
                        orderInquiryParams.ORDER_ID = param.datas.Search;
                        break;

                    case 2: //身分證字號
                        orderInquiryParams.CUST_ID = param.datas.Search;
                        break;

                    case 3: //姓名
                        orderInquiryParams.CUST_NME = param.datas.Search;
                        break;

                    case 4: //案件編號
                        orderInquiryParams.APP_NO = param.datas.Search;
                        break;

                    case 5: //車牌號碼
                        orderInquiryParams.LINC_NO = param.datas.Search;
                        break;
                }

                BaseResult<LatestCaseViewModel> baseResult = await CaseService.GetOrderInquiry(orderInquiryParams);

                return this.Json(baseResult);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]訂單/案件查詢失敗，原因為：{e.Message}！！！");

                return this.Json(new BaseResult<LatestCaseViewModel>
                {
                    Data = new LatestCaseViewModel(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "訂單/案件查詢失敗"
                    }
                });
            }
        }

        /// <summary>
        /// 取消案件
        /// </summary>
        /// <param name="caseNumber">案件編號</param>
        /// <returns>案件取消結果</returns>
        [HttpPost]
        public async Task<JsonResult> CancelCase(string caseNumber)
        {
            try
            {
                object model = new
                {
                    ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID,
                    APP_NO = caseNumber
                };

                ResultModel result = await CaseService.CancelCase(model);

                return this.Json(result);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]取消案件失敗，原因為：{e.Message}！！！");

                return this.Json(new ResultModel
                {
                    ReturnCode = -1,
                    Alert = "取消案件失敗"
                });
            }
        }

        /// <summary>
        /// 線上填單
        /// </summary>
        /// <param name="model">線上填單 ViewModel</param>
        /// <returns>執行結果</returns>
        [HttpPost]
        public async Task<JsonResult> FillIn(CaseFillInViewModel model)
        {
            // 經辦人員手機號碼有填寫則需檢核
            if (!string.IsNullOrEmpty(model.ManagerPhone) && !model.ManagerPhone.Regex(RegexEnum.Phone))
            {
                this.Logger.Error($"經辦人員手機號碼[{model.Phone} ]輸入錯誤！");
                return this.Json(new ResultModel
                {
                    ReturnCode = -1,
                    Alert = "經辦人員手機號碼輸入錯誤"
                });
            }

            // 手機號碼有填寫則需檢核
            if (!string.IsNullOrEmpty(model.Phone) && !model.Phone.Regex(RegexEnum.Phone))
            {
                this.Logger.Error($"手機號碼[{model.Phone} ]輸入錯誤！");
                return this.Json(new ResultModel
                {
                    ReturnCode = -1,
                    Alert = "手機號碼輸入錯誤"
                });
            }

            // 前端沒有輸入訂單編號時，後端自行產生訂單編號
            if (string.IsNullOrEmpty(model.OrderId))
            {
                model.OrderId = (DateTime.Now.AddHours(-8) - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds.ToString().Split('.')[0];
            }

            // 分公司自帶 VenderId
            if (((VendorOperator)this.OperatorContent.Operator).IsBranch)
            {
                model.VenderId = ((VendorOperator)this.OperatorContent.Operator).VENDER_ID;
                model.VenderName = ((VendorOperator)this.OperatorContent.Operator).VENDER_NME;
            }

            object productModel = new
            {
                ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID,
                ((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID,
                FEE_TYPE = model.InterestRate
            };

            BaseResult<ProductInformationViewModel> baseResult = await CaseService.GetProductInformation(productModel);

            string productType = baseResult.Data.Items.Where(x => x.SeqId == model.ProductType).Select(y => y.ProductType).FirstOrDefault();

            try
            {
                VendorOperator vendorOperator = (VendorOperator)this.OperatorContent.Operator;
                model.MemberId = vendorOperator.MEMBER_ID;
                model.TopVenderId = vendorOperator.TOP_VENDER_ID;
                model.TopVenderName = vendorOperator.TOP_VENDER_NME;
                model.ProductType = int.Parse(productType);

                ResultModel result = await CaseService.FillIn(model);

                if (result.ReturnCode == 0)
                {
                    result.ReturnMsg = model.OrderId;
                }

                return this.Json(result);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]線上填單失敗，原因為：{e.Message}！！！");

                return this.Json(new ResultModel
                {
                    ReturnCode = -1,
                    Alert = "線上填單失敗"
                });
            }
        }

        /// <summary>
        /// 取得門市資訊
        /// </summary>
        /// <returns>門市資訊</returns>
        public async Task<JsonResult> GetStoreInformation()
        {
            try
            {
                if (((VendorOperator)this.OperatorContent.Operator).IsBranch)
                {
                    return this.Json(new BaseListResult<StoreInformationViewModel>
                    {
                        Data = new List<StoreInformationViewModel>
                        {
                            new StoreInformationViewModel
                            {
                                Level = "2",
                                VenderId = ((VendorOperator)this.OperatorContent.Operator).VENDER_ID,
                                VenderName = ((VendorOperator)this.OperatorContent.Operator).VENDER_NME,
                                Venders = new List<StoreInformationViewModel>()
                            }
                        },
                        Result = new ResultModel
                        {
                            ReturnCode = 0
                        }
                    });
                }

                object model = new
                {
                    ((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID,
                    ((VendorOperator)this.OperatorContent.Operator).VENDER_ID
                };

                BaseListResult<StoreInformationViewModel> baseListResult = await CaseService.GetStoreInformation(model);

                return this.Json(baseListResult);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]取得門市資訊失敗，原因為：{e.Message}！！！");

                // TODO 在後端API開發完成前，此處一律回覆停機資訊

                return this.Json(new BaseListResult<StoreInformationViewModel>
                {
                    Data = new List<StoreInformationViewModel>(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "取得門市資訊失敗"
                    }
                });
            }
        }

        /// <summary>
        /// 取得商品資訊
        /// </summary>
        /// <returns>商品資訊</returns>
        public async Task<JsonResult> GetProductInformation(int interestRate)
        {
            try
            {
                object model = new
                {
                    ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID,
                    ((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID,
                    FEE_TYPE = interestRate
                };

                BaseResult<ProductInformationViewModel> baseResult = await CaseService.GetProductInformation(model);

                return this.Json(baseResult);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]取得商品資訊失敗，原因為：{e.Message}！！！");

                return this.Json(new BaseResult<ProductInformationViewModel>
                {
                    Data = new ProductInformationViewModel(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "取得商品資訊失敗"
                    }
                });
            }
        }

        /// <summary>
        /// 取得分期資訊
        /// </summary>
        /// <returns>分期資訊</returns>
        public async Task<JsonResult> GetPeriodNumInformation(string productType, string updateDt, int majSeqId, int seqId)
        {
            if (productType == null)
                productType = "0"; // 暫時修正用

            try
            {
                object model = new
                {
                    ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID,
                    ((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID,
                    EC_PROD_TYPE_CD = productType,
                    EC_QUOTE_CHG_DT = updateDt,
                    EC_QUOTE_MAJ_SEQ_ID = majSeqId,
                    EC_QUOTE_SUB_SEQ_ID = seqId
                };

                BaseResult<PeriodNumInformationViewModel> baseResult = await CaseService.GetPeriodNumInformation(model);

                return this.Json(baseResult);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]取得分期資訊失敗，原因為：{e.Message}！！！");

                return this.Json(new BaseResult<ProductInformationViewModel>
                {
                    Data = new ProductInformationViewModel(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "取得分期資訊失敗"
                    }
                });
            }
        }

        /// <summary>
        /// 取得計算金額
        /// </summary>
        /// <returns>分期資訊</returns>
        public JsonResult Calculator(int transAmt, double interestRate, int periodNum)
        {
            try
            {
                BaseResult<CalculatorResultViewModel> baseResult = CaseService.AmountCalculation(transAmt, interestRate, periodNum);

                return this.Json(baseResult);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]取得計算金額失敗，原因為：{e.Message}！！！");

                return this.Json(new BaseResult<CalculatorResultViewModel>
                {
                    Data = new CalculatorResultViewModel(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "取得計算金額失敗"
                    }
                });
            }
        }

        private string GetCaseType(int case_type)
        {
            switch (case_type)
            {
                case 1:
                    return "紙本";

                case 2:
                    return "電子";

                default:
                    return "";
            }
        }

        private string GetStatus(int status)
        {
            switch (status)
            {
                case 1:
                    return "尚未操作";

                case 2:
                    return "審核中";

                case 3:
                    return "婉拒(未通過)";

                case 4:
                    return "取消案件";

                case 5:
                    return "已核准未請款";

                case 6:
                    return "請款中";

                case 7:
                    return "已撥款";

                case 8:
                    return "已核准處理中";

                case 9:
                    return "退貨";

                case 10:
                    return "調降金額處理中";

                default:
                    return "";
            }
        }

        /// <summary>
        /// 匯出當前明細
        /// </summary>
        /// <param name="model"></param>
        /// <returns>查詢結果</returns>
        [HttpPost]
        public FileStreamResult GetCurrentDetail(CaseGetCurrentDetailViewModel model)
        {
            ExeclService execlService = new ExeclService();

            string[] title = { "申請日期", "門市", "訂單編號", "案件編號", "狀態", "申請人", "交易金額", "期數", "案件類型" };
            string[][] rowData;

            if (model.datas == null)
            {
                rowData = new string[][] { new string[] { "", "", "", "", "", "", "", "", "" } };
            }
            else
            {
                rowData = model.datas.Select(x => new string[]
                {
                        x.Date,
                        x.Store,
                        x.OrderSN,
                        x.CaseSN,
                        GetStatus(x.Status),
                        x.Applicant,
                        x.Price,
                        x.Period,
                        GetCaseType(x.CaseType)
                }).ToArray();
            }

            FileStreamResult fileStreamResult = execlService.GetCurrentDetail(title, rowData);

            return fileStreamResult;
        }

        public class SaveUploadFilesParams
        {
            /// <summary>
            /// 供應商帳號MEMBER_ID
            /// </summary>
            public string MEMBER_ID { get; set; }

            /// <summary>
            /// 送件編號
            /// </summary>
            public string CASE_ID { get; set; }

            /// <summary>
            /// CASE_TODO_TYPE
            /// </summary>
            public string CASE_TODO_TYPE { get; set; }

            /// <summary>
            /// 備註事項
            /// </summary>
            public string TODO_REMARK { get; set; }

            public List<FILE_LIST> FileList { get; set; }
        }

        public class FILE_LIST
        {
            public string FileUID { get; set; }
            public string FileParam { get; set; }
            public string FileType { get; set; }
        }

        /// <summary>
        /// 圖片上傳
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UploadAdditional(string remark, string case_id, string todo_type)
        {
            string result = "";
            List<FILE_LIST> files = new List<FILE_LIST>();
            try
            {
                if (remark == null || case_id == null || case_id == todo_type)
                {
                    this.Response.StatusCode = 400;
                    return this.Json(new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "上傳失敗"
                    });
                }

                ChaileaseUpload chaileaseUpload = new ChaileaseUpload();

                foreach (string s in this.Request.Files)
                {
                    HttpPostedFileBase file = this.Request.Files[s];

                    if (this.IsImage(file))
                    {
                        byte[] data;
                        using (Stream inputStream = file.InputStream)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                inputStream.Position = 0;
                                inputStream.CopyTo(memoryStream);
                                data = memoryStream.ToArray();
                            }
                        }

                        string uuid = chaileaseUpload.Upload(data, file.FileName);

                        files.Add(new FILE_LIST
                        {
                            FileUID = uuid,
                        });
                    }
                    else
                    {
                        result = null;
                        break;
                    }
                }

                var model = new SaveUploadFilesParams
                {
                    CASE_ID = case_id,
                    CASE_TODO_TYPE = todo_type,
                    MEMBER_ID = ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID.ToString(),
                    TODO_REMARK = remark,
                    FileList = files,
                };

                BaseResult<SaveUploadFilesViewModel> baseResult = await CaseService.SaveUploadFiles(model);

                if (result == null)
                {
                    this.Response.StatusCode = 400;
                    return this.Json(new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "上傳失敗"
                    });
                }

                if (baseResult.Data.RtnCd == 0)
                {
                    this.Response.StatusCode = 200;
                    return this.Json(new ResultModel
                    {
                        ReturnCode = 0,
                        Alert = baseResult.Data.AlertMsg,
                    });
                }
                else
                {
                    this.Response.StatusCode = 400;
                    return this.Json(new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = baseResult.Data.AlertMsg
                    });
                }
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex.StackTrace);
                this.Logger.Error(ex.Message);
                this.Response.StatusCode = 400;

                return this.Json(new ResultModel
                {
                    ReturnCode = -1,
                    Alert = "上傳失敗"
                });
            }
        }

        /// <summary>
        /// 圖檔檢核
        /// </summary>
        /// <param name="postedFile">待上傳圖檔</param>
        /// <returns></returns>
        private bool IsImage(HttpPostedFileBase postedFile)
        {
            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (postedFile.ContentType.ToLower() != "image/jpg" &&
                        postedFile.ContentType.ToLower() != "image/jpeg" &&
                        postedFile.ContentType.ToLower() != "image/pjpeg" &&
                        postedFile.ContentType.ToLower() != "image/gif" &&
                        postedFile.ContentType.ToLower() != "image/x-png" &&
                        postedFile.ContentType.ToLower() != "image/png")
            {
                return false;
            }

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            if (Path.GetExtension(postedFile.FileName).ToLower() != ".jpg"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".png"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".gif"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg")
            {
                return false;
            }

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                if (!postedFile.InputStream.CanRead)
                {
                    return false;
                }

                //if (postedFile.ContentLength < ImageMinimumBytes)
                //{
                //    return false;
                //}

                byte[] buffer = new byte[512];
                postedFile.InputStream.Read(buffer, 0, 512);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            //-------------------------------------------
            //  Try to instantiate new Bitmap, if .NET will throw exception
            //  we can assume that it's not a valid image
            //-------------------------------------------

            try
            {
                using (Bitmap bitmap = new Bitmap(postedFile.InputStream))
                {
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
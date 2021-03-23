using Newtonsoft.Json;
using NLog;
using SupplierPlatform.Models;
using SupplierPlatform.Models.ViewModel;
using System.Threading.Tasks;

namespace SupplierPlatform.Services
{
    public class HomeService : BaseService
    {
        private HomeService()
        {
        }

        private static string TableName = "Project";

        private static HomeService instance;

        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public static HomeService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HomeService();
                }
                return instance;
            }
        }

        /// <summary>
        /// 登入檢核
        /// </summary>
        /// <param name="model">取得會員資訊 Input</param>
        /// <returns>取得會員資訊 Output</returns>
        public static async Task<BaseResult<LoginResultViewModel>> CheckLogin(LoginViewModel model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER/VENDER_WEB_LOGIN";
            BaseResult<LoginResultViewModel> result = await ApiService.Instance.ApiCheckLogin(model, apiUrl);

            if (result.Data.RtnCd != 0)
            {
                return new BaseResult<LoginResultViewModel>
                {
                    Data = new LoginResultViewModel(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "登入失敗，請聯繫客服人員，謝謝"
                    }
                };
            }

            return result;
        }

        /// <summary>
        /// APP 登入檢核
        /// </summary>
        /// <param name="Login_ID">會員編號</param>
        /// <param name="App_ID">CCFAPP_Vondor</param>
        /// <param name="Verify_code">登入時取得的 VERIFYCODE</param>
        /// <returns>登入驗證結果</returns>
        public static BaseResult<string> AppLogin(string Login_ID, string App_ID, string Verify_code)
        {
            Logger.Info($"input argu => Login_ID : {Login_ID} App_ID: {App_ID},  Verify_code:{Verify_code} ");
            APP_WSServiceReference.APP_WSSoapClient aPP_WSSoap = new APP_WSServiceReference.APP_WSSoapClient();
            string resultJson = aPP_WSSoap.CHECK_VERIFY_CODE_S(Login_ID, App_ID, Verify_code);
            Logger.Info($"resultJson => {resultJson}");
            BaseResult<string> baseResult = new BaseResult<string>();
            if (!string.IsNullOrEmpty(resultJson))
            {
                baseResult = JsonConvert.DeserializeObject<BaseResult<string>>(resultJson);
            }

            return baseResult;
        }

        /// <summary>
        /// 停機公告
        /// </summary>
        /// <returns>取得停機公告資訊 Output</returns>
        public static async Task<ResultModel> DowntimeCheck()
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/Announcement/v1/DownTime";

            object model = new
            {
                AppId = "CCFAPP_VENDER"
            };

            BaseResult<DownDtoModel> result = await ApiService.Instance.ApiDowntimeCheck(model, apiUrl);

            if (result.Result.ReturnCode == -1)
            {
                return new ResultModel
                {
                    ReturnCode = -1,
                    Alert = result.Result.Alert
                };
            }

            return new ResultModel
            {
                ReturnCode = 0,
                Alert = result.Result.Alert,
                ReturnMsg = result.Data.DownTimeMessage ?? string.Empty
            };
        }

        /// <summary>
        /// 系統公告
        /// </summary>
        /// <returns>取得系統公告資訊 Output</returns>
        public static async Task<ResultModel> SystemNotification()
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_APP_FORM_LOGIN/GET_LOGIN_INFO";

            object model = new
            {
                PUSH_OBJ = "1"
            };

            BaseResult<LoginInfoViewModel> result = await ApiService.Instance.ApiSystemNotification(model, apiUrl);

            if (result.Result.ReturnCode != 0)
            {
                return new ResultModel
                {
                    ReturnCode = -1,
                    Alert = "系統公告取得失敗"
                };
            }

            return new ResultModel
            {
                ReturnCode = result.Result.ReturnCode,
                Alert = result.Data.LoginInfo
            };
        }

        /// <summary>
        /// 忘記(重設)密碼
        /// </summary>
        /// <param name="model">取得會員資訊 Input</param>
        /// <returns>取得會員資訊 Output</returns>
        public static async Task<BaseResult<ResultViewModel>> ReSetPassWord(LoginBaseModel model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER/VENDER_FORGET_PWD";
            BaseResult<ResultViewModel> result = await ApiService.Instance.ApiForgetPW(model, apiUrl);

            if (result.Data == null)
            {
                return new BaseResult<ResultViewModel>
                {
                    Data = new ResultViewModel
                    {
                        RTN_CD = -1,
                        ALERT_MSG = "密碼已重置失敗"
                    },
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "密碼已重置失敗"
                    }
                };
            }

            return result;
        }
    }
}
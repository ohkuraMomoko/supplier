using NLog;
using SupplierPlatform.Models;
using SupplierPlatform.Models.ViewModel;
using System.Threading.Tasks;

namespace SupplierPlatform.Services
{
    public class AccountService
    {
        private AccountService()
        {
        }

        private static AccountService instance;

        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public static AccountService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccountService();
                }
                return instance;
            }
        }

        /// <summary>
        /// 修改密碼
        /// </summary>
        /// <param name="model">取得會員資訊 Input</param>
        /// <returns>取得會員資訊 Output</returns>
        public static async Task<BaseResult<ResultViewModel>> ChangePW(ChangePassWordViewModel model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER/VENDER_CHANGE_PWD";
            BaseResult<ResultViewModel> baseResult = await ApiService.Instance.ApiChangePW(model, apiUrl);
            ResultViewModel result = new ResultViewModel
            {
                RTN_CD = -1,
                ALERT_MSG = "修改密碼失敗"
            };

            if (baseResult.Data == null)
            {
                baseResult.Data = result;
            }

            return baseResult;
        }
    }
}
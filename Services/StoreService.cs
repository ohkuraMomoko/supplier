using NLog;
using SupplierPlatform.Models;
using SupplierPlatform.Models.ViewModel;
using System;
using System.Threading.Tasks;

namespace SupplierPlatform.Services
{
    /// <summary>
    /// 商店相關服務
    /// </summary>
    public class StoreService
    {
        private StoreService()
        {
        }

        private static StoreService instance;

        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public static StoreService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StoreService();
                }
                return instance;
            }
        }

        /// <summary>
        /// 建立或修改商店
        /// </summary>
        /// <returns>最新案件訊息</returns>
        public static async Task<ResultModel> Update(StoreViewModel model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/Store/Create";

            if (model.Action == 1)
            {
                apiUrl = "/Store/Modify";
            }

            ResultModel result = await ApiService.Instance.ApiUpdateStore(model, apiUrl);

            return result;
        }

        /// <summary>
        /// 發佈商家
        /// </summary>
        /// <returns>執行狀態</returns>
        public static async Task<BaseResult<string>> Publish(object model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/Store/Publish";
            BaseResult<string> result = await ApiService.Instance.ApiPublishStore(model, apiUrl);
            if(result.Result.ReturnCode == -1)
            {
                throw new Exception($"發布失敗，{result.Result.ReturnMsg}");
            }
            return result;
        }

        /// <summary>
        /// 取得商店資料
        /// </summary>
        /// <returns>商店資料</returns>
        public static async Task<BaseResult<StoreInfoViewModel>> GetInfo(object model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/Store/Info";
            BaseResult<StoreInfoViewModel> result = await ApiService.Instance.ApiGetStoreInfo(model, apiUrl);

            return result;
        }

        /// <summary>
        /// 取得一頁式電商交易金鑰
        /// </summary>
        /// <param name="model"></param>
        /// <returns>交易金鑰</returns>
        public static async Task<BaseResult<VenderResultViewModel>> GetToken(object model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER/VENDER_WEB_TOKEN";
            BaseResult<VenderResultViewModel> baseResult = await ApiService.Instance.ApiGetToken(model, apiUrl);

            if (baseResult.Result.ReturnCode != 0)
            {
                return new BaseResult<VenderResultViewModel>
                {
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        ReturnMsg = "一頁式電商交易金鑰取得失敗",
                        Alert = "一頁式電商交易金鑰取得失敗"
                    }
                };
            }

            return baseResult;
        }

        /// <summary>
        /// 取得已發布商店資料
        /// </summary>
        /// <returns>商店資料</returns>
        public static async Task<BaseResult<StoreInfoViewModel>> GetPublishInfo(object model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/Store/GetPublish";
            BaseResult<StoreInfoViewModel> result = await ApiService.Instance.ApiGetStoreInfo(model, apiUrl);

            return result;
        }
    }
}
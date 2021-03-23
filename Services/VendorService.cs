using NLog;
using SupplierPlatform.Models;
using SupplierPlatform.Models.Dto;
using SupplierPlatform.Models.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupplierPlatform.Services
{
    /// <summary>
    /// 通知訊息相關服務
    /// </summary>
    public class VendorService
    {
        private VendorService()
        {
        }

        private static string TableName = "Project";

        private static VendorService instance;

        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public static VendorService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new VendorService();
                }
                return instance;
            }
        }

        /// <summary>
        /// 取得通知訊息
        /// </summary>
        /// <returns>通知訊息集合</returns>
        public static async Task<BaseListResult<NotificationResultViewModel>> Notification(NotificationViewModel model, int star, int len, string orde)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER_NOTI/VENDER_GET_NOTI_LIST";
            BaseListResult<NotificationResultViewModel> baseListResult = await ApiService.Instance.ApiNotification(model, apiUrl);

            if (baseListResult.Result.ReturnCode != 0)
            {
                return new BaseListResult<NotificationResultViewModel>
                {
                    Data = new List<NotificationResultViewModel>(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "通知訊息取得失敗"
                    }
                };
            }

            // 原本API回傳回來的是依照時間做倒序
            List<NotificationResultViewModel> notificationResultViewModels = baseListResult.Data;
            List<NotificationResultViewModel> newNotificationResultViewModels = new List<NotificationResultViewModel>();
            if (orde == "asc")
            {
                notificationResultViewModels = notificationResultViewModels.OrderBy(x => x.Date).ToList();
            }

            int isNotRead = notificationResultViewModels.Where(x => !x.IsReady).Count();
            int maxLen = (star + len) > notificationResultViewModels.Count ? notificationResultViewModels.Count : (star + len);

            for (int s = star; s < maxLen; s++)
            {
                newNotificationResultViewModels.Add(notificationResultViewModels[s]);
            }

            baseListResult.Data = newNotificationResultViewModels;
            baseListResult.Result.ReturnMsg = isNotRead.ToString();

            return baseListResult;
        }

        /// <summary>
        /// (單筆或全部)通知訊息狀態變更(刪除或已讀)
        /// </summary>
        /// <param name="id">訊息id</param>
        /// <returns>刪除結果</returns>
        public static async Task<ResultModel> ChangeNotificationStatus(NotificationStatusDtoModel model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER_NOTI/VENDER_UPDATE_NOTI_STATUS";
            ResultModel result = await ApiService.Instance.ApiDeleteNotification(model, apiUrl);

            if (result.ReturnCode != 0)
            {
                return new ResultModel
                {
                    ReturnCode = -1,
                    Alert = "通知訊息處理失敗"
                };
            }

            return result;
        }

        /// <summary>
        /// 同意會員同意條款
        /// </summary>
        /// <param name="model">取得會員資訊 Input</param>
        /// <returns>取得會員資訊 Output</returns>
        public static async Task<ResultModel> Agree(object model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER/VENDER_SET_AGRT";
            ResultModel result = await ApiService.Instance.ApiAgree(model, apiUrl);

            if (result.ReturnCode != 0)
            {
                return new ResultModel
                {
                    ReturnCode = -1,
                    Alert = "同意會員同意條款失敗"
                };
            }

            return result;
        }

        /// <summary>
        /// 取得會員同意條款
        /// </summary>
        /// <param name="model">取得會員資訊 Input</param>
        /// <returns>取得會員資訊 Output</returns>
        public static async Task<BaseResult<AgreeResultViewModel>> GetAgree()
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER/VENDER_GET_AGRT";
            BaseResult<AgreeResultViewModel> baseResult = await ApiService.Instance.ApiGetAgree(null, apiUrl);

            if (baseResult.Result.ReturnCode != 0)
            {
                return new BaseResult<AgreeResultViewModel>
                {
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "會員同意條款取得失敗"
                    }
                };
            }

            return baseResult;
        }

        /// <summary>
        /// 取得產品利率對照
        /// </summary>
        /// <param name="model"></param>
        /// <returns>產品利率對照</returns>
        public static async Task<BaseResult<ProductPeriodViewModel>> GetRate(object model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER_TRANS/GET_EP_RATE_LIST";
            BaseResult<ProductPeriodViewModel> baseResult = await ApiService.Instance.ApiGetRate(null, apiUrl);

            if (baseResult.Result.ReturnCode != 0)
            {
                return new BaseResult<ProductPeriodViewModel>
                {
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "產品利率對照取得失敗"
                    }
                };
            }

            return baseResult;
        }
    }
}
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
    /// 客服相關 服務
    /// </summary>
    public class CustomerService
    {
        private CustomerService()
        {
        }

        private static string TableName = "Project";

        private static CustomerService instance;

        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public static CustomerService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CustomerService();
                }
                return instance;
            }
        }

        /// <summary>
        /// 取得客服留言
        /// </summary>
        /// <returns>客服留言集合</returns>
        public static async Task<BaseListResult<CustomerMessageViewModel>> GetMessage(object model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER_CUST_SERVICE/VENDER_GET_MSG_LIST";
            BaseListResult<CustomerMessageDtoModel> baseListResult = await ApiService.Instance.ApiGetMessage(model, apiUrl);

            if (baseListResult.Result.ReturnCode != 0)
            {
                return new BaseListResult<CustomerMessageViewModel>
                {
                    Data = new List<CustomerMessageViewModel>(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "取得客服留言失敗"
                    }
                };
            }

            int index = 0;
            List<CustomerMessageViewModel> retult = baseListResult.Data.OrderByDescending(y => y.ReplyDt).Select(x => new CustomerMessageViewModel
            {
                Who = x.Who == "Y" ? 2 : 1,
                Reply = x.Reply,
                ReplyDt = x.ReplyDt,
                Sort = index += 1
            }).ToList();

            return new BaseListResult<CustomerMessageViewModel>
            {
                Data = retult,
                Result = new ResultModel
                {
                    ReturnCode = baseListResult.Result.ReturnCode,
                    Alert = baseListResult.Result.Alert
                }
            };
        }

        /// <summary>
        /// 回覆留言
        /// </summary>
        /// <param name="message">回覆留言</param>
        /// <returns>留言狀態</returns>
        public static async Task<ResultViewModel> ReplyMessage(object model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER_CUST_SERVICE/VENDER_SUBMIT_MSG";
            BaseResult<ResultViewModel> baseResult = await ApiService.Instance.ApiReplyMessage(model, apiUrl);

            if (baseResult.Data?.RTN_CD != 0)
            {
                return new ResultViewModel
                {
                    RTN_CD = -1,
                    ALERT_MSG = "回覆留言失敗"
                };
            }

            return baseResult.Data;
        }
    }
}
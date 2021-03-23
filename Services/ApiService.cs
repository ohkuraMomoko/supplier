using SupplierPlatform.Models;
using SupplierPlatform.Models.Dto;
using SupplierPlatform.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SupplierPlatform.Services
{
    public class ApiService : BaseService
    {
        private ApiService()
        {
        }

        private static ApiService instance;

        public static ApiService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ApiService();
                }
                return instance;
            }
        }

        /// <summary>
        /// 取得標的物資料(繳款紀錄)
        /// </summary>
        /// <param name="model">param</param>
        /// <param name="apiUrl">Api 路徑</param>
        /// <returns>result</returns>
        public async Task<BaseListResult<List<LatestCaseViewModel>>> ApiGetLatestCase(object model, string apiUrl)
        {
            BaseListResult<List<LatestCaseViewModel>> result = await this.GetApiResultAsync<BaseListResult<List<LatestCaseViewModel>>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 登入檢核
        /// </summary>
        /// <param name="model">param</param>
        /// <param name="apiUrl">Api 路徑</param>
        /// <returns>result</returns>
        public async Task<BaseResult<LoginResultViewModel>> ApiCheckLogin(LoginViewModel model, string apiUrl)
        {
            BaseResult<LoginResultViewModel> result = await this.GetApiResultAsync<BaseResult<LoginResultViewModel>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 登入檢核
        /// </summary>
        /// <param name="model">param</param>
        /// <param name="apiUrl">Api 路徑</param>
        /// <returns>result</returns>
        public async Task<Result> ApiAppLogin(object model, string apiUrl)
        {
            Result result = await this.GetApiResultAsync<Result>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 停機公告
        /// </summary>
        /// <returns>取得停機公告資訊 Output</returns>
        public async Task<BaseResult<DownDtoModel>> ApiDowntimeCheck(object model, string apiUrl)
        {
            BaseResult<DownDtoModel> result = await this.GetApiResultAsync<BaseResult<DownDtoModel>>(apiUrl, model, true);

            return result;
        }

        /// <summary>
        /// 系統公告
        /// </summary>
        /// <returns>取得系統公告資訊 Output</returns>
        public async Task<BaseResult<LoginInfoViewModel>> ApiSystemNotification(object model, string apiUrl)
        {
            BaseResult<LoginInfoViewModel> result = await this.GetApiResultAsync<BaseResult<LoginInfoViewModel>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 取得會員資訊
        /// </summary>
        /// <param name="model">param</param>
        /// <param name="apiUrl">Api 路徑</param>
        /// <returns>result</returns>
        public async Task<BaseResult<ResultViewModel>> ApiForgetPW(LoginBaseModel model, string apiUrl)
        {
            BaseResult<ResultViewModel> result = await this.GetApiResultAsync<BaseResult<ResultViewModel>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 修改密碼
        /// </summary>
        /// <param name="model">param</param>
        /// <param name="apiUrl">Api 路徑</param>
        /// <returns>result</returns>
        public async Task<BaseResult<ResultViewModel>> ApiChangePW(ChangePassWordViewModel model, string apiUrl)
        {
            BaseResult<ResultViewModel> result = await this.GetApiResultAsync<BaseResult<ResultViewModel>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 同意會員同意條款
        /// </summary>
        /// <param name="model">param</param>
        /// <param name="apiUrl">Api 路徑</param>
        /// <returns>result</returns>
        public async Task<ResultModel> ApiAgree(object model, string apiUrl)
        {
            ResultModel result = await this.GetApiResultAsync<ResultModel>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 同意會員同意條款
        /// </summary>
        /// <param name="model">param</param>
        /// <param name="apiUrl">Api 路徑</param>
        /// <returns>result</returns>
        public async Task<BaseResult<AgreeResultViewModel>> ApiGetAgree(object model, string apiUrl)
        {
            BaseResult<AgreeResultViewModel> result = await this.GetApiResultAsync<BaseResult<AgreeResultViewModel>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 最新案件
        /// </summary>
        /// <returns>最新案件訊息</returns>
        public async Task<BaseResult<LatestCaseViewModel>> ApiLatestCase(object model, string apiUrl)
        {
            BaseResult<LatestCaseViewModel> result = await this.GetApiResultAsync<BaseResult<LatestCaseViewModel>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 案件明細查詢
        /// </summary>
        /// <returns>案件明細</returns>
        public async Task<BaseResult<CaseDetailResultViewModel>> ApiCaseDetail(CaseDetailViewModel model, string apiUrl)
        {
            BaseResult<CaseDetailResultViewModel> result = await this.GetApiResultAsync<BaseResult<CaseDetailResultViewModel>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 案件歷程
        /// </summary>
        /// <returns>最新案件訊息</returns>
        public async Task<BaseListResult<CaseHistoryViewModel>> ApiGetHistory(object model, string apiUrl)
        {
            BaseListResult<CaseHistoryViewModel> result = await this.GetApiResultAsync<BaseListResult<CaseHistoryViewModel>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 案件留言
        /// </summary>
        /// <param name="model">param</param>
        /// <param name="apiUrl">Api 路徑</param>
        /// <returns>result</returns>
        public async Task<BaseResult<CaseSendMessageViewModel>> ApiSendMessage(object model, string apiUrl)
        {
            BaseResult<CaseSendMessageViewModel> result = await this.GetApiResultAsync<BaseResult<CaseSendMessageViewModel>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 訂單/案件查詢
        /// </summary>
        /// <returns>案件明細</returns>
        public async Task<BaseResult<LatestCaseViewModel>> ApiOrderInquiry(object model, string apiUrl)
        {
            BaseResult<LatestCaseViewModel> result = await this.GetApiResultAsync<BaseResult<LatestCaseViewModel>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 取消案件
        /// </summary>
        /// <param name="model">param</param>
        /// <param name="apiUrl">Api 路徑</param>
        /// <returns>result</returns>
        public async Task<BaseResult<ResultViewModel>> ApiCancelCase(object model, string apiUrl)
        {
            BaseResult<ResultViewModel> result = await this.GetApiResultAsync<BaseResult<ResultViewModel>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 取得客服留言
        /// </summary>
        /// <returns>案件明細</returns>
        public async Task<BaseListResult<CustomerMessageDtoModel>> ApiGetMessage(object model, string apiUrl)
        {
            BaseListResult<CustomerMessageDtoModel> result = await this.GetApiResultAsync<BaseListResult<CustomerMessageDtoModel>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 回覆留言
        /// </summary>
        /// <param name="message">回覆留言</param>
        /// <returns>留言狀態</returns>
        public async Task<BaseResult<ResultViewModel>> ApiReplyMessage(object model, string apiUrl)
        {
            BaseResult<ResultViewModel> result = await this.GetApiResultAsync<BaseResult<ResultViewModel>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 取得通知訊息
        /// </summary>
        /// <returns>通知訊息集合</returns>
        public async Task<BaseListResult<NotificationResultViewModel>> ApiNotification(object model, string apiUrl)
        {
            BaseListResult<NotificationResultViewModel> result = await this.GetApiResultAsync<BaseListResult<NotificationResultViewModel>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 刪除通知訊息
        /// </summary>
        /// <param name="model">param</param>
        /// <param name="apiUrl">Api 路徑</param>
        /// <returns>result</returns>
        public async Task<ResultModel> ApiDeleteNotification(object model, string apiUrl)
        {
            ResultModel result = await this.GetApiResultAsync<ResultModel>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 刪除所有通知訊息
        /// </summary>
        /// <param name="model">param</param>
        /// <param name="apiUrl">Api 路徑</param>
        /// <returns>result</returns>
        public async Task<Result> ApiDeleteAllNotification(object model, string apiUrl)
        {
            Result result = await this.GetApiResultAsync<Result>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 設定已讀通知訊息
        /// </summary>
        /// <param name="model">param</param>
        /// <param name="apiUrl">Api 路徑</param>
        /// <returns>result</returns>
        public async Task<Result> ApiReadNotification(object model, string apiUrl)
        {
            Result result = await this.GetApiResultAsync<Result>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 設定已讀所有通知訊息
        /// </summary>
        /// <param name="model">param</param>
        /// <param name="apiUrl">Api 路徑</param>
        /// <returns>result</returns>
        public async Task<ResultModel> ApiReadAllNotification(object model, string apiUrl)
        {
            ResultModel result = await this.GetApiResultAsync<ResultModel>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 線上填單
        /// </summary>
        /// <param name="model">線上填單 ViewModel</param>
        /// <returns>執行結果</returns>
        public async Task<BaseResult<ResultViewModel>> ApiFillIn(CaseFillInViewModel model, string apiUrl)
        {
            BaseResult<ResultViewModel> result = await this.GetApiResultAsync<BaseResult<ResultViewModel>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 取得門市資訊
        /// </summary>
        /// <returns>門市資訊</returns>
        public async Task<BaseListResult<StoreInformationViewModel>> ApiGetStoreInformation(object model, string apiUrl)
        {
            BaseListResult<StoreInformationViewModel> result = await this.GetApiResultAsync<BaseListResult<StoreInformationViewModel>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 取得商品資訊
        /// </summary>
        /// <returns>商品資訊</returns>
        public async Task<BaseResult<ProductInformationViewModel>> ApiGetProductInformation(object model, string apiUrl)
        {
            BaseResult<ProductInformationViewModel> result = await this.GetApiResultAsync<BaseResult<ProductInformationViewModel>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 取得分期利率資訊
        /// </summary>
        /// <returns>分期利率資訊</returns>
        public async Task<BaseResult<PeriodNumInformationViewModel>> ApiGetPeriodNumInformation(object model, string apiUrl)
        {
            BaseResult<PeriodNumInformationViewModel> result = await this.GetApiResultAsync<BaseResult<PeriodNumInformationViewModel>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 補件上傳
        /// </summary>
        /// <returns>門市資訊</returns>
        public async Task<BaseResult<SaveUploadFilesViewModel>> ApiSaveUploadFiles(object model, string apiUrl)
        {
            BaseResult<SaveUploadFilesViewModel> result = await this.GetApiResultAsync<BaseResult<SaveUploadFilesViewModel>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 金額試算
        /// </summary>
        /// <returns>案件明細</returns>
        public async Task<BaseResult<CalculatorResultViewModel>> ApiCalculator(CalculatorViewModel model, string apiUrl)
        {
            BaseResult<CalculatorResultViewModel> result = await this.GetApiResultAsync<BaseResult<CalculatorResultViewModel>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 查詢撥款
        /// </summary>
        /// <returns>案件明細</returns>
        public async Task<BaseResult<AppropriationInquiryResultViewModel>> ApiAppropriationInquiry(AppropriationInquiryDtoModel model, string apiUrl)
        {
            BaseResult<AppropriationInquiryResultViewModel> result = await this.GetApiResultAsync<BaseResult<AppropriationInquiryResultViewModel>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 取得調降金額案件資訊
        /// </summary>
        /// <returns>案件明細</returns>
        public async Task<BaseResult<AdjustCaseDtoModel>> ApiAdjustCaseInfo(object model, string apiUrl)
        {
            BaseResult<AdjustCaseDtoModel> result = await this.GetApiResultAsync<BaseResult<AdjustCaseDtoModel>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 調降金額確認
        /// </summary>
        /// <returns>案件明細</returns>
        public async Task<BaseResult<AdjustPriceResultViewModel>> ApiAdjustPrice(object model, string apiUrl)
        {
            BaseResult<AdjustPriceResultViewModel> baseResult = await this.GetApiResultAsync<BaseResult<AdjustPriceResultViewModel>>(apiUrl, model);

            return baseResult;
        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <returns>案件明細</returns>
        public async Task<BaseResult<ResultViewModel>> ApiReturns(object model, string apiUrl)
        {
            BaseResult<ResultViewModel> result = await this.GetApiResultAsync<BaseResult<ResultViewModel>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 線上請款列表
        /// </summary>
        /// <returns>案件明細</returns>
        public async Task<BaseResult<AskPaymentListResultViewModel>> ApiAskPaymentList(object model, string apiUrl)
        {
            BaseResult<AskPaymentListResultViewModel> result = await this.GetApiResultAsync<BaseResult<AskPaymentListResultViewModel>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 線上請款功能
        /// </summary>
        /// <param name="model">param</param>
        /// <param name="apiUrl">Api 路徑</param>
        /// <returns>result</returns>
        public async Task<BaseResult<ResultViewModel>> ApiSendAskPayment(SendAskPaymentDtoModel model, string apiUrl)
        {
            BaseResult<ResultViewModel> result = await this.GetApiResultAsync<BaseResult<ResultViewModel>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 建立或修改商店
        /// </summary>
        /// <returns>最新案件訊息</returns>
        public async Task<ResultModel> ApiUpdateStore(object model, string apiUrl)
        {
            ResultModel result = await this.GetApiResultAsync<ResultModel>(apiUrl, model, onepage: true);

            return result;
        }

        /// <summary>
        /// 發佈商家
        /// </summary>
        /// <returns>執行狀態</returns>
        public async Task<BaseResult<string>> ApiPublishStore(object model, string apiUrl)
        {
            BaseResult<string> result = await this.GetApiResultAsync<BaseResult<string>>(apiUrl, model, onepage: true);

            return result;
        }

        /// <summary>
        /// 取得商店資料
        /// </summary>
        /// <returns>商店資料</returns>
        public async Task<BaseResult<StoreInfoViewModel>> ApiGetStoreInfo(object model, string apiUrl)
        {
            BaseResult<StoreInfoViewModel> result = await this.GetApiResultAsync<BaseResult<StoreInfoViewModel>>(apiUrl, model, onepage: true);

            return result;
        }

        /// <summary>
        /// 取得商品列表
        /// </summary>
        /// <returns>商品列表</returns>
        public async Task<BaseListResult<ProductInfoViewModel>> ApiGetProductList(object model, string apiUrl)
        {
            BaseListResult<ProductInfoViewModel> result = await this.GetApiResultAsync<BaseListResult<ProductInfoViewModel>>(apiUrl, model, onepage: true);

            return result;
        }

        /// <summary>
        /// 取得商品訂單紀錄列表
        /// </summary>
        /// <returns>商品列表</returns>
        public async Task<BaseListResult<OrderItemDtoModel>> ApiGetOrderItemList(object model, string apiUrl)
        {
            BaseListResult<OrderItemDtoModel> result = await this.GetApiResultAsync<BaseListResult<OrderItemDtoModel>>(apiUrl, model, onepage: true);

            return result;
        }

        /// <summary>
        /// 取得上架商品庫存資訊
        /// </summary>
        /// <returns>商品列表</returns>
        public async Task<BaseListResult<SpecDtoModel>> ApiGetInStock(object model, string apiUrl)
        {
            BaseListResult<SpecDtoModel> result = await this.GetApiResultAsync<BaseListResult<SpecDtoModel>>(apiUrl, model, onepage: true);

            return result;
        }

        /// <summary>
        /// 商品新增或修改
        /// </summary>
        /// <returns>執行狀態</returns>
        public async Task<ResultModel> ApiUpdataProduct(object model, string apiUrl)
        {
            ResultModel result = await this.GetApiResultAsync<ResultModel>(apiUrl, model, onepage: true);

            return result;
        }

        /// <summary>
        /// 商品複製或刪除
        /// </summary>
        /// <returns>執行狀態</returns>
        public async Task<ResultModel> ApiCopyOrDeleteProduct(object model, string apiUrl)
        {
            ResultModel result = await this.GetApiResultAsync<ResultModel>(apiUrl, model, onepage: true);

            return result;
        }

        /// <summary>
        /// 上下架商品
        /// </summary>
        /// <returns>執行狀態</returns>
        public async Task<ResultModel> ApiModifyProductStatus(object model, string apiUrl)
        {
            ResultModel result = await this.GetApiResultAsync<ResultModel>(apiUrl, model, onepage: true);

            return result;
        }

        /// <summary>
        /// 取得一頁式電商交易金鑰
        /// </summary>
        /// <param name="model">param</param>
        /// <param name="apiUrl">Api 路徑</param>
        /// <returns>result</returns>
        public async Task<BaseResult<VenderResultViewModel>> ApiGetToken(object model, string apiUrl)
        {
            BaseResult<VenderResultViewModel> result = await this.GetApiResultAsync<BaseResult<VenderResultViewModel>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 取得產品利率對照
        /// </summary>
        /// <param name="model">param</param>
        /// <param name="apiUrl">Api 路徑</param>
        /// <returns>result</returns>
        public async Task<BaseResult<ProductPeriodViewModel>> ApiGetRate(object model, string apiUrl)
        {
            BaseResult<ProductPeriodViewModel> result = await this.GetApiResultAsync<BaseResult<ProductPeriodViewModel>>(apiUrl, model);

            return result;
        }

        /// <summary>
        /// 取得商品資訊
        /// </summary>
        /// <returns>商品資訊</returns>
        public async Task<BaseResult<ProductInfoViewModel>> ApiGetProductInfo(object model, string apiUrl)
        {
            BaseResult<ProductInfoViewModel> result = await this.GetApiResultAsync<BaseResult<ProductInfoViewModel>>(apiUrl, model, false, true);

            return result;
        }
    }
}
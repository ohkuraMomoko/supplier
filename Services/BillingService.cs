using NLog;
using RestSharp;
using RestSharp.Authenticators;
using SupplierPlatform.Models;
using SupplierPlatform.Models.Dto;
using SupplierPlatform.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SupplierPlatform.Services
{
    /// <summary>
    /// 開票相關服務
    /// </summary>
    public class BillingService
    {
        private BillingService()
        {
        }

        private static BillingService instance;

        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public static BillingService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BillingService();
                }
                return instance;
            }
        }

        /// <summary>
        /// 查詢撥款
        /// </summary>
        /// <param name="model">案件編號</param>
        /// <returns>案件明細</returns>
        public static async Task<BaseResult<AppropriationInquiryResultViewModel>> AppropriationInquiry(AppropriationInquiryDtoModel model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER_CASE/VENDER_WEB_STATEMENT";
            BaseResult<AppropriationInquiryResultViewModel> baseResult = await ApiService.Instance.ApiAppropriationInquiry(model, apiUrl);

            if (baseResult.Data?.RTN_CD != 0)
            {
                return new BaseResult<AppropriationInquiryResultViewModel>
                {
                    Data = new AppropriationInquiryResultViewModel(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "查詢撥款查詢失敗"
                    }
                };
            }

            baseResult.Data.Cases.ForEach(x =>
            {
                x.AppropriationDate = x.AppropriationDate.Substring(0, 4) + "/" + x.AppropriationDate.Substring(4, 2) + "/" + x.AppropriationDate.Substring(6, 2);
            });

            baseResult.Data.Sums.ForEach(x =>
            {
                x.AppropriationDate = x.AppropriationDate.Substring(0, 4) + "/" + x.AppropriationDate.Substring(4, 2) + "/" + x.AppropriationDate.Substring(6, 2);
            });

            return baseResult;
        }

        /// <summary>
        /// 線上請款列表
        /// </summary>
        /// <param name="model">案件編號</param>
        /// <returns>案件明細</returns>
        public static async Task<BaseResult<AskPaymentListResultViewModel>> AskPaymentList(object model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER_CASE/VENDER_WEB_CAPTURE_LIST";
            BaseResult<AskPaymentListResultViewModel> baseResult = await ApiService.Instance.ApiAskPaymentList(model, apiUrl);

            if (baseResult.Data?.RTN_CD != 0)
            {
                return new BaseResult<AskPaymentListResultViewModel>
                {
                    Data = new AskPaymentListResultViewModel(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "線上請款列表查詢失敗"
                    }
                };
            }

            return baseResult;
        }

        /// <summary>
        /// 線上請款功能
        /// </summary>
        /// <param name="caseNumber">案件編號</param>
        /// <returns>請款結果</returns>
        public static async Task<ResultModel> SendAskPayment(List<SendAskPaymentDtoModel> models)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER_CASE/VENDER_WEB_CAPTURE";
            List<string> OrderIds = new List<string>();

            foreach (SendAskPaymentDtoModel model in models)
            {
                BaseResult<ResultViewModel> baseResult = await ApiService.Instance.ApiSendAskPayment(model, apiUrl);
                if (baseResult.Data?.RTN_CD == -1)
                {
                    Logger.Error($"會員帳號[{model.MemberId} 線上請款失敗，原因為：{baseResult.Result.ReturnMsg}！！！");
                    OrderIds.Add(model.OrderId);
                }
            }

            if (OrderIds.Count > 0)
            {
                return new ResultModel
                {
                    ReturnCode = -1,
                    Alert = $"案件編號：{string.Join("、", OrderIds)}，線上請款失敗"
                };
            }

            return new ResultModel
            {
                ReturnCode = 0,
                Alert = "線上請款成功"
            };
        }

        /// <summary>
        /// 下載撥款檔
        /// </summary>
        /// <param name="model">查詢撥款 DtoModel</param>
        /// <returns>撥款檔</returns>
        public static (byte[], string) DownloadDocument(AppropriationInquiryDtoModel model)
        {
            // 透過 Api 把資料送出去
            string account = ConfigurationManager.AppSettings["Account"];
            string passwprd = ConfigurationManager.AppSettings["Passwprd"];
            string url = ConfigurationManager.AppSettings["API_URL"];
            string apiUrl = url + "/EP_VENDER_CASE/VENDER_WEB_DOWNLOAD_STATEMENT";
            HttpResponseMessage result = new HttpResponseMessage();
            RestClient client = new RestClient(apiUrl);
            RestRequest request = new RestRequest(Method.POST);
            client.Authenticator = new HttpBasicAuthenticator(account, passwprd);

            request.AddParameter("MEMBER_ID", model.MemberId);
            request.AddParameter("START_DT", model.StartDt);
            request.AddParameter("END_DT", model.EndDt);
            request.AddParameter("DAYS", model.Days);
            request.AddParameter("ORDER_ID", model.OrderId);
            request.AddParameter("CUST_ID", model.CustId);
            request.AddParameter("CUST_NME", model.CustName);
            request.AddParameter("APP_NO", model.AppNo);
            request.AddParameter("LINC_NO", model.LincNo);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string ContentDisposition = response.Headers.FirstOrDefault(t => t.Name == "Content-Disposition").Value.ToString();
                string filename = Uri.UnescapeDataString(ContentDisposition.Split(';')[1].Replace(" filename=", ""));
                string ex = Path.GetExtension(filename);
                return (response.RawBytes, filename);
                //string ContentDisposition = response.Headers.FirstOrDefault(t => t.Name == "Content-Disposition").Value.ToString();
                //string filename = Uri.UnescapeDataString(ContentDisposition.Split(';')[1].Replace(" filename=", ""));

                //result = new HttpResponseMessage(HttpStatusCode.OK)
                //{
                //    Content = new ByteArrayContent(response.RawBytes)
                //};
                //result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                //{
                //    FileName = filename
                //};
                //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            }
            else
            {
                throw response.ErrorException;
            }
        }
    }
}
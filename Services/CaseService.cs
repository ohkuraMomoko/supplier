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
using System.Threading.Tasks;

namespace SupplierPlatform.Services
{
    /// <summary>
    /// 案件相關
    /// </summary>
    public class CaseService
    {
        private CaseService()
        {
        }

        private static CaseService instance;

        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public static CaseService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CaseService();
                }
                return instance;
            }
        }

        /// <summary>
        /// 最新案件
        /// </summary>
        /// <returns>最新案件訊息</returns>
        public static async Task<BaseResult<LatestCaseViewModel>> LatestCase(object model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER_CASE/VENDER_WEB_CASE_QY";
            BaseResult<LatestCaseViewModel> baseResult = await ApiService.Instance.ApiLatestCase(model, apiUrl);

            if (baseResult.Data?.RTN_CD != 0)
            {
                return new BaseResult<LatestCaseViewModel>
                {
                    Data = new LatestCaseViewModel(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "最新案件查詢失敗"
                    }
                };
            }

            baseResult.Data.CASE_LIST.ForEach(x =>
            {
                x.CreateDt = x.CreateDt.Substring(0, 4) + "/" + x.CreateDt.Substring(4, 2) + "/" + x.CreateDt.Substring(6, 2);
                x.UpdateDt = x.UpdateDt.Substring(0, 4) + "/" + x.UpdateDt.Substring(4, 2) + "/" + x.UpdateDt.Substring(6, 2);
            });

            return baseResult;
        }

        /// <summary>
        /// 案件明細查詢
        /// </summary>
        /// <param name="caseNumber">案件編號</param>
        /// <returns>案件明細</returns>
        public static async Task<BaseResult<CaseDetailResultViewModel>> GetCaseDetail(CaseDetailViewModel model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER_CASE/VENDER_WEB_CASE_DETAIL";
            BaseResult<CaseDetailResultViewModel> baseResult = await ApiService.Instance.ApiCaseDetail(model, apiUrl);

            if (baseResult.Result.ReturnCode != 0)
            {
                return new BaseResult<CaseDetailResultViewModel>
                {
                    Data = new CaseDetailResultViewModel(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "案件明細查詢失敗"
                    }
                };
            }

            string appDate = baseResult.Data.ApplicationDate;
            baseResult.Data.ApplicationDate = appDate.Substring(0, 4) + "/" + appDate.Substring(4, 2) + "/" + appDate.Substring(6, 2);
            baseResult.Data.AppropriationAmount = baseResult.Data.AppropriationAmount ?? "";

            return baseResult;
        }

        /// <summary>
        /// 案件歷程
        /// </summary>
        /// <returns>最新案件訊息</returns>
        public static async Task<BaseListResult<CaseHistoryViewModel>> GetHistory(object model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER_CASE/VENDER_GET_CASE_HIST";
            BaseListResult<CaseHistoryViewModel> baseListResult = await ApiService.Instance.ApiGetHistory(model, apiUrl);

            if (baseListResult.Result.ReturnCode != 0)
            {
                return new BaseListResult<CaseHistoryViewModel>
                {
                    Data = new List<CaseHistoryViewModel>(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "案件歷程取得失敗"
                    }
                };
            }

            return baseListResult;
        }

        /// <summary>
        /// 案件留言
        /// </summary>
        /// <param name="message">案件留言</param>
        /// <returns>案件留言結果</returns>
        public static async Task<BaseResult<CaseSendMessageViewModel>> SendMessage(object model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER_CASE/CASE_SEND_MSG";
            BaseResult<CaseSendMessageViewModel> baseResult = await ApiService.Instance.ApiSendMessage(model, apiUrl);

            if (baseResult.Result.ReturnCode != 0)
            {
                return new BaseResult<CaseSendMessageViewModel>
                {
                    Data = new CaseSendMessageViewModel(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "案件留言發送失敗"
                    }
                };
            }

            return baseResult;
        }

        /// <summary>
        /// 訂單/案件查詢
        /// </summary>
        /// <param name="model">案件編號</param>
        /// <returns>案件明細</returns>
        public static async Task<BaseResult<LatestCaseViewModel>> GetOrderInquiry(object model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER_CASE/VENDER_WEB_CASE_QY";
            BaseResult<LatestCaseViewModel> baseResult = await ApiService.Instance.ApiOrderInquiry(model, apiUrl);

            if (baseResult.Data?.RTN_CD == -1)
            {
                return new BaseResult<LatestCaseViewModel>
                {
                    Data = new LatestCaseViewModel(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "訂單/案件查詢失敗"
                    }
                };
            }

            baseResult.Data.CASE_LIST.ForEach(x =>
            {
                x.CreateDt = x.CreateDt.Substring(0, 4) + "/" + x.CreateDt.Substring(4, 2) + "/" + x.CreateDt.Substring(6, 2);
                x.UpdateDt = x.UpdateDt.Substring(0, 4) + "/" + x.UpdateDt.Substring(4, 2) + "/" + x.UpdateDt.Substring(6, 2);
            });

            return baseResult;
        }

        /// <summary>
        /// 取消案件
        /// </summary>
        /// <param name="caseNumber">案件編號</param>
        /// <returns>案件取消結果</returns>
        public static async Task<ResultModel> CancelCase(object model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER_CASE/VENDER_WEB_CASE_CANCEL";
            BaseResult<ResultViewModel> baseResult = await ApiService.Instance.ApiCancelCase(model, apiUrl);
            ResultViewModel result = new ResultViewModel
            {
                RTN_CD = -1,
                ALERT_MSG = "取消案件失敗"
            };

            if (baseResult.Data?.RTN_CD != 0)
            {
                return new ResultModel
                {
                    ReturnCode = -1,
                    Alert = "取消案件失敗"
                };
            }

            return new ResultModel
            {
                ReturnCode = baseResult.Data.RTN_CD,
                Alert = baseResult.Data.ALERT_MSG
            };
        }

        /// <summary>
        /// 線上填單
        /// </summary>
        /// <param name="model">線上填單 ViewModel</param>
        /// <returns>執行結果</returns>
        public static async Task<ResultModel> FillIn(CaseFillInViewModel model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER_TRANS/VENDER_WEB_ORDER_SUBMIT";
            BaseResult<ResultViewModel> baseResult = await ApiService.Instance.ApiFillIn(model, apiUrl);

            if (baseResult.Data?.RTN_CD != 0)
            {
                return new ResultModel
                {
                    ReturnCode = -1,
                    Alert = baseResult.Data?.ALERT_MSG
                };
            }

            return new ResultModel
            {
                ReturnCode = baseResult.Data.RTN_CD,
                Alert = baseResult.Data.ALERT_MSG
            };
        }

        /// <summary>
        /// 取得門市資訊
        /// </summary>
        /// <returns>門市資訊</returns>
        public static async Task<BaseListResult<StoreInformationViewModel>> GetStoreInformation(object model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER_TRANS/GET_VENDER_LIST";
            BaseListResult<StoreInformationViewModel> baseListResult = await ApiService.Instance.ApiGetStoreInformation(model, apiUrl);

            if (baseListResult.Result.ReturnCode != 0)
            {
                return new BaseListResult<StoreInformationViewModel>
                {
                    Data = new List<StoreInformationViewModel>(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "取得門市資訊失敗"
                    }
                };
            }

            return baseListResult;
        }

        /// <summary>
        /// 取得商品資訊
        /// </summary>
        /// <returns>商品資訊</returns>
        public static async Task<BaseResult<ProductInformationViewModel>> GetProductInformation(object model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER_CASE/VM_VENDER_WEB_PROD_DATA";
            BaseResult<ProductInformationViewModel> baseResult = await ApiService.Instance.ApiGetProductInformation(model, apiUrl);

            if (baseResult.Data.RTN_CD != 0)
            {
                return new BaseResult<ProductInformationViewModel>
                {
                    Data = new ProductInformationViewModel(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "取得商品資訊失敗"
                    }
                };
            }

            if (baseResult.Data.Items != null)
            {
                baseResult.Data.Items.ForEach(x => x.ProductType = string.IsNullOrEmpty(x.ProductType) ? "0" : x.ProductType);

                return baseResult;
            }
            else
            {
                return new BaseResult<ProductInformationViewModel>
                {
                    Data = new ProductInformationViewModel(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "沒有任何商品資訊"
                    }
                };
            }
        }

        /// <summary>
        /// 取得分期利率資訊
        /// </summary>
        /// <returns>分期利率資訊</returns>
        public static async Task<BaseResult<PeriodNumInformationViewModel>> GetPeriodNumInformation(object model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER_CASE/VENDER_WEB_EC_PRD_NUM";
            BaseResult<PeriodNumInformationViewModel> baseResult = await ApiService.Instance.ApiGetPeriodNumInformation(model, apiUrl);

            if (baseResult.Data.RTN_CD != 0)
            {
                return new BaseResult<PeriodNumInformationViewModel>
                {
                    Data = new PeriodNumInformationViewModel(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "取得分期利率資訊失敗"
                    }
                };
            }

            return baseResult;
        }

        /// <summary>
        /// 補件上傳
        /// </summary>
        /// <returns>門市資訊</returns>
        public static async Task<BaseResult<SaveUploadFilesViewModel>> SaveUploadFiles(object model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER_CASE/VENDER_WEB_APPEND_FILE";
            BaseResult<SaveUploadFilesViewModel> baseListResult = await ApiService.Instance.ApiSaveUploadFiles(model, apiUrl);

            if (baseListResult.Result.ReturnCode != 0)
            {
                return new BaseResult<SaveUploadFilesViewModel>
                {
                    Data = new SaveUploadFilesViewModel(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "補件上傳失敗"
                    }
                };
            }

            return baseListResult;
        }

        /// <summary>
        /// 金額試算
        /// </summary>
        /// <param name="model"></param>
        /// <returns>查詢結果</returns>
        public static BaseResult<CalculatorResultViewModel> AmountCalculation(int transAmt, double interestRate, int periodNum)
        {
            int totalPrice = (transAmt + Convert.ToInt32(Math.Round(transAmt * (Convert.ToDouble(interestRate) / 100), 0, MidpointRounding.AwayFromZero)));
            int otherInstallments = Convert.ToInt32(Math.Round(Convert.ToDouble(totalPrice) / Convert.ToDouble(periodNum), 0, MidpointRounding.AwayFromZero));
            int initialPayment = otherInstallments + (totalPrice - otherInstallments * periodNum);

            BaseResult<CalculatorResultViewModel> baseResult = new BaseResult<CalculatorResultViewModel>
            {
                Data = new CalculatorResultViewModel
                {
                    InitialPayment = initialPayment,
                    OtherInstallments = otherInstallments,
                    TotalPrice = totalPrice
                },
                Result = new ResultModel
                {
                    ReturnCode = (initialPayment != 0 && otherInstallments != 0 && totalPrice != 0) ? 0 : -1,
                    Alert = (initialPayment != 0 && otherInstallments != 0 && totalPrice != 0) ? "" : "金額試算錯誤",
                }
            };

            if (baseResult.Result.ReturnCode != 0)
            {
                return new BaseResult<CalculatorResultViewModel>
                {
                    Data = new CalculatorResultViewModel(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "金額試算失敗"
                    }
                };
            }

            return baseResult;
        }

        /// <summary>
        /// 取得調降金額案件資訊
        /// </summary>
        /// <param name="caseNumber">案件編號</param>
        /// <returns>案件明細</returns>
        public static async Task<BaseResult<AdjustCaseDtoModel>> AdjustCaseInfo(object model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER_CASE/VENDER_WEB_CNTRT_DATA";
            BaseResult<AdjustCaseDtoModel> baseResult = await ApiService.Instance.ApiAdjustCaseInfo(model, apiUrl);
            string appDate = string.Empty;

            if (baseResult.Result.ReturnCode != 0)
            {
                if (baseResult.Data != null && !string.IsNullOrEmpty(baseResult.Data?.ALERT_MSG))
                {
                    if (!string.IsNullOrEmpty(baseResult.Data.AppropriationDate))
                    {
                        appDate = baseResult.Data.AppropriationDate;
                        baseResult.Data.AppropriationDate = appDate.Substring(0, 4) + "/" + appDate.Substring(4, 2) + "/" + appDate.Substring(6, 2);

                        return baseResult;
                    }
                }

                return new BaseResult<AdjustCaseDtoModel>
                {
                    Data = new AdjustCaseDtoModel(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "調降金額案件資訊取得失敗",
                        ReturnMsg = baseResult.Data?.ALERT_MSG
                    }
                };
            }

            appDate = baseResult.Data.AppropriationDate;
            baseResult.Data.AppropriationDate = appDate.Substring(0, 4) + "/" + appDate.Substring(4, 2) + "/" + appDate.Substring(6, 2);

            return baseResult;
        }

        /// <summary>
        /// 調降金額確認
        /// </summary>
        /// <param name="caseNumber">案件編號</param>
        /// <param name="price">調降金額</param>
        /// <returns>調降結果</returns>
        public static async Task<BaseResult<AdjustPriceResultViewModel>> AdjustPrice(CancellationFeeCalculationViewModel model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER_CASE/VENDER_WEB_CNTRT_ABT_CAL";
            BaseResult<AdjustPriceResultViewModel> baseResult = await ApiService.Instance.ApiAdjustPrice(model, apiUrl);

            if (baseResult.Data.RTN_CD != 0)
            {
                return new BaseResult<AdjustPriceResultViewModel>
                {
                    Data = new AdjustPriceResultViewModel(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = baseResult.Data.ALERT_MSG
                    }
                };
            }

            return new BaseResult<AdjustPriceResultViewModel>
            {
                Data = baseResult.Data,
                Result = new ResultModel
                {
                    ReturnCode = baseResult.Data.RTN_CD,
                    Alert = baseResult.Data.ALERT_MSG
                }
            };
        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="model">退款相關 ViewModel</param>
        /// <returns>退款結果</returns>
        public static async Task<ResultModel> Returns(CaseReturnViewModel model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER_CASE/VENDER_WEB_CNTRT_ABT_SUBMIT";
            BaseResult<ResultViewModel> baseListResult = await ApiService.Instance.ApiReturns(model, apiUrl);

            if (baseListResult.Data.RTN_CD != 0)
            {
                return new ResultModel
                {
                    ReturnCode = -1,
                    Alert = baseListResult.Data.ALERT_MSG
                };
            }

            return new ResultModel
            {
                ReturnCode = baseListResult.Data.RTN_CD,
                Alert = baseListResult.Data.ALERT_MSG
            };
        }

        /// <summary>
        /// 下載通知函
        /// </summary>
        /// <param name="caseNumber">案件編號</param>
        /// <returns>案件明細</returns>
        public static (byte[], string) DownloadDocument(int memberId, string caseNumber)
        {
            // 透過 Api 把資料送出去
            string account = ConfigurationManager.AppSettings["Account"];
            string passwprd = ConfigurationManager.AppSettings["Passwprd"];
            string url = ConfigurationManager.AppSettings["API_URL"];
            string apiUrl = url + "/EP_VENDER_CASE/VENDER_WEB_DOWNLOAD_APRVNOTICE_PDF";

            HttpResponseMessage result = new HttpResponseMessage();
            RestClient client = new RestClient(apiUrl);
            RestRequest request = new RestRequest(Method.POST);
            client.Authenticator = new HttpBasicAuthenticator(account, passwprd);
            request.AddParameter("MEMBER_ID", memberId);
            request.AddParameter("APP_NO", caseNumber);
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
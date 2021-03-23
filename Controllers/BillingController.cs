using Infrastructure.LoggerService;
using Infrastructure.LoggerService.Services;
using SupplierPlatform.Entities;
using SupplierPlatform.Models;
using SupplierPlatform.Models.Dto;
using SupplierPlatform.Models.ViewModel;
using SupplierPlatform.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SupplierPlatform.Controllers
{
    /// <summary>
    /// 開票相關 Controller
    /// </summary>
    public class BillingController : AuthController
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
        public BillingController(IOperatorContext _operatorContent) : base()
        {
            this.Logger = new NLogger("BillingController");
            this.OperatorContent = _operatorContent ?? throw new ArgumentNullException(nameof(_operatorContent));
        }

        public async Task<ActionResult> AskPayment()
        {
            var data = await AskPaymentList();
            ViewBag.AskPaymentList = data.Data;

            return this.View();
        }

        public ActionResult AppropriationInquiry()
        {
            return this.View();
        }

        public ActionResult PaymentUploadList()
        {
            return this.View();
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
        /// 查詢撥款
        /// </summary>
        /// <param name="model"></param>
        /// <returns>查詢結果</returns>
        [HttpPost]
        public async Task<JsonResult> AppropriationInquiry(AppropriationInquiryViewModel model)
        {
            try
            {
                string errorLog = $"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID} 查詢撥款查詢錯誤！";

                // 檢核是否有輸入查詢條件
                if (model.DateType == 0)
                {
                    if (model.SearchType == 0)
                    {
                        this.Logger.Error(errorLog);
                        return this.Json(new BaseResult<string>
                        {
                            Data = "2",
                            Result = new ResultModel
                            {
                                ReturnCode = -1,
                                Alert = "請輸入查詢條件！"
                            }
                        });
                    }

                    if (string.IsNullOrEmpty(model.Search))
                    {
                        this.Logger.Error(errorLog);
                        return this.Json(new BaseResult<string>
                        {
                            Data = "2",
                            Result = new ResultModel
                            {
                                ReturnCode = -1,
                                Alert = "請輸入查詢條件！"
                            }
                        });
                    }
                }


                AppropriationInquiryDtoModel inputModel = new AppropriationInquiryDtoModel
                {
                    MemberId = ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID,
                    OrderId = model.SearchType == 1 ? model.Search : string.Empty,
                    CustId = model.SearchType == 2 ? model.Search : string.Empty,
                    CustName = model.SearchType == 3 ? model.Search : string.Empty,
                    AppNo = model.SearchType == 4 ? model.Search : string.Empty,
                    LincNo = model.SearchType == 5 ? model.Search : string.Empty,
                    Days = model.DateType == 1 ? this.OrderDaysConvert(model.RangeDate).ToString() : "0",
                    StartDt = model.DateType == 2 ? model.StartDt : string.Empty,
                    EndDt = model.DateType == 2 ? model.EndDt : string.Empty
                };

                BaseResult<AppropriationInquiryResultViewModel> baseResult = await BillingService.AppropriationInquiry(inputModel);

                return this.Json(baseResult);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID} 查詢撥款查詢失敗，原因為：{e.Message}！！！");

                return this.Json(new BaseResult<AppropriationInquiryResultViewModel>
                {
                    Data = new AppropriationInquiryResultViewModel(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "查詢撥款查詢失敗"
                    }
                });
            }
        }

        /// <summary>
        /// 線上請款列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns>請款結果</returns>
        public async Task<JsonResult> AskPaymentList()
        {
            try
            {
                BaseResult<AskPaymentListResultViewModel> baseListResult = await BillingService.AskPaymentList(new { ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID });

                return this.Json(baseListResult);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID} 線上請款列表查詢失敗，原因為：{e.Message}！！！");

                return this.Json(new BaseResult<AskPaymentListResultViewModel>
                {
                    Data = new AskPaymentListResultViewModel(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "線上請款列表查詢失敗"
                    }
                });
            }
        }

        /// <summary>
        /// 線上請款功能
        /// </summary>
        /// <param name="caseNumber">案件編號</param>
        /// <returns>案件取消結果</returns>
        [HttpPost]
        public async Task<JsonResult> SendAskPayment(List<SendAskPaymentDtoModel> models)
        {
            try
            {
                models.ForEach(x => x.MemberId = ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID);
                ResultModel result = await BillingService.SendAskPayment(models);

                return this.Json(result);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID} 線上請款失敗，原因為：{e.Message}！！！");

                return this.Json(new ResultModel
                {
                    ReturnCode = -1,
                    Alert = "線上請款失敗"
                });
            }
        }

        /// <summary>
        /// 下載撥款檔
        /// </summary>
        /// <returns></returns>
        public FileStreamResult DownloadStatement(AppropriationInquiryViewModel model)
        {
            try
            {
                string errorLog = $"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID} 查詢撥款查詢錯誤！";

                // 檢核是否有輸入查詢條件
                // 檢核是否有輸入查詢條件
                if (model.DateType == 0)
                {
                    if (model.SearchType == 0)
                    {
                        this.Logger.Error(errorLog);
                        this.Response.StatusCode = 400;
                        return null;
                    }

                    if (string.IsNullOrEmpty(model.Search))
                    {
                        this.Logger.Error(errorLog);
                        this.Response.StatusCode = 400;
                        return null;
                    }
                }

                AppropriationInquiryDtoModel inputModel = new AppropriationInquiryDtoModel
                {
                    MemberId = ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID,
                    OrderId = model.SearchType == 1 ? model.Search : string.Empty,
                    CustId = model.SearchType == 2 ? model.Search : string.Empty,
                    CustName = model.SearchType == 3 ? model.Search : string.Empty,
                    AppNo = model.SearchType == 4 ? model.Search : string.Empty,
                    LincNo = model.SearchType == 5 ? model.Search : string.Empty,
                    Days = model.DateType == 1 ? this.OrderDaysConvert(model.RangeDate).ToString() : "0",
                    StartDt = model.DateType == 2 ? model.StartDt : string.Empty,
                    EndDt = model.DateType == 2 ? model.EndDt : string.Empty,
                };

                (byte[] result, string filename) = BillingService.DownloadDocument(inputModel);
                Stream filestream = new MemoryStream(result);

                Response.Headers.Add("Filename", filename);
                return new FileStreamResult(filestream, "text/csv");
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]下載撥款檔失敗，原因為：{e.Message}！！！");

                this.Response.StatusCode = 400;
                return null;
            }
        }

        /// <summary>
        /// 報表匯出
        /// </summary>
        /// <param name="model"></param>
        /// <returns>查詢結果</returns>
        public async Task<FileStreamResult> GetCurrentDetail(BillingCurrentDetailViewModel model)
        {
            try
            {

                ExeclService execlService = new ExeclService();

                string[] title = new string[] { "撥款日期", "門市", "訂單編號", "案件編號", "申請人", "期數", "交易金額" };
                string[][] rowData;

                if (model.datas == null)
                {
                    rowData = new string[][] { new string[] { "", "", "", "", "", "", "" } };
                }
                else
                {
                    rowData = model.datas.Select(x => new string[]
                    {
                        x.AppropriationDate,
                        x.Store,
                        x.OrderSN,
                        x.CaseSN,
                        x.Applicant,
                        x.Period,
                        x.Price,
                    }).ToArray();
                }

                FileStreamResult fileStreamResult = execlService.GetCurrentDetail(title, rowData);

                return fileStreamResult;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ActionResult NotOpen()
        {
            return this.View();
        }

        public ActionResult AskPaymentNotOpen()
        {
            return this.View();
        }
    }
}
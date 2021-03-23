using Infrastructure.LoggerService;
using Infrastructure.LoggerService.Services;
using Infrastructure.UploadService;
using SupplierPlatform.Entities;
using SupplierPlatform.Extensions;
using SupplierPlatform.Models;
using SupplierPlatform.Models.Dto;
using SupplierPlatform.Models.ViewModel;
using SupplierPlatform.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SupplierPlatform.Controllers
{
    /// <summary>
    /// 通知訊息相關 Controller
    /// </summary>
    public class VendorController : AuthController
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILoggerProvider Logger = new NLogger("StoreController");

        /// <summary>
        /// 登入者資訊
        /// </summary>
        private readonly IOperatorContext OperatorContent;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="_operatorContent"></param>
        public VendorController(IOperatorContext _operatorContent)
        {
            this.OperatorContent = _operatorContent ?? throw new ArgumentNullException(nameof(_operatorContent));
        }

        // GET: Vendor
        public async Task<ActionResult> Index()
        {
            //object model = new
            //{
            //    ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID,
            //    INDEX = 1,
            //    PAGE_NUM = 20,
            //    STATUS = new string[] { }
            //};

            //BaseResult<LatestCaseViewModel> baseListResult = await CaseService.LatestCase(model);

            //baseListResult.Data.CASE_LIST.Select(o => { o.Status = o.Status.ConvertLatestCaseStatus(); return o; }).ToList();

            //this.ViewBag.LatestCase = baseListResult;

            return this.View();
        }

        public async Task<ActionResult> Agree()
        {
            BaseResult<AgreeResultViewModel> baseResult = await VendorService.GetAgree();

            if (baseResult.Result.ReturnCode == 0)
            {
                this.ViewBag.Agreement = baseResult.Data.Agreement;
            }

            return this.View();
        }

        /// <summary>
        /// 取得會員同意條款
        /// </summary>
        /// <returns>會員同意條款</returns>
        public async Task<JsonResult> GetAgree()
        {
            try
            {
                BaseResult<AgreeResultViewModel> baseResult = await VendorService.GetAgree();

                return this.Json(baseResult);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID} 會員同意條款取得失敗，原因為：{e.Message}！！！");

                return this.Json(new BaseResult<AgreeResultViewModel>
                {
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "會員同意條款取得失敗"
                    }
                });
            }
        }

        /// <summary>
        /// 同意會員同意條款
        /// </summary>
        /// <param name="agree">同意狀態</param>
        /// <returns>執行結果</returns>
        [HttpPost]
        public async Task<JsonResult> Agree(bool agree)
        {
            try
            {
                object model = new
                {
                    ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID
                };

                ResultModel result = await VendorService.Agree(model);

                return this.Json(result);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]同意會員同意條款失敗，原因為：{e.Message}！！！");

                return this.Json(new ResultModel
                {
                    ReturnCode = -1,
                    Alert = "同意會員同意條款失敗"
                });
            }
        }

        /// <summary>
        /// 取得通知訊息
        /// </summary>
        /// <returns>通知訊息集合</returns>
        [HttpPost]
        public async Task<JsonResult> Notification(int start = 0, int len = 20, string order = "desc")
        {
            try
            {
                NotificationViewModel model = new NotificationViewModel
                {
                    MemberId = ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID.ToString(),
                    Index = 0,
                    PageNumber = 0
                };

                BaseListResult<NotificationResultViewModel> baseListResult = await VendorService.Notification(model, start, len, order);

                return this.Json(baseListResult);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID} 通知訊息取得失敗，原因為：{e.Message}！！！");

                return this.Json(new BaseListResult<NotificationResultViewModel>
                {
                    Data = new List<NotificationResultViewModel>(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "通知訊息取得失敗"
                    }
                });
            }
        }

        /// <summary>
        /// 刪除通知訊息
        /// </summary>
        /// <param name="id">訊息id</param>
        /// <returns>刪除結果</returns>
        public async Task<JsonResult> DeleteNotification(string id)
        {
            try
            {
                NotificationStatusDtoModel model = new NotificationStatusDtoModel
                {
                    MemberId = ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID.ToString(),
                    MajSeqId = id,
                    Status = "D"
                };

                // 檢核帳號密碼
                ResultModel result = await VendorService.ChangeNotificationStatus(model);

                return this.Json(result);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]通知訊息刪除失敗，原因為：{e.Message}！！！");

                return this.Json(new ResultModel
                {
                    ReturnCode = -1,
                    Alert = "通知訊息刪除失敗"
                });
            }
        }

        /// <summary>
        /// 刪除所有通知訊息
        /// </summary>
        /// <returns>刪除結果</returns>
        public async Task<JsonResult> DeleteAllNotification()
        {
            try
            {
                NotificationStatusDtoModel model = new NotificationStatusDtoModel
                {
                    MemberId = ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID.ToString(),
                    MajSeqId = string.Empty,
                    Status = "D"
                };

                // 檢核帳號密碼
                ResultModel result = await VendorService.ChangeNotificationStatus(model);

                return this.Json(result);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]所有通知訊息刪除失敗，原因為：{e.Message}！！！");

                return this.Json(new ResultModel
                {
                    ReturnCode = -1,
                    Alert = "所有通知訊息刪除失敗"
                });
            }
        }

        /// <summary>
        /// 設定已讀通知訊息
        /// </summary>
        /// <param name="id">訊息id</param>
        /// <returns>刪除結果</returns>
        public async Task<JsonResult> ReadNotification(string id)
        {
            try
            {
                NotificationStatusDtoModel model = new NotificationStatusDtoModel
                {
                    MemberId = ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID.ToString(),
                    MajSeqId = id,
                    Status = "R"
                };

                // 檢核帳號密碼
                ResultModel result = await VendorService.ChangeNotificationStatus(model);

                return this.Json(result);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]設定已讀通知訊息失敗，原因為：{e.Message}！！！");

                return this.Json(new ResultModel
                {
                    ReturnCode = -1,
                    Alert = "設定已讀通知訊息失敗"
                });
            }
        }

        /// <summary>
        /// 設定已讀所有通知訊息
        /// </summary>
        /// <returns>刪除結果</returns>
        public async Task<JsonResult> ReadAllNotification()
        {
            try
            {
                NotificationStatusDtoModel model = new NotificationStatusDtoModel
                {
                    MemberId = ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID.ToString(),
                    MajSeqId = string.Empty,
                    Status = "R"
                };

                ResultModel result = await VendorService.ChangeNotificationStatus(model);

                return this.Json(result);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]設定已讀所有通知訊息失敗，原因為：{e.Message}！！！");

                return this.Json(new ResultModel
                {
                    ReturnCode = -1,
                    Alert = "設定已讀所有通知訊息失敗"
                });
            }
        }

        /// <summary>
        /// 取得產品利率對照
        /// </summary>
        /// <param name="model"></param>
        /// <returns>產品利率對照</returns>
        public async Task<JsonResult> GetRate()
        {
            try
            {
                object model = new
                {
                    MEMBER_ID = ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID.ToString(),
                };

                BaseResult<ProductPeriodViewModel> result = await VendorService.GetRate(model);

                return this.Json(result);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]設定已讀所有通知訊息失敗，原因為：{e.Message}！！！");

                return this.Json(new ResultModel
                {
                    ReturnCode = -1,
                    Alert = "一頁式電商交易金鑰取得失敗"
                });
            }
        }

        /// <summary>
        /// 圖片上傳
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UploadImage(string remark, string case_id, string todo_type)
        {
            this.Session["uuid"] = string.Empty;
            this.Session["url"] = string.Empty;
            try
            {
                ChaileaseUpload chaileaseUpload = new ChaileaseUpload();
                HttpPostedFileBase file = this.Request.Files[0];
                string topVenderId = ((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID;
                int lastIndexOf = file.FileName.LastIndexOf(".");
                string fileNameExtension = file.FileName.Substring(lastIndexOf, file.FileName.Length - lastIndexOf);
                string fileName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").SHA256().Replace("\\",string.Empty).Substring(0, 10) + fileNameExtension;
                string url = string.Empty;
                string uuid = string.Empty;
                this.Logger.Info($@"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]
                                    進行檔案上傳，原檔案名稱為：{file.FileName}，
                                    編碼後新檔案名稱為：{fileName}，
                                    檔案長度為：{file.ContentLength}");

                if (!this.IsImage(file))
                {
                    this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]檔案上傳失敗，原因為：檢核是否為圖檔失敗！！！");
                    this.Response.StatusCode = 400;
                    return this.Json(new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "上傳失敗"
                    });
                }

                byte[] data;
                using (Stream inputStream = file.InputStream)
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        inputStream.Position = 0;
                        inputStream.CopyTo(memoryStream);
                        url = chaileaseUpload.UploadFileByTopVenderId(topVenderId, memoryStream, fileName, "image");
                        data = memoryStream.ToArray();
                    }
                }

                if (!string.IsNullOrEmpty(url))
                {
                    uuid = chaileaseUpload.Upload(data, file.FileName);
                }

                if (string.IsNullOrEmpty(uuid))
                {
                    this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]檔案上傳失敗，原因為：uuid取得失敗！！！");
                    this.Response.StatusCode = 400;
                    return this.Json(new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "上傳失敗"
                    });
                }

                this.Session["uuid"] = uuid;
                this.Session["url"] = url;
                this.Response.StatusCode = 200;
                return this.Json(new ResultModel
                {
                    ReturnCode = 0,
                    Alert = "圖檔上傳成功"
                });
            }
            catch (Exception ex)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]檔案上傳失敗，原因為：{ex.Message}！！！");
                this.Logger.Error(ex.StackTrace);
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
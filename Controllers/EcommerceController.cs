using Infrastructure.LoggerService;
using Infrastructure.LoggerService.Services;
using Infrastructure.UploadService;
using SupplierPlatform.Entities;
using SupplierPlatform.Enums;
using SupplierPlatform.Extensions;
using SupplierPlatform.Helper;
using SupplierPlatform.Helps;
using SupplierPlatform.Models;
using SupplierPlatform.Models.BaseModels;
using SupplierPlatform.Models.Dto;
using SupplierPlatform.Models.ViewModel;
using SupplierPlatform.Repository;
using SupplierPlatform.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SupplierPlatform.Controllers
{
    public class EcommerceController : AuthController
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILoggerProvider Logger;

        /// <summary>
        /// 登入者資訊
        /// </summary>
        private readonly IOperatorContext OperatorContent;

        public EcommerceController(IOperatorContext _operatorContent)
        {
            this.Logger = new NLogger("EcommerceController");
            this.OperatorContent = _operatorContent ?? throw new ArgumentNullException(nameof(_operatorContent));
        }

        // GET: Ecommerce
        public ActionResult Index()
        {
            return this.View();
        }

        public async Task<ActionResult> MaintainStore()
        {
            StoreInfoViewModel getStore = await EcommerceService.GetStore(((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID);
            bool isEdit = false;

            if (getStore != null)
            {
                isEdit = true;
                if (string.IsNullOrEmpty(getStore.LogoPath))
                {
                    string weburl = ConfigurationManager.AppSettings["Web_Url"];
                    getStore.LogoPath = $"{weburl}Home/RenderImage?id={getStore.LogoFileuuid}";
                }
                this.ViewBag.StoreInfo = getStore;
            }

            this.ViewBag.isEdit = isEdit;
            this.ViewBag.isMobile = ((VendorOperator)this.OperatorContent.Operator).IsMobile;

            return this.View();
        }

        /// <summary>
        /// 商品管理
        /// </summary>
        /// <returns>商品列表</returns>
        public async Task<ActionResult> ProductManage()
        {
            // 檢查有沒有商店
            StoreInfoViewModel getStore = await EcommerceService.GetStore(((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID);
            if (getStore != null)
            {
                this.ViewBag.ProductList = await EcommerceService.GetProductyList(((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID);
                string topVenderId = ((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID;
                string apiUrl = ConfigurationManager.AppSettings["API_file"];
                string fileName = "index";
                string weburl = ConfigurationManager.AppSettings["Web_Url"];
                string filePath = Path.Combine(this.Server.MapPath(apiUrl), topVenderId, fileName);
                string url = Flurl.Url.Combine(new string[] { weburl, apiUrl, topVenderId, fileName });
                this.ViewBag.publishUrl = "";

                if (getStore.DeployStatus == "Y")
                {
                    this.ViewBag.publishUrl = url;
                }

                return this.View();
            }
            else
            {
                return this.RedirectToAction("MaintainStore", "Ecommerce");
            }
        }

        /// <summary>
        /// 商品 新增 / 編輯
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> AddProduct()
        {
            this.ViewBag.Title = "新增商品";

            // 檢查有沒有商店
            StoreInfoViewModel getStore = await EcommerceService.GetStore(((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID);
            if (getStore != null)
            {
                this.Session["ImgLogoList"] = null;
                this.Session["IntroImgList"] = null;

                this.ViewBag.LowPrice = ((VendorOperator)this.OperatorContent.Operator).EpLstOnceAmt;

                PeriodInfoViewModel periodList = await EcommerceService.GetStorePeriod(((VendorOperator)this.OperatorContent.Operator).MEMBER_ID);

                this.ViewBag.PeriodList = periodList;

                if (this.Request.QueryString["product_id"] != null)
                {
                    int productId;
                    ProductInfoViewModel productInfo;
                    if (int.TryParse(this.Request.QueryString["product_id"], out productId))
                    {
                        productInfo = await EcommerceService.GetProduct(productId, ((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID);
                        productInfo.productIntroductType = "1";
                        // imagetype = 3
                        if (productInfo.ProductImages.Where(o => o.ImageType == 3).ToList().Count() > 0)
                        {
                            productInfo.productIntroductType = "1";
                        }
                        if (!string.IsNullOrWhiteSpace(productInfo.ProductLink))
                        {
                            productInfo.productIntroductType = "2";
                        }

                        this.ViewBag.Title = "編輯商品";

                        List<FilebaseModel> Imgfiles = new List<FilebaseModel>();
                        Imgfiles.Add(new FilebaseModel
                        {
                            FileUID = productInfo.ProductFileuuid,
                            FileParam = "",
                            FileType = "2"
                        });
                        //this.Session["ImgList"] = Imgfiles;

                        List<FilebaseModel> IntroImgfiles = new List<FilebaseModel>();

                        IntroImgfiles.AddRange(
                             productInfo.ProductImages.Select(x => new FilebaseModel
                             {
                                 FileUID = x.Uuid,
                                 FileParam = "",
                                 FileType = "3"
                             })
                            );
                    }
                    else
                    {
                        productInfo = null;
                    }

                    this.ViewBag.ProductInfo = productInfo;
                }

                return this.View();
            }
            else
            {
                return this.RedirectToAction("MaintainStore", "Ecommerce");
            }
        }

        /// <summary>
        /// 取得訂單資訊
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<JsonResult> Orders(int page = 1, int pageSize = 20, OnePageStatusEnum? status = null)
        {
            OrderInquiryParamsViewModel orderInquiryParams = new OrderInquiryParamsViewModel
            {
                MEMBER_ID = ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID,
                INDEX = page,
                PAGE_NUM = pageSize,
                VENDER_ID = ((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID,
                IS_EC_QY = "Y",
                STATUS = new List<OrderInquiryParamsStatus> { }
            };

            BaseResult<List<CaseDtoModel>> result = await EcommerceService.GerPageOrders(orderInquiryParams, page, pageSize, status);

            int total = result.Result.Count;
            int totalPage = total / pageSize;
            return this.ResultJson(new BasePageResult<CaseDtoModel>
            {
                Data = result.Data,
                Pagination = new PaginationModel
                {
                    Page = page,
                    PageSize = pageSize,
                    Total = total,
                    TotalPage = total % pageSize == 0 ? totalPage : totalPage + 1
                },
                Result = result.Result
            });
        }

        /// <summary>
        /// 訂單查詢
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> OrderManage()
        {
            // 檢查有沒有商店
            StoreInfoViewModel getStore = await EcommerceService.GetStore(((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID);

            if (getStore == null)
            {
                return this.RedirectToAction("MaintainStore", "Ecommerce");
            }

            return this.View();
        }

        /// <summary>
        /// 商店預覽
        /// </summary>
        /// <returns></returns>
        public ActionResult StorePreview()
        {
            return this.View();
        }

        /// <summary>
        /// 取得商店內容
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> StoreContent()
        {
            this.ViewBag.StoreName = ((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_NME;
            StoreInfoViewModel storeInfo = await EcommerceService.GetStore(((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID);
            this.ViewBag.Title = storeInfo.Title;
            string weburl = ConfigurationManager.AppSettings["Web_Url"];

            if (string.IsNullOrEmpty(storeInfo.LogoPath))
            {
                storeInfo.LogoPath = $"{weburl}/Home/RenderImage?id={storeInfo.LogoFileuuid}";
            }

            this.ViewBag.LogoPath = storeInfo.LogoPath;
            this.ViewBag.ZerocardWebUrl = ConfigurationManager.AppSettings["ZerocardWebUrl"];
            this.ViewBag.StoreInfo = new
            {
                storeInfo.Title,
                storeInfo.SubTitle,
                storeInfo.Phone,
                storeInfo.OtherInstructions,
                storeInfo.Storeid,
                storeInfo.DeliveryNote,
                storeInfo.Address,
                storeInfo.LogoPath,
                storeInfo.StoreName,
                storeInfo.FacebookId,
                storeInfo.LineId,
                storeInfo.IGUrl,
                storeInfo.StoreUrl
            };

            List<ProductInfoViewModel> productList = (await EcommerceService.GetProductyList(((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID)).Where(x => x.ProductStatus == ProductStatusEnum.上架).ToList();

            // 轉移舊有商品資料時，當沒有商品連結，也沒有商品描述時，在預覽的時候預設將主圖放進小圖
            productList.ForEach(x =>
            {
                if (string.IsNullOrEmpty(x.ProductLink) && x.ProductImages.Count == 0)
                {
                    x.ProductImages = new List<StoreProductImageDtoModel>
                    {
                        new StoreProductImageDtoModel
                        {
                            Product_Id = int.Parse(x.ProductId),
                            Store_Id = x.StoreId,
                            ImageType = 3,
                            Uuid = x.ProductImage
                        }
                    };
                }
            });

            if (productList != null)
            {
                var new_productList = productList.Where(o => o.ProductStatus == ProductStatusEnum.上架).Select(o => new
                {
                    o.ProductId,
                    ProductImage = $"{weburl}/Home/RenderImage?id={o.ProductImage}",
                    //o.ProductImage,
                    o.ProductInfo,
                    o.ProductLink,
                    o.ProductName,
                    ProductPeriod = o.ProductPeriod.Select(x => new ProductPeriodTypeForPublish { PeriodId = x.PeriodId, PeriodDescription = x.PeriodDescription, PeriodNum = x.PeriodNum, PeriodType = x.PeriodType }),
                    o.ProductPrice,
                    o.SuggestPrice,
                    ProductSpecs = o.ProductSpecs.Select(p => new ProductSpecsPublishDtoModel { SpecsId = p.SpecsId, SpecsName = p.SpecsName, SpecsInstock = p.SpecsInstock }),
                    o.ProductStatus,
                    o.StoreId,
                    CustomeSpec = o.CustomeSpecs.Select(p => new ProductCustomeSpecsTypeForPublish { SpecsId = p.SpecsId, SpecsName = p.SpecsName }),
                    ProductImages = o.ProductImages.Select(p => new StoreProductImageDtoModel
                    {
                        Uuid = p.Uuid,
                        Store_Id = p.Store_Id,
                        ImageType = p.ImageType,
                        ImagePath = $"{weburl}/Home/RenderImage?id={p.Uuid}"
                    })
                }).ToList();

                this.ViewBag.ProductList = new_productList;

                //var test = productList.Select(k => $"{storeInfo.Storeid}, {k.ProductName}, {k.ProductPrice}, {}, fee_type, {storeInfo.Storeid}, { ((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_NME}").ToList();
                List<OnePageToken> tokens = new List<OnePageToken>();

                string key = ConfigurationManager.AppSettings["ZerocardKey"];
                string iv = ConfigurationManager.AppSettings["ZerocardIV"];
                AesCrypto ase = new AesCrypto(key, iv);

                foreach (ProductInfoViewModel item in productList)
                {
                    foreach (ProductPeriodTypeDtoModel periodItem in item.ProductPeriod)
                    {
                        foreach (ProductSpecsTypeDtoModel specsItem in item.ProductSpecs)
                        {
                            //custome_spec
                            if (item.CustomeSpecs.Count > 0)
                            {
                                foreach (ProductCustomeSpecsTypeDtoModel customeItem in item.CustomeSpecs)
                                {
                                    tokens.Add(new OnePageToken
                                    {
                                        Id = $"{periodItem.PeriodId}_{specsItem.SpecsId}_{customeItem.SpecsId}",
                                        Token = ase.Encryptor($"{storeInfo.Storeid},{storeInfo.StoreName},{item.ProductId},{item.ProductName},{specsItem.SpecsName},{customeItem.SpecsName},{item.ProductPrice},{periodItem.PeriodNum},{EnumAttributeHelper.GetEnumDescription(periodItem.PeriodType)}".Trim())
                                    });
                                }
                            }
                            else
                            {
                                tokens.Add(new OnePageToken
                                {
                                    Id = $"{periodItem.PeriodId}_{specsItem.SpecsId}",
                                    Token = ase.Encryptor($"{storeInfo.Storeid},{storeInfo.StoreName},{item.ProductId},{item.ProductName},{specsItem.SpecsName},{string.Empty},{item.ProductPrice},{periodItem.PeriodNum},{EnumAttributeHelper.GetEnumDescription(periodItem.PeriodType)}".Trim())
                                });
                            }
                        }
                    }
                }

                this.ViewBag.Tokens = tokens;
            }
            else
            {
                this.ViewBag.ProductList = new List<string>();
                this.ViewBag.Tokens = new List<OnePageToken>();
            }

            return this.View();
        }

        /// <summary>
        /// 圖片上傳 - 商品介紹
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UploadImage(string remark, string case_id, string todo_type)
        {
            string result = "";
            int upfiles = 0;
            string topVenderId = ((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID;

            List<FilebaseModel> files = new List<FilebaseModel>();
            try
            {
                if (remark == null || case_id == null || case_id == todo_type)
                {
                    this.Response.StatusCode = 400;
                    return this.Json(new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "上傳失敗"
                    });
                }

                List<String> IntroImgPathList = new List<String>();
                List<String> IntroImgList = new List<String>();
                upfiles = this.Request.Files.Count;

                if (todo_type == "0")
                {
                    this.Session["ImgList"] = null;
                }
                else if (todo_type == "2")
                {
                    this.Session["ImgLogoList"] = null;
                }
                else if (todo_type == "3")
                {
                    this.Session["IntroImgList"] = null;
                }
                //if (upfiles > 1)
                //{
                //    this.Session["IntroImgList"] = null;
                //}
                //else
                //{
                //    this.Session["ImgList"] = null;
                //}

                ChaileaseUpload chaileaseUpload = new ChaileaseUpload();

                foreach (string s in this.Request.Files)
                {
                    HttpPostedFileBase file = this.Request.Files[s];

                    if (this.IsImage(file))
                    {
                        byte[] data;
                        string url = string.Empty;
                        int lastIndexOf = file.FileName.LastIndexOf(".");
                        string oldname = file.FileName.Substring(lastIndexOf, file.FileName.Length - lastIndexOf);
                        string fileName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").SHA256().Replace("\\", string.Empty).Replace("/", string.Empty).Substring(0, 10) + oldname;
                        using (Stream inputStream = file.InputStream)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                inputStream.Position = 0;
                                inputStream.CopyTo(memoryStream);
                                data = memoryStream.ToArray();
                            }
                        }

                        string uuid = new FileRepository().Upload(file.FileName, data);

                        files.Add(new FilebaseModel
                        {
                            FileUID = uuid,
                            FileParam = url,
                            FileType = todo_type
                        });
                    }
                    else
                    {
                        result = null;
                        break;
                    }
                }
                if (todo_type == "0")
                {
                    this.Session["ImgList"] = files;
                }
                else if (todo_type == "2")
                {
                    this.Session["ImgLogoList"] = files;
                }
                else if (todo_type == "3")
                {
                    this.Session["IntroImgList"] = files;
                }

                this.Response.StatusCode = 200;
                return this.Json(new ResultModel
                {
                    ReturnCode = 0,
                    Alert = "圖檔上傳成功"
                });
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex.StackTrace);
                this.Logger.Error(ex.Message);
                this.Response.StatusCode = 400;

                return this.Json(new ResultModel
                {
                    ReturnCode = -1,
                    Alert = "上傳失敗"
                });
            }
        }

        /// <summary>
        /// 圖檔檢核-共用
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

                //if (postedFile.ContentLength < ImageMinimumBytes)
                //{
                //    return false;
                //}

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

        public ActionResult NotOpen()
        {
            return this.View();
        }

        public ActionResult ProductManageNotOpen()
        {
            return this.View();
        }
    }
}
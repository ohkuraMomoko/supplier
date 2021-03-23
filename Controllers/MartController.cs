using Infrastructure.LoggerService;
using Newtonsoft.Json;
using SupplierPlatform.Helper;
using SupplierPlatform.Helps;
using SupplierPlatform.Models;
using SupplierPlatform.Models.Dto;
using SupplierPlatform.Models.ViewModel;
using SupplierPlatform.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SupplierPlatform.Controllers
{
    public class MartController : Controller
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILoggerProvider Logger;

        /// <summary>
        /// 取得動態商店資訊
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Index(string id)
        {
            object model = new
            {
                storeid = id
            };

            BaseResult<StoreInfoViewModel> result = await StoreService.GetPublishInfo(model);

            if (result.Data == null || result.Result.Alert == "該商店並未發布")
            {
                return new RedirectResult("~/Home/Error");
            }

            StoreInfoViewModel storeInfo = result.Data;
            string weburl = ConfigurationManager.AppSettings["Web_Url"];

            if (string.IsNullOrEmpty(storeInfo.LogoPath))
            {
                storeInfo.LogoPath = $"{weburl}/Home/RenderImage?id={storeInfo.LogoFileuuid}";
            }

            this.ViewBag.Title = storeInfo.Title;
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

            List<ProductInfoViewModel> productList = await ProductService.GetPublishProductyList(id);

            // 轉移舊有商品資料時，當沒有商品連結，也沒有商品描述時，在預覽的時候預設將主圖放進小圖
            productList?.ForEach(x =>
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
                var new_productList = productList.Select(o => new
                {
                    o.ProductId,
                    ProductImage = $"{weburl}/Home/RenderImage?id={o.ProductImage}",
                    o.ProductInfo,
                    o.ProductLink,
                    o.ProductName,
                    ProductPeriod = o.ProductPeriod.Select(x => new ProductPeriodTypeForPublish { PeriodId = x.PeriodId, PeriodDescription = x.PeriodDescription, PeriodNum = x.PeriodNum, PeriodType = x.PeriodType }),
                    o.ProductPrice,
                    o.SuggestPrice,
                    ProductSpecs = o.ProductSpecs.Select(p => new ProductSpecsPublishDtoModel { SpecsId = p.SpecsId, SpecsName = p.SpecsName }).ToList(),
                    o.ProductStatus,
                    o.StoreId,
                    CustomeSpec = o.CustomeSpecs.Select(p => new ProductCustomeSpecsTypeForPublish { SpecsId = p.SpecsId, SpecsName = p.SpecsName }).ToList(),
                    ProductImages = o.ProductImages.Select(p => new StoreProductImageDtoModel
                    {
                        Uuid = p.Uuid,
                        Store_Id = p.Store_Id,
                        ImageType = p.ImageType,
                        ImagePath = $"{weburl}/Home/RenderImage?id={p.Uuid}"
                    })
                }).ToList();

                this.ViewBag.ProductList = new_productList;
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
                                        Token = ase.Encryptor($"{storeInfo.Storeid}|{storeInfo.StoreName}|{item.ProductId}|{item.ProductName}|{specsItem.SpecsName}|{customeItem.SpecsName}|{item.ProductPrice}|{periodItem.PeriodNum}|{EnumAttributeHelper.GetEnumDescription(periodItem.PeriodType)}".Trim())
                                    });
                                }
                            }
                            else
                            {
                                tokens.Add(new OnePageToken
                                {
                                    Id = $"{periodItem.PeriodId}_{specsItem.SpecsId}",
                                    Token = ase.Encryptor($"{storeInfo.Storeid}|{storeInfo.StoreName}|{item.ProductId}|{item.ProductName}|{specsItem.SpecsName}|{string.Empty}|{item.ProductPrice}|{periodItem.PeriodNum}|{EnumAttributeHelper.GetEnumDescription(periodItem.PeriodType)}".Trim())
                                });
                            }
                        }
                    }
                }

                string json = JsonConvert.SerializeObject(tokens);

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
        /// 取得商品庫存資訊列表
        /// </summary>
        /// <returns>商品列表</returns>
        [HttpPost]
        public async Task<JsonResult> GetInStock(string storeid, int productId, int specId)
        {
            try
            {
                var model = new
                {
                    storeid
                };

                BaseListResult<SpecDtoModel> result = await ProductService.GetInStock(model);

                if (result.Result.ReturnCode == -1 || result.Data.Count == 0)
                {
                    this.Logger.Error($"商品代碼：{productId}，規格待碼：{specId}，取得庫存資訊失敗，原因為查無資料！！！");

                    return this.Json(new BaseResult<SpecDtoModel>
                    {
                        Data = new SpecDtoModel
                        {
                            Id = specId,
                            Product_Id = productId,
                            StockAmount = "0"
                        },
                        Result = new ResultModel
                        {
                            Count = 1,
                            ReturnCode = 0,
                        },
                    });
                }

                // 取出欲查詢庫存量之商品庫存資訊
                SpecDtoModel specDtoModel = result.Data.Where(x => x.Product_Id == productId && x.Id == specId).FirstOrDefault();

                if (specDtoModel == null)
                {
                    this.Logger.Error($"商品代碼：{productId}，規格待碼：{specId}，取得庫存資訊失敗，原因為與上架商品資料不符！！！");

                    return this.Json(new BaseResult<SpecDtoModel>
                    {
                        Data = new SpecDtoModel
                        {
                            Id = specId,
                            Product_Id = productId,
                            StockAmount = "0"
                        },
                        Result = new ResultModel
                        {
                            Count = 1,
                            ReturnCode = 0,
                        },
                    });
                }

                return this.Json(new BaseResult<SpecDtoModel>
                {
                    Data = specDtoModel,
                    Result = new ResultModel
                    {
                        Count = 1,
                        ReturnCode = 0,
                    },
                });
            }
            catch (Exception e)
            {
                this.Logger.Error($"取得商品庫存資訊失敗，原因為：{e.Message}！！！");

                return this.Json(new BaseListResult<ProductInfoViewModel>
                {
                    Data = new List<ProductInfoViewModel>(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "取得商品列表失敗"
                    }
                });
            }
        }
    }
}
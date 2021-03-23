using Infrastructure.LoggerService;
using Infrastructure.LoggerService.Services;
using SupplierPlatform.Entities;
using SupplierPlatform.Enums;
using SupplierPlatform.Extensions;
using SupplierPlatform.Models;
using SupplierPlatform.Models.BaseModels;
using SupplierPlatform.Models.Dto;
using SupplierPlatform.Models.ViewModel;
using SupplierPlatform.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SupplierPlatform.Controllers
{
    public class ProductController : AuthController
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILoggerProvider Logger = new NLogger("ProductController");

        /// <summary>
        /// 登入者資訊
        /// </summary>
        private readonly IOperatorContext OperatorContent;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="_operatorContent"></param>
        public ProductController(IOperatorContext _operatorContent)
        {
            this.OperatorContent = _operatorContent ?? throw new ArgumentNullException(nameof(_operatorContent));
        }

        public class CalculatePeriodPriceModel
        {
            /// <summary>
            /// 期數
            /// </summary>
            public string Period { get; set; }

            /// <summary>
            /// 分期付款利率(名目利率)
            /// </summary>
            public string Rate { get; set; }

            /// <summary>
            /// 交易金額
            /// </summary>
            public int TotalPrice { get; set; }
        }

        private int CalculatePeriodPrice(CalculatePeriodPriceModel item)
        {
            //計算包含本金的利率
            decimal rate = 1 + Convert.ToDecimal(item.Rate) / 100;
            //計算總金額
            decimal stage_all_price = Math.Round(item.TotalPrice * rate, 0, MidpointRounding.AwayFromZero);
            //分幾期
            decimal period = Convert.ToDecimal(item.Period);
            //計算每期金額
            int other_price = Convert.ToInt32(Math.Round(stage_all_price / period, 0, MidpointRounding.AwayFromZero));
            return other_price;
        }

        /// <summary>
        /// 計算期數總金額
        /// </summary>
        /// <returns>商品列表</returns>
        [HttpPost]
        public async Task<JsonResult> CalculatePeriodPrice(List<CalculatePeriodPriceModel> param)
        {
            try
            {
                var returnData = new List<int>();
                foreach (var item in param)
                {
                    var other_price = CalculatePeriodPrice(item);
                    returnData.Add(other_price);
                }

                return this.Json(returnData);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]計算期數總金額，原因為：{e.Message}！！！");

                return this.Json(new BaseListResult<int>
                {
                    Data = new List<int>(),
                    Result = new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = "計算期數總金額失敗"
                    }
                });
            }
        }

        /// <summary>
        /// 取得商品列表
        /// </summary>
        /// <returns>商品列表</returns>
        [HttpPost]
        public async Task<JsonResult> GetList()
        {
            try
            {
                var model = new
                {
                    storeid = ((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID
                };

                BaseListResult<ProductInfoViewModel> result = await ProductService.GetList(model);

                return this.Json(result);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]取得商品列表失敗，原因為：{e.Message}！！！");

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

        /// <summary>
        /// 商品新增或修改
        /// </summary>
        /// <returns>執行狀態</returns>
        [HttpPost]
        public async Task<JsonResult> AddOrModify(ProductDtoModel model, int action)
        {
            List<FilebaseModel> ImgLogoList = null;
            List<FilebaseModel> IntroImgList = null;
            if (this.Session["ImgLogoList"] != null)
            {
                ImgLogoList = (List<FilebaseModel>)this.Session["ImgLogoList"];
            }

            if (this.Session["IntroImgList"] != null)
            {
                IntroImgList = (List<FilebaseModel>)this.Session["IntroImgList"];
            }

            if ((ImgLogoList != null || model.ProductFileuuid != null))   //&& IntroImgList != null && IntroImgList.Count != 0
            {
                int memberId = ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID;

                PeriodInfoViewModel periodList = await ProductService.GetStorePeriod(memberId);
                // 取利率要丟整個 Model 而且還是用 object ? (暫時不管他)

                Dictionary<string, double> periods = periodList?.PeriodItem?.ToDictionary(e => $"{e.PeriodType}_{e.Period}", e => e.Rate);

                // 如果抓不到分期資料，先不處理? (預期不太可能發生)
                if (periods != null)
                {
                    // 計算分期正確的金額
                    foreach (ProductPeriodTypeDtoModel item in model.ProductPeriod)
                    {
                        int price = this.CalculatePeriodPrice(new CalculatePeriodPriceModel
                        {
                            Period = item.PeriodNum,
                            Rate = periods[$"{item.PeriodType}_{item.PeriodNum}"].ToString(),
                            TotalPrice = model.ProductPrice.TryToInt().GetValueOrDefault()
                        });
                        item.PeriodDescription = $"{price:N0}/期";
                    }
                }

                try
                {
                    // image
                    List<StoreProductImageDtoModel> images = new List<StoreProductImageDtoModel>();

                    // 檢查有沒有新上傳的檔案的 logoPath 或 uuid
                    if (ImgLogoList is null || string.IsNullOrEmpty(ImgLogoList[0].FileUID))
                    {
                        // 取得原本的 logoPath 或 uuid
                        if (string.IsNullOrEmpty(model.ProductFileuuid))
                        {
                            return this.Json(new ResultModel
                            {
                                ReturnCode = -1,
                                Alert = "Logo上傳失敗"
                            });
                        }

                        ImgLogoList = new List<FilebaseModel>();
                        ImgLogoList.Add(new FilebaseModel
                        {
                            FileUID = model.ProductFileuuid
                        });
                    }

                    model.StoreId = ((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID;
                    model.CreateAccount = ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID.ToString();

                    model.ProductPeriod = model.ProductPeriod.OrderBy(e => e.PeriodNum).ToList();

                    // 主圖
                    images.Add(new StoreProductImageDtoModel
                    {
                        Uuid = ImgLogoList[0].FileUID,
                        ImageType = 2,
                        Store_Id = model.StoreId,
                        Product_Id = model.ProductId == null ? 0 : int.Parse(model.ProductId, 0)
                    });

                    // 未刪除的小圖
                    if (model.ProductImages != null && model.ProductImages.Count > 0 && model.ProductType == 1)
                    {
                        for (int i = 0; i < model.ProductImages.Count(); i++)
                        {
                            images.Add(new StoreProductImageDtoModel
                            {
                                Uuid = model.ProductImages[i].Uuid,
                                ImageType = 3,
                                Store_Id = model.StoreId,
                                Product_Id = model.ProductId == null ? 0 : int.Parse(model.ProductId, 0)
                            });
                        }
                    }

                    // 新增的小圖
                    if (IntroImgList != null && IntroImgList.Count > 0 && model.ProductType == 1)
                    {
                        for (int i = 0; i < IntroImgList.Count(); i++)
                        {
                            images.Add(new StoreProductImageDtoModel
                            {
                                Uuid = IntroImgList[i].FileUID,
                                ImageType = 3,
                                Store_Id = model.StoreId,
                                Product_Id = model.ProductId == null ? 0 : int.Parse(model.ProductId, 0)
                            });
                        }
                    }

                    model.ProductImages = images;

                    foreach (StoreProductImageDtoModel item in model.ProductImages)
                    {
                        item.Store_Id = model.StoreId;
                    }

                    ResultModel result = await ProductService.AddOrModify(model, action);

                    return this.Json(result);
                }
                catch (Exception e)
                {
                    this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]商品{(action == 0 ? "建立" : "修改")}失敗，原因為：{e.Message}！！！");

                    return this.Json(new ResultModel
                    {
                        ReturnCode = -1,
                        Alert = $"商品{(action == 0 ? "建立" : "修改")}失敗"
                    });
                }
            }
            else
            {
                return this.Json(new ResultModel
                {
                    ReturnCode = -1,
                    Alert = "商品上傳失敗"
                });
            }
        }

        /// <summary>
        /// 複製或刪除商品
        /// action 0: 複製 1: 刪除 （預設0）
        /// </summary>
        /// <returns>執行狀態</returns>
        [HttpPost]
        public async Task<JsonResult> CopyOrDelete(string[] productids, CopyOrDeleteEnum action)
        {
            try
            {
                string storeid = ((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID;
                int memberId = ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID;

                ResultModel result = await ProductService.CopyOrDelete(memberId, productids, action, storeid);

                return this.Json(result);
            }
            catch (Exception e)
            {
                string func_name = (action == 0) ? "複製" : "刪除";
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}] {func_name}商品失敗，原因為：{e.Message}！！！");

                return this.Json(new ResultModel
                {
                    ReturnCode = -1,
                    Alert = $"{func_name}商品失敗"
                });
            }
        }

        /// <summary>
        /// 上下架商品
        /// status : 上架：true 下架：false
        /// </summary>
        /// <returns>執行狀態</returns>
        [HttpPost]
        public async Task<JsonResult> SetProductStatus(string[] productids, bool status)
        {
            try
            {
                string storeid = ((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID;
                int memberId = ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID;

                ResultModel result = await ProductService.ModifyStatus(memberId, productids, status, storeid);

                return this.Json(result);
            }
            catch (Exception e)
            {
                string func_name = status ? "上架" : "下架";
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]{func_name}商品失敗，原因為：{e.Message}！！！");

                return this.Json(new ResultModel
                {
                    ReturnCode = -1,
                    Alert = $"{func_name}商品失敗"
                });
            }
        }
    }
}
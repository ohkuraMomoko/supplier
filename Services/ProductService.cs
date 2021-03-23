using NLog;
using SupplierPlatform.Enums;
using SupplierPlatform.Models;
using SupplierPlatform.Models.Dto;
using SupplierPlatform.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupplierPlatform.Services
{
    public class ProductService
    {
        private ProductService()
        {
        }

        private static ProductService _instance;

        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public static ProductService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ProductService();
                }
                return _instance;
            }
        }

        /// <summary>
        /// 取得商品資訊
        /// </summary>
        /// <returns>商品列表</returns>
        public static async Task<BaseResult<ProductInfoViewModel>> GetInfo(object model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/Product/GetInfo";
            BaseResult<ProductInfoViewModel> result = await ApiService.Instance.ApiGetProductInfo(model, apiUrl);

            return await Task.FromResult(result);
        }

        /// <summary>
        /// 取得商品列表
        /// </summary>
        /// <returns>商品列表</returns>
        public static async Task<BaseListResult<ProductInfoViewModel>> GetList(object model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/Product/List";
            BaseListResult<ProductInfoViewModel> result = await ApiService.Instance.ApiGetProductList(model, apiUrl);

            return await Task.FromResult(result);
        }

        /// <summary>
        /// 商品新增或修改
        /// </summary>
        /// <returns>執行狀態</returns>
        public static async Task<ResultModel> AddOrModify(ProductDtoModel model, int action)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/Product/Modify";

            if (action == 0)
            {
                apiUrl = "/Product/Add";
            }

            ResultModel result = await ApiService.Instance.ApiUpdataProduct(model, apiUrl);

            return result;
        }

        /// <summary>
        /// 取得分期數
        /// </summary>
        /// <returns>執行狀態</returns>
        public static async Task<BaseResult<PeriodInfoViewModel>> GetPeriod(int memberId)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/EP_VENDER_TRANS/GET_EP_RATE_LIST";
            BaseResult<ProductPeriodViewModel> baseResult = await ApiService.Instance.ApiGetRate(new { MEMBER_ID = memberId }, apiUrl);

            List<PeriodItemViewModel> periodItems = baseResult.Data.Rates1.Select(x => new PeriodItemViewModel
            {
                Period = int.Parse(x.PeriodNum),
                PeriodDescription = "",
                PeriodType = ProductOptionEnum.零利率,
                Rate = double.Parse(x.InterestRate)
            }).ToList();

            periodItems.AddRange(baseResult.Data.Rates2.Select(x => new PeriodItemViewModel
            {
                Period = int.Parse(x.PeriodNum),
                PeriodDescription = "",
                PeriodType = ProductOptionEnum.低利率,
                Rate = double.Parse(x.InterestRate)
            }).ToList());

            BaseResult<PeriodInfoViewModel> result = new BaseResult<PeriodInfoViewModel>
            {
                Data = new PeriodInfoViewModel
                {
                    PeriodItem = periodItems
                },
                Result = new ResultModel
                {
                    Alert = "",
                    ReturnMsg = "",
                    Count = periodItems.Count,
                    ReturnCode = 0
                }
            };

            return result;
        }

        public static async Task<PeriodInfoViewModel> GetStorePeriod(int memberId)
        {
            try
            {
                BaseResult<PeriodInfoViewModel> periodList = await GetPeriod(memberId);

                if (periodList.Result.ReturnCode == 0)
                {
                    return periodList.Data;
                }
                else
                {
                    Logger.Error($"會員帳號[{memberId}]取得商家分期資訊異常，原因為：{periodList.Result.ReturnMsg}！！！");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"會員帳號[{memberId}]取得商家分期資訊失敗，原因為：{ex.Message}！！！");
                return null;
            }
        }

        /// <summary>
        /// 複製或刪除商品
        /// action
        /// </summary>
        /// <returns>商店資料</returns>
        public static async Task<ResultModel> CopyOrDelete(int memberId, string[] productIds, CopyOrDeleteEnum action, string storeid)
        {
            var model = new
            {
                create_account = memberId,
                product_ids = productIds,
                action,
                storeid
            };
            // 透過 Api 把資料送出去
            string apiUrl = "/Product/Copy";

            if (action == CopyOrDeleteEnum.刪除)
            {
                apiUrl = "/Product/Delete";
            }

            ResultModel result = await ApiService.Instance.ApiCopyOrDeleteProduct(model, apiUrl);

            return result;
        }

        /// <summary>
        /// 上下架商品
        /// </summary>
        /// <returns>商店資料</returns>
        public static async Task<ResultModel> ModifyStatus(int memberId, string[] productIds, bool status, string storeid)
        {
            var model = new
            {
                create_account = memberId,
                product_ids = productIds,
                status = status ? (int)ProductStatusEnum.上架 : (int)ProductStatusEnum.下架,
                storeid
            };
            // 透過 Api 把資料送出去
            string apiUrl = "/Product/SetProductStatus";
            ResultModel result = await ApiService.Instance.ApiModifyProductStatus(model, apiUrl);

            return result;
        }

        /// <summary>
        /// 取得上架商品庫存資訊
        /// </summary>
        /// <returns>商品列表</returns>
        public static async Task<BaseListResult<SpecDtoModel>> GetInStock(object model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/Product/InStock";
            BaseListResult<SpecDtoModel> result = await ApiService.Instance.ApiGetInStock(model, apiUrl);

            return await Task.FromResult(result);
        }

        /// <summary>
        /// 取得已發布商品列表
        /// </summary>
        /// <returns>商品列表</returns>
        public static async Task<BaseListResult<ProductInfoViewModel>> GetPublishList(object model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/Product/PublishList";
            BaseListResult<ProductInfoViewModel> result = await ApiService.Instance.ApiGetProductList(model, apiUrl);

            return await Task.FromResult(result);
        }

        /// <summary>
        /// 取得已發佈商品列表
        /// </summary>
        /// <returns></returns>
        public async static Task<List<ProductInfoViewModel>> GetPublishProductyList(string storeId)
        {
            try
            {
                var model = new
                {
                    storeid = storeId
                };

                BaseListResult<ProductInfoViewModel> productList = await GetPublishList(model);

                if (productList.Result.ReturnCode != 0)
                {
                    Logger.Error($"商店編號[{storeId}]取得商品列表異常，原因為：{productList.Result.ReturnMsg}！！！");

                    return null;
                }

                productList.Data.ForEach(x =>
                {
                    x.InventoryStatus = "無庫存";
                    int specsInstock = x.ProductSpecs.Where(y => y.SpecsInstock != "0").Count();

                    if (specsInstock != 0)
                    {
                        x.InventoryStatus = specsInstock == x.ProductSpecs.Count ? "有現貨" : "部分缺貨";
                    }

                    List<ProductPeriodTypeDtoModel> productPeriodTypeDtoModels = x.ProductPeriod.OrderBy(y => int.Parse(y.PeriodNum)).ToList();

                    x.ProductPeriod = productPeriodTypeDtoModels;
                });

                return productList.Data;
            }
            catch (Exception ex)
            {
                Logger.Error($"商店編號[{storeId}]取得商品列表失敗，原因為：{ex.Message}！！！");

                return null;
            }
        }
    }
}
using NLog;
using SupplierPlatform.Enums;
using SupplierPlatform.Extensions;
using SupplierPlatform.Models;
using SupplierPlatform.Models.Dto;
using SupplierPlatform.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupplierPlatform.Services
{
    /// <summary>
    /// 一頁式電商相關服務
    /// </summary>
    public class EcommerceService
    {
        private EcommerceService()
        {
        }

        private static EcommerceService _instance;

        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public static EcommerceService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EcommerceService();
                }
                return _instance;
            }
        }

        /// <summary>
        /// 取得商店資訊
        /// </summary>
        /// <returns></returns>
        public async static Task<StoreInfoViewModel> GetStore(string storeId)
        {
            try
            {
                object model = new
                {
                    storeid = storeId
                };

                BaseResult<StoreInfoViewModel> result = await StoreService.GetInfo(model);

                if (result.Result.ReturnCode == 0 && result.Result.Count == 1)
                {
                    return result.Data;
                }
                else if (result.Result.ReturnCode == 0 && result.Result.Count == 0)
                {
                    // 沒有資料就回傳null
                    return null;
                }
                else
                {
                    Logger.Error($"商店編號[{storeId}]取得商店資訊失敗，原因為：{result.Result.ReturnMsg}！！！");
                    return null;
                }
            }
            catch (Exception e)
            {
                Logger.Error($"商店編號[{storeId}]取得商店資訊失敗，原因為：{e.Message}！！！");
                return null;
            }
        }

        /// <summary>
        /// 取得商品列表
        /// </summary>
        /// <returns></returns>
        public async static Task<List<ProductInfoViewModel>> GetProductyList(string storeId)
        {
            try
            {
                var model = new
                {
                    storeid = storeId
                };

                BaseListResult<ProductInfoViewModel> productList = await ProductService.GetList(model);

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

        /// <summary>
        /// 取得商品資訊
        /// </summary>
        /// <returns></returns>
        public async static Task<ProductInfoViewModel> GetProduct(int productid, string storeId)
        {
            try
            {
                var model = new
                {
                    storeid = storeId,
                    product_id = productid
                };

                BaseResult<ProductInfoViewModel> productList = await ProductService.GetInfo(model);

                if (productList.Result.ReturnCode == 0)
                {
                    productList.Data.ProductSpecs.ForEach(x =>
                    {
                        x.SpecsInstockHidden = x.SpecsInstock;
                    });
                    return productList.Data;
                }
                else
                {
                    Logger.Error($"商店編號[{storeId}]取得商品資訊異常，原因為：{productList.Result.ReturnMsg}！！！");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"商店編號[{storeId}]取得商品資訊失敗，原因為：{ex.Message}！！！");
                return null;
            }
        }

        /// <summary>
        /// 取得商家分期資訊
        /// </summary>
        /// <returns></returns>
        public async static Task<PeriodInfoViewModel> GetStorePeriod(int memberId)
        {
            try
            {
                BaseResult<PeriodInfoViewModel> periodList = await ProductService.GetPeriod(memberId);

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
        /// 取得訂單資訊
        /// </summary>
        /// <param name="index"></param>
        /// <param name="pageSize"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async static Task<BaseResult<List<CaseDtoModel>>> GerPageOrders(OrderInquiryParamsViewModel orderParams, int index = 1, int pageSize = 20, OnePageStatusEnum? status = null)
        {
            BaseResult<LatestCaseViewModel> result = new BaseResult<LatestCaseViewModel>
            {
                Data = new LatestCaseViewModel
                {
                    CASE_LIST = new List<CaseDtoModel>()
                },
                Result = new ResultModel { }
            };

            switch (status)
            {
                case OnePageStatusEnum.Cancel:
                case OnePageStatusEnum.NoPayment:
                case OnePageStatusEnum.UnderReview:
                case OnePageStatusEnum.UnderReview_Second:
                    {
                        OrderInquiryParamsStatus inquiryParamsStatus = new OrderInquiryParamsStatus();

                        foreach (string s in status.Value.ConvertApiStatus())
                        {
                            //直接改記憶體裡面的資串
                            inquiryParamsStatus.STATUS = s;

                            orderParams.STATUS.Add(inquiryParamsStatus);

                            BaseResult<LatestCaseViewModel> baseResult = await CaseService.GetOrderInquiry(orderParams);
                            if (baseResult.Result.ReturnCode == 0)
                            {
                                result.Data.CASE_LIST.AddRange(baseResult.Data.CASE_LIST);
                                result.Result.Count += baseResult.Result.Count;
                            }
                        }
                    }
                    break;

                default:
                    {
                        result = await CaseService.GetOrderInquiry(orderParams);

                        if (result.Result.ReturnCode != 0)
                        {
                            return new BaseResult<List<CaseDtoModel>>
                            {
                                Data = new List<CaseDtoModel>(),
                                Result = result.Result
                            };
                        }
                    }
                    break;
            }

            List<string> orderIds = result.Data.CASE_LIST.Select(x => x.OrderId).ToList();

            var model = new
            {
                storeid = orderParams.VENDER_ID,
                orserids = orderIds
            };

            // 透過訂單編號取得商品規格
            BaseListResult<OrderItemDtoModel> orderItemList = await GetProductByOrder(model);

            result.Data.CASE_LIST.ForEach(x =>
            {
                x.ProductSpecs = string.Empty;
                x.CustomeSpecs = string.Empty;

                if (orderItemList.Data != null && orderItemList.Data.Count != 0)
                {
                    OrderItemDtoModel  orderItemDtoModel = orderItemList.Data.Where(y => y.OrderId == x.OrderId).FirstOrDefault();

                    if (orderItemDtoModel != null)
                    {
                        x.ProductSpecs = orderItemDtoModel.Specs1Name;
                        x.CustomeSpecs = orderItemDtoModel.Specs2Name;
                    }
                }

                x.Address = x.Address ?? string.Empty;
                x.AppNo = x.AppNo ?? string.Empty;
                x.CaseId = x.CaseId ?? string.Empty;
                x.CustName = x.CustName ?? string.Empty;
                x.AppropriationAmount = x.AppropriationAmount ?? string.Empty;
                x.Mobile = x.Mobile ?? string.Empty;
                x.CreateDt = x.CreateDt ?? string.Empty;
                x.MerchantId = x.MerchantId ?? string.Empty;
                x.OrderId = x.OrderId ?? string.Empty;
                x.ProductName = x.ProductName ?? string.Empty;
                x.TransactionAmount = x.TransactionAmount ?? string.Empty;
                x.Period = x.Period ?? string.Empty;
                x.UpdateDt = x.UpdateDt ?? string.Empty;
                x.Status = x.Status ?? string.Empty;
                x.StatusName = x.StatusName ?? string.Empty;
            });

            return new BaseResult<List<CaseDtoModel>>
            {
                Data = result.Data.CASE_LIST,
                Result = result.Result
            };
        }

        /// <summary>
        /// 取得商品訂單列表
        /// </summary>;
        /// <returns>商品列表</returns>
        public static async Task<BaseListResult<OrderItemDtoModel>> GetProductByOrder(object model)
        {
            // 透過 Api 把資料送出去
            string apiUrl = "/Order/OrderList";
            BaseListResult<OrderItemDtoModel> result = await ApiService.Instance.ApiGetOrderItemList(model, apiUrl);

            return await Task.FromResult(result);
        }
    }
}
using Infrastructure.LoggerService;
using Infrastructure.LoggerService.Services;
using Infrastructure.UploadService;
using SupplierPlatform.Entities;
using SupplierPlatform.Models;
using SupplierPlatform.Models.BaseModels;
using SupplierPlatform.Models.ViewModel;
using SupplierPlatform.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SupplierPlatform.Controllers
{
    public class StoreController : AuthController
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
        public StoreController(IOperatorContext _operatorContent)
        {
            this.OperatorContent = _operatorContent ?? throw new ArgumentNullException(nameof(_operatorContent));
        }

        /// <summary>
        /// 取得商店資料
        /// </summary>
        /// <returns>商店資料</returns>
        [HttpPost]
        public async Task<JsonResult> GetInfo()
        {
            try
            {
                var model = new
                {
                    storeid = ((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID
                };

                BaseResult<StoreInfoViewModel> result = await StoreService.GetInfo(model);

                return this.Json(result);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]建立或修改商店失敗，原因為：{e.Message}！！！");

                return this.Json(new ResultModel
                {
                    ReturnCode = -1,
                    Alert = "建立或修改商店失敗"
                });
            }
        }

        /// <summary>
        /// 修改商店
        /// </summary>
        /// <returns>執行狀態</returns>
        [HttpPost]
        public async Task<JsonResult> CreateAndModify(StoreViewModel model)
        {
            try
            {
                // 檢查有沒有新上傳的檔案的 uuid
                if (this.Session["ImgList"] == null)
                {
                    // 取得原本的 logoPath 或 uuid
                    if (string.IsNullOrEmpty(model.LogoPath) || string.IsNullOrEmpty(model.LogoFileuuid))
                    {
                        return this.Json(new ResultModel
                        {
                            ReturnCode = -1,
                            Alert = "上傳失敗"
                        });
                    }
                }
                else
                {
                    FilebaseModel imageFile = ((List<FilebaseModel>)this.Session["ImgList"]).FirstOrDefault();
                    string weburl = ConfigurationManager.AppSettings["Web_Url"];
                    model.LogoPath = $"{weburl}Home/RenderImage?id={imageFile.FileUID}";
                    model.LogoFileuuid = imageFile.FileUID;
                }

                model.Storeid = ((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID;
                model.StoreName = ((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_NME;
                model.CreateAccount = ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID.ToString();
                model.MerchantId = ((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID;

                object modelApiKey = new
                {
                    MEMBER_ID = ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID.ToString(),
                    ((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID
                };

                BaseResult<VenderResultViewModel> resultApiKey = await StoreService.GetToken(modelApiKey);

                if (resultApiKey.Result.ReturnCode == -1)
                {
                    this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]TOKEN 取得失敗，原因為：{resultApiKey.Result.ReturnMsg}！！！");

                    return this.Json(new ResultModel
                    {
                        ReturnCode = -1,
                        Count = 0,
                        Alert = "系統發生異常，請聯繫客服人員，謝謝"
                    });
                }

                model.ApiKey = resultApiKey.Data.TOKEN;

                ResultModel result = await StoreService.Update(model);

                if (result.ReturnCode == 0)
                {
                    result.ReturnMsg = model.LogoPath;
                }

                return this.Json(result);
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]更新商店失敗，原因為：{e.Message} {e.StackTrace}！！！");

                return this.Json(new ResultModel
                {
                    ReturnCode = -1,
                    Alert = "更新商店失敗"
                });
            }
        }

        /// <summary>
        /// 取得商店資訊
        /// </summary>
        /// <returns></returns>
        private async Task<StoreInfoViewModel> GetStore()
        {
            try
            {
                var model = new
                {
                    storeid = ((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID
                };

                BaseResult<StoreInfoViewModel> result = await StoreService.GetInfo(model);

                if (result.Result.ReturnCode == 0 && result.Result.Count == 1)
                {
                    return result.Data;
                }
                else
                {
                    this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]發佈商店時取得商店資訊失敗，原因為沒資料或是：{result.Result.ReturnMsg}！！！");
                    return null;
                }
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]發佈商店時取得商店資訊失敗，原因為：{e.Message}！！！");
                return null;
            }
        }

        /// <summary>
        /// 取得商品列表
        /// </summary>
        /// <returns></returns>
        private async Task<List<ProductInfoViewModel>> GetProductyListAsync()
        {
            try
            {
                var model = new
                {
                    storeid = ((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID
                };

                BaseListResult<ProductInfoViewModel> productList = await ProductService.GetList(model);

                if (productList.Result.ReturnCode == 0)
                {
                    return productList.Data;
                }
                else
                {
                    this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]發佈商店時取得商品列表異常，原因為：{productList.Result.ReturnMsg}！！！");
                    return null;
                }
            }
            catch (Exception ex)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]發佈商店時取得商品列表失敗，原因為：{ex.Message}！！！");
                return null;
            }
        }

        private MemoryStream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// 發佈商家
        /// </summary>
        /// <returns>執行狀態</returns>
        [HttpPost]
        public async Task<JsonResult> Publish()
        {
            try
            {
                string topVenderId = ((VendorOperator)this.OperatorContent.Operator).TOP_VENDER_ID;
                string memberId = ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID.ToString();

                // 取得商店資訊
                var model = new
                {
                    storeid = topVenderId
                };

                BaseResult<StoreInfoViewModel> result = await StoreService.GetInfo(model);

                if (result.Data == null || result.Result.ReturnCode == -1)
                {
                    this.Logger.Error($"會員帳號[{memberId}]商店資訊 取得失敗，原因為：{result.Result.ReturnMsg}！！！");

                    return this.Json(new ResultModel
                    {
                        ReturnCode = -1,
                        Count = 0,
                        Alert = "系統發生異常，請聯繫客服人員，謝謝"
                    });
                }

                // 取得交易金鑰
                object modelApiKey = new
                {
                    MEMBER_ID = memberId,
                    TOP_VENDER_ID = topVenderId
                };

                BaseResult<VenderResultViewModel> resultApiKey = await StoreService.GetToken(modelApiKey);

                if (resultApiKey.Result.ReturnCode == -1)
                {
                    this.Logger.Error($"會員帳號[{memberId}]TOKEN 取得失敗，原因為：{resultApiKey.Result.ReturnMsg}！！！");

                    return this.Json(new ResultModel
                    {
                        ReturnCode = -1,
                        Count = 0,
                        Alert = "系統發生異常，請聯繫客服人員，謝謝"
                    });
                }

                // 轉換成商店更新 MODEL
                StoreViewModel storeViewModel = new StoreViewModel
                {
                    Action = 1,
                    ApiKey = resultApiKey.Data.TOKEN,
                    Storeid = result.Data.Storeid,
                    StoreName = result.Data.StoreName,
                    CreateAccount = memberId,
                    MerchantId = topVenderId,
                    LogoPath = result.Data.LogoPath,
                    LogoFileuuid = result.Data.LogoFileuuid,
                    LineId = result.Data.LineId,
                    FacebookId = result.Data.FacebookId,
                    StoreUrl = result.Data.StoreUrl,
                    IGUrl = result.Data.IGUrl,
                    Address = result.Data.Address,
                    DeliveryNote = result.Data.DeliveryNote,
                    DeployStatus = "Y",
                    Phone = result.Data.Phone,
                    SubTitle = result.Data.SubTitle,
                    Title = result.Data.Title,
                    OtherInstructions = result.Data.OtherInstructions
                };

                ResultModel storeResult = await StoreService.Update(storeViewModel);
                if (storeResult.ReturnCode == -1)
                {
                    this.Logger.Error($"會員帳號[{memberId}]商店發布失敗，原因為：{storeResult.ReturnMsg}！！！");

                    return this.Json(new ResultModel
                    {
                        ReturnCode = -1,
                        Count = 0,
                        Alert = "系統發生異常，請聯繫客服人員，謝謝"
                    });
                }

                ChaileaseUpload chaileaseUpload = new ChaileaseUpload();
                string url = chaileaseUpload.GetStoreUrlByTopVenderId(topVenderId, "index", "");
                //發布
                await StoreService.Publish(model);

                return this.Json(new BaseResult<string>
                {
                    Data = url,
                    Result = new ResultModel
                    {
                        ReturnCode = 0,
                        Alert = "發佈商店成功"
                    }
                }
               );
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]發佈商店失敗，原因為：{e.Message}！！！");

                return this.Json(new ResultModel
                {
                    ReturnCode = -1,
                    Alert = "發佈商店失敗"
                });
            }
        }
    }
}
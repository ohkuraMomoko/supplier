using Infrastructure.LoggerService;
using Infrastructure.LoggerService.Services;
using SupplierPlatform.Entities;
using SupplierPlatform.Models;
using SupplierPlatform.Models.ViewModel;
using SupplierPlatform.Services;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SupplierPlatform.Controllers
{
    public class AccountController : AuthController
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
        public AccountController(IOperatorContext _operatorContent) : base()
        {
            this.Logger = new NLogger("AccountController");
            this.OperatorContent = _operatorContent ?? throw new ArgumentNullException(nameof(_operatorContent));
        }

        // GET: Account
        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult ChangePW()
        {
            return this.View();
        }

        /// <summary>
        /// 修改密碼
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ChangePW(ChangePassWordViewModel model)
        {
            // 檢查必填項目是否有值
            this.ValidateModel(model);

            model.MemberId = ((VendorOperator)this.OperatorContent.Operator).MEMBER_ID.ToString();

            try
            {
                // 檢核帳號密碼
                BaseResult<ResultViewModel> result = await AccountService.ChangePW(model);

                if (result.Result.ReturnCode != 0)
                {
                    this.Logger.Error($"會員帳號[{model.MemberId} ]密碼已修改失敗，原因為：{result.Result.ReturnMsg}！！！");
                }

                return this.Json(new ResultModel
                {
                    ReturnCode = result.Data.RTN_CD,
                    Alert = result.Data.ALERT_MSG
                });
            }
            catch (Exception e)
            {
                this.Logger.Error($"會員帳號[{((VendorOperator)this.OperatorContent.Operator).MEMBER_ID}]密碼已修改失敗，原因為：{e.Message}！！！");

                return this.Json(new ResultModel
                {
                    ReturnCode = -1,
                    Alert = "密碼已修改失敗"
                });
            }
        }
    }
}
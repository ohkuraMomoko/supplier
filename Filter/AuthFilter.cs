using SupplierPlatform.Content;
using SupplierPlatform.Controllers;
using SupplierPlatform.Entities;
using SupplierPlatform.Services;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SupplierPlatform.Filter
{
    internal sealed class AuthFilter : IAuthorizationFilter
    {
        private readonly IOperatorContext OperatorContext;

        public AuthFilter(IOperatorContext operatorContext)
        {
            this.OperatorContext = operatorContext ?? throw new ArgumentNullException(nameof(operatorContext));
        }

        private bool CheckAjax(string contentType)
        {
            return (contentType == "application/json;charset=UTF-8") ? true : false;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            ControllerBase controller = filterContext.Controller;

            if (controller is AuthController)
            {
                Operator @operator = this.OperatorContext.Operator;

                if (@operator == null && this.CheckAjax(filterContext.HttpContext.Request.ContentType))
                {
                    filterContext.HttpContext.Response.StatusCode = 401;
                    filterContext.Result = new JsonResult
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        ContentEncoding = System.Text.Encoding.UTF8,
                        ContentType = "application/json",
                        Data = new { error_code = 401, message = "Unauthorized" }
                    };

                    //忽略之後ASP.NET Pipeline的處理步驟，直接跳關到EndRequest
                    filterContext.HttpContext.ApplicationInstance.CompleteRequest();
                }

                if (@operator == null && !this.CheckAjax(filterContext.HttpContext.Request.ContentType))
                {
                    filterContext.Result = new RedirectResult("~/Home/Login?url=" + filterContext.HttpContext.Request.RawUrl);
                    return;
                }

                if (@operator != null)
                {
                    this.PageAuthority(@operator, filterContext);
                }
            }
        }

        private void PageAuthority(Operator @operator, AuthorizationContext filterContext)
        {
            VendorOperator op = @operator as VendorOperator;

            if (string.Equals(filterContext.ActionDescriptor.ActionName, "NotOpen", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (string.Equals(filterContext.ActionDescriptor.ActionName, "AskPaymentNotOpen", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // 處理線上填單 處理訂單/案件查詢
            if (filterContext.Controller is CaseController)
            {
                // 處理線上填單
                if (string.Equals(filterContext.ActionDescriptor.ActionName, "FillIn", StringComparison.OrdinalIgnoreCase))
                {
                    if (!op.MENU_LIST.Any(o => string.Equals(o.FuncId, "ORDER", StringComparison.OrdinalIgnoreCase)))
                    {
                        filterContext.Result = new RedirectResult("~/Case/NotOpen");
                        return;
                    }
                }

                // 處理訂單/案件查詢
                if (string.Equals(filterContext.ActionDescriptor.ActionName, "CaseInquiry", StringComparison.OrdinalIgnoreCase))
                {
                    if (!op.MENU_LIST.Any(o => string.Equals(o.FuncId, "INQUIRY", StringComparison.OrdinalIgnoreCase)))
                    {
                        filterContext.Result = new RedirectResult("~/Case/NotOpen");
                        return;
                    }
                }
            }

            // 處理線上請款, 查詢撥款
            if (filterContext.Controller is BillingController)
            {
                //線上請款
                if (string.Equals(filterContext.ActionDescriptor.ActionName, "AskPayment", StringComparison.OrdinalIgnoreCase))
                {
                    if (!op.MENU_LIST.Any(o => string.Equals(o.FuncId, "CAPTURE", StringComparison.OrdinalIgnoreCase)))
                    {
                        filterContext.Result = new RedirectResult("~/Billing/AskPaymentNotOpen");
                        return;
                    }
                }

                // 查詢撥款
                if (string.Equals(filterContext.ActionDescriptor.ActionName, "AppropriationInquiry", StringComparison.OrdinalIgnoreCase))
                {
                    if (!op.MENU_LIST.Any(o => string.Equals(o.FuncId, "STATEMENT", StringComparison.OrdinalIgnoreCase)))
                    {
                        filterContext.Result = new RedirectResult("~/Billing/NotOpen");
                        return;
                    }
                }
            }

            // 處理一頁式電商
            if (filterContext.Controller is EcommerceController)
            {
                // 商品管理
                if (string.Equals(filterContext.ActionDescriptor.ActionName, "ProductManage", StringComparison.OrdinalIgnoreCase))
                {
                    if (!op.MENU_LIST.Any(o => string.Equals(o.FuncId, "EC", StringComparison.OrdinalIgnoreCase)))
                    {
                        filterContext.Result = new RedirectResult("~/Ecommerce/ProductManageNotOpen");
                        return;
                    }
                }

                // 訂單管理
                if (string.Equals(filterContext.ActionDescriptor.ActionName, "OrderManage", StringComparison.OrdinalIgnoreCase))
                {
                    if (!op.MENU_LIST.Any(o => string.Equals(o.FuncId, "EC", StringComparison.OrdinalIgnoreCase)))
                    {
                        filterContext.Result = new RedirectResult("~/Ecommerce/NotOpen");
                        return;
                    }
                }
            }
        }
    }
}
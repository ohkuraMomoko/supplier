using System.ComponentModel;

namespace SupplierPlatform.Enums
{
    /// <summary>
    /// App Login 對應
    /// </summary>
    public enum AppLoginTypeEnum
    {
        /// <summary>
        /// 查詢撥款
        /// </summary>
        [Description("/Billing/AppropriationInquiry")]
        GrantQuery,

        /// <summary>
        /// 線上請款
        /// </summary>
        [Description("/Billing/AskPayment")]
        OnlineCharge,

        /// <summary>
        /// 線上填單
        /// </summary>
        [Description("/Case/FillIn")]
        OrderDetail
    }
}
using System.ComponentModel;

namespace SupplierPlatform.Enums
{
    /// <summary>
    /// 標題名稱對應 Enum
    /// </summary>
    public enum TitleTypeEnum
    {
        /// <summary>
        /// 一般案件
        /// </summary>
        [Description("一般案件")]
        General = 1,

        /// <summary>
        /// 廣告類
        /// </summary>
        [Description("廣告類")]
        AD = 2,

        /// <summary>
        /// 中租零卡支付案件
        /// </summary>
        [Description("中租零卡支付案件")]
        ZeroCard = 4
    }
}
using System.ComponentModel;

namespace SupplierPlatform.Enums
{
    public enum ProductOptionEnum
    {
        /// <summary>
        /// 零利率
        /// </summary>
        [Description("vendor")]
        零利率 = 0,

        /// <summary>
        /// 低利率
        /// </summary>
        [Description("consumer")]
        低利率 = 1,

        /// <summary>
        /// 顏色規格
        /// </summary>
        顏色規格 = 3,

        /// <summary>
        /// 自訂選項
        /// </summary>
        自訂選項 = 4
    }
}
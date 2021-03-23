using System.ComponentModel;

namespace SupplierPlatform.Enums
{
    /// <summary>
    /// 檢核規則 Enum
    /// </summary>
    public enum RegexEnum
    {
        /// <summary>
        /// 僅限英文
        /// </summary>
        [Description("^[A-Za-z]+$")]
        English,

        /// <summary>
        /// 僅限數字
        /// </summary>
        [Description("^[0-9]+$")]
        Numeric,

        /// <summary>
        /// 僅限英文+數字
        /// </summary>
        [Description("^[A-Za-z0-9]+$")]
        EnglishNumeric,

        /// <summary>
        /// 手機、室內電話號碼
        /// </summary>
        [Description("((?=(0))[0-9]{10})$")]
        Phone
    }
}
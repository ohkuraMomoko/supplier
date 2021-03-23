using SupplierPlatform.Enums;
using SupplierPlatform.Models.ViewModel;
using System;
using System.Collections.Generic;

namespace SupplierPlatform.Entities
{
    [Serializable]
    public class VendorOperator : Operator
    {
        /// <summary>
        /// 廠商會員ID
        /// </summary>
        public int MEMBER_ID { get; set; }

        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string VEND_SALE_NME { get; set; }

        /// <summary>
        /// 頂層供應商ID
        /// </summary>
        public string TOP_VENDER_ID { get; set; }

        /// <summary>
        /// 底層供應商ID
        /// </summary>
        public string VENDER_ID { get; set; }

        /// <summary>
        /// 頂層供應商名稱
        /// </summary>
        public string TOP_VENDER_NME { get; set; }

        /// <summary>
        /// 底層供應商名稱
        /// </summary>
        public string VENDER_NME { get; set; }

        /// <summary>
        /// 門市代碼
        /// </summary>
        public string VNO2 { get; set; }

        /// <summary>
        /// 是否有同意會員條款 1.是, 2否
        /// </summary>
        public string APPROVE_POLICY { get; set; }

        /// <summary>
        /// 是否為中租零卡支付供應商 1.是, 2否
        /// </summary>
        public string IS_EP_VENDER { get; set; }

        /// <summary>
        /// 是否有零利率報價1.是, 2否
        /// </summary>
        public string EP_RATE_TYPE_1 { get; set; }

        /// <summary>
        /// 是否有低利率報價1.是, 2否
        /// </summary>
        public string EP_RATE_TYPE_2 { get; set; }

        /// <summary>
        /// 預設產品名稱
        /// </summary>
        public string DEFAULT_PROD_NME { get; set; }

        /// <summary>
        /// 門市狀態
        /// 往來狀態:
        /// REF:CRS_REF.VENDER_STATUS_CD
        /// A.正常  P.靜止戶
        /// S.停用  L.缺件
        /// </summary>
        public string VENDER_STATUS_CD { get; set; }

        /// <summary>
        /// 是否有主管權限 1:是 2:否
        /// </summary>
        public string IS_ROLE01_TYPE { get; set; }

        /// <summary>
        /// 是否有請款權限 1:是 2:否
        /// </summary>
        public string IS_ROLE02_TYPE { get; set; }

        /// <summary>
        /// 功能清單
        /// </summary>
        public List<MenuViewModel> MENU_LIST { get; set; }

        /// <summary>
        /// 是否為行動裝置
        /// </summary>
        public bool IsMobile { get; set; }

        /// <summary>
        /// 是否為分公司帳號
        /// </summary>
        public bool IsBranch
        {
            get => this.TOP_VENDER_ID != this.VENDER_ID;
        }

        /// <summary>
        /// 裝置類別
        /// </summary>
        public int MobileType { get; set; }

        /// <summary>
        /// 裝置類別對應 ENUM
        /// </summary>
        public MobileTypeEnum MobileTypeNnum
        {
            get => (MobileTypeEnum)this.MobileType;
            set => this.MobileType = (int)value;
        }

        /// <summary>
        /// 一頁式電商的單筆最低交易金額(元)
        /// </summary>
        public int EpLstOnceAmt { get; internal set; }
    }
}
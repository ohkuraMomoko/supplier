using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SupplierPlatform.Enums
{
    public enum OnePageStatusEnum
    {
        [Description("全部")]
        All = 0,
        [Description("顧客未操作付款")]
        NoPayment = 1,
        [Description("審核中")]
        UnderReview = 2,
        [Description("審核中(核准/婉拒)")]
        UnderReview_Second = 3,
        [Description("已取消")]
        Cancel = 4,
    }
}
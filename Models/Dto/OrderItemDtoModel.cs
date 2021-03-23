namespace SupplierPlatform.Models.Dto
{
    /// <summary>
    /// 訂單明細 Dto Model
    /// </summary>
    public class OrderItemDtoModel
    {
        /// <summary>
        /// 訂單編號
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 商品編號
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品價格
        /// </summary>
        public int ProductPrice { get; set; }

        /// <summary>
        /// 商品規格名稱1
        /// </summary>
        public string Specs1Name { get; set; }

        /// <summary>
        /// 商品規格名稱2
        /// </summary>
        public string Specs2Name { get; set; }

        /// <summary>
        /// 訂單金額
        /// </summary>
        public int OrderAmount { get; set; }
    }
}
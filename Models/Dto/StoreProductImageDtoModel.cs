using Newtonsoft.Json;

namespace SupplierPlatform.Models.Dto
{
    /// <summary>
    /// 圖檔詳細資料
    /// </summary>
    public class StoreProductImageDtoModel
    {
        /// <summary>
        /// 圖片 UUId
        /// </summary>
        [JsonProperty("imageuuid")]
        public string Uuid { get; set; }        

        /// <summary>
        /// 商店ID
        /// </summary>
        [JsonProperty("storeid")]
        public string Store_Id { get; set; }

        /// <summary>
        /// 商品編號
        /// </summary>
        [JsonProperty("productid")]
        public int Product_Id { get; set; }

        /// <summary>
        /// 圖檔類別 1：商店LOGO，2：商品主圖，3：商品小圖
        /// </summary>
        [JsonProperty("imagetype")]
        public int ImageType { get; set; }

        /// <summary>
        /// 建立帳號
        /// </summary>
        [JsonProperty("createaccount")]
        public string Create_Account { get; set; }

        /// <summary>
        /// 修改帳號
        /// </summary>
        [JsonProperty("updateaccount")]
        public string Update_Account { get; set; }

        /// <summary>
        /// 圖片路徑
        /// </summary>
        [JsonProperty("imagepath")]
        public string ImagePath { get; set; }
    }

    /// <summary>
    /// 圖檔詳細資料
    /// </summary>
    public class StoreProductImageForPublish
    {
        /// <summary>
        /// 圖片 UUId
        /// </summary>
        public string Uuid { get; set; }

        /// <summary>
        /// 商店ID
        /// </summary>
        public string Store_Id { get; set; }

        /// <summary>
        /// 商品編號
        /// </summary>
        public int Product_Id { get; set; }

        /// <summary>
        /// 圖檔類別 1：商店LOGO，2：商品主圖，3：商品小圖
        /// </summary>
        public int ImageType { get; set; }

        /// <summary>
        /// 建立帳號
        /// </summary>
        public string Create_Account { get; set; }

        /// <summary>
        /// 修改帳號
        /// </summary>
        public string Update_Account { get; set; }
    }
}
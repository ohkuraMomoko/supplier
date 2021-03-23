namespace SupplierPlatform.Models
{
    public class Root<T>
    {
        public T Data { get; set; }
        public ResultModel Result { get; set; }
    }
}
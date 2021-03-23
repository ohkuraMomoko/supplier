using System.Collections.Generic;

namespace SupplierPlatform.Models.BaseModels
{
    public class BasePageResult<T> : BaseResult<List<T>>
    {
        public PaginationModel Pagination { get; set; }
    }
}
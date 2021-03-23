using SupplierPlatform.Entities;

namespace SupplierPlatform.Services
{
    public interface IOperatorContext
    {
        Operator Operator { get; set; }
    }
}
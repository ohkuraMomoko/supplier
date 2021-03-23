using SupplierPlatform.Entities;
using SupplierPlatform.Services;

namespace SupplierPlatform.Controllers
{
    public class AuthController : BaseController
    {
        protected Operator Operator
        {
            get
            {
                return new OperatorSessionContext().Operator;
            }
        }
    }
}
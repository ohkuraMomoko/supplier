using SupplierPlatform.Entities;
using System.Web;

namespace SupplierPlatform.Services
{
    public sealed class OperatorSessionContext : IOperatorContext
    {
        private const string SESSION_NAME = "_supplier_platform";

        public Operator Operator
        {
            get
            {
                return (Operator)HttpContext.Current.Session[SESSION_NAME];
            }
            set
            {
                HttpContext.Current.Session[SESSION_NAME] = value;
            }
        }
    }
}
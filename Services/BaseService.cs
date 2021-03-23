using SupplierPlatform.Helps;
using System;
using System.Threading.Tasks;

namespace SupplierPlatform.Services
{
    public class BaseService
    {
        private WebApiAdapter _webapi = null;

        public BaseService()
            : this(new WebApiAdapter())
        {
        }

        public BaseService(WebApiAdapter adapter)
        {
            this.WebApi = adapter;
        }

        public WebApiAdapter WebApi
        {
            get
            {
                if (null == this._webapi)
                {
                    this._webapi = new WebApiAdapter();
                }
                return this._webapi;
            }
            set
            {
                this._webapi = value;
            }
        }

        protected async Task<T> GetApiResultAsync<T>(string requestMethod, object jsonData,bool isDown = false, bool onepage = false)
        {
            T rtnModel = await this.WebApi.QueryAsync<T>(requestMethod, jsonData, isDown, onepage);
            return rtnModel;
        }
    }
}
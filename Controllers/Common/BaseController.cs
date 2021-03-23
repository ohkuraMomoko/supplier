using RestSharp;
using RestSharp.Authenticators;
using SupplierPlatform.Models;
using SupplierPlatform.Models.BaseModels;
using SupplierPlatform.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SupplierPlatform.Controllers
{
    public class BaseController : Controller, IActionFilter
    {

        public JsonResult ResultJson<T>(BasePageResult<T> data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取得圖檔
        /// </summary>
        /// <param name="id">file uuid</param>
        /// <returns></returns>
        //public ActionResult Image(string id)
        //{
        //    //if (!string.IsNullOrEmpty(id))
        //    //{
        //    //    byte[] imageBytes = new FileRepository().Download(id)?.File;
        //    //    if (imageBytes != null)
        //    //    {
        //    //        return this.Json(new { image = System.Convert.ToBase64String(imageBytes), result = true }, JsonRequestBehavior.AllowGet);
        //    //    }
        //    //}

        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        return new FileRepository().DownloadFile(id);
        //    }

        //    return null;
        //}

        public FileResult RenderImage(string id)
        {

            string ContentType = string.Empty;
            IRestResponse response = null;
            string filename = "";
            if (id != null)
            {
                string Download_Api_Url = System.Web.Configuration.WebConfigurationManager.AppSettings["FileCloudUrl"].ToString()+ "DownloadFile";
                string Download_Usr_Id = System.Web.Configuration.WebConfigurationManager.AppSettings["Account"].ToString();
                string Download_Usr_Pwd = System.Web.Configuration.WebConfigurationManager.AppSettings["Passwprd"].ToString();
                var client = new RestClient(Download_Api_Url);
                var request = new RestRequest(Method.POST);
                client.Authenticator = new HttpBasicAuthenticator(Download_Usr_Id, Download_Usr_Pwd);
                request.AddParameter("FileUID", id);
                response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    System.Web.HttpContext Context = this.HttpContext.ApplicationInstance.Context;
                    string ContentDisposition = response.Headers.FirstOrDefault(t => t.Name == "Content-Disposition").Value.ToString();
                    ContentType = response.Headers.FirstOrDefault(t => t.Name == "Content-Type").Value.ToString();
                    filename = Uri.UnescapeDataString(ContentDisposition.Split(';')[1].Replace(" filename=", ""));
                    return File(response.RawBytes, ContentType);
                }
            }
            return null;
        }



    }
}
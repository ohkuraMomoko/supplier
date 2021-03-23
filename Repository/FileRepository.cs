using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using SupplierPlatform.Models;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SupplierPlatform.Repository
{
    public class FileRepository
    {
        private RestClient Client { get; set; }

        public FileRepository()
        {
            string baseURL = ConfigurationManager.AppSettings["FileCloudUrl"]?.ToString();
            this.Client = new RestClient(baseURL)
            {
                Authenticator = new HttpBasicAuthenticator(ConfigurationManager.AppSettings["Account"]?.ToString(),
                 ConfigurationManager.AppSettings["Passwprd"]?.ToString())
            };
        }

        public string Upload(string Filename, Stream FileStream)
        {
            MemoryStream memoryStream = new MemoryStream();
            FileStream.CopyTo(memoryStream);
            return this.Upload(Filename, memoryStream.ToArray());
        }

        public string Upload(string Filename, byte[] File)
        {
            RestRequest request = new RestRequest("UoloadFile", Method.POST);
            request.AddFile("File", File, Filename);
            IRestResponse response = this.Client.Execute(request);
            if (response.ResponseStatus == ResponseStatus.Error)
            {
                throw new Exception("file upload api exception - " + response.ErrorException.Message, response.ErrorException);
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(string.Format("file upload api error - url:{0}, statusCode:{1}, responseContent:{2}", response.ResponseUri.ToString(), response.StatusDescription, response.Content));
            }

            Root<UploadFile> result = JsonConvert.DeserializeObject<Root<UploadFile>>(response.Content);
            if (result.Result.ReturnCode != 0)
            {
                throw new Exception(string.Format("file upload api error - url:{0}, statusCode:{1}, errorMsg:{2}, responseContent: {3}", response.ResponseUri.ToString(), response.StatusDescription, response.ErrorMessage, response.Content));
            }

            return result.Data.FileUUID;
        }

        public DownloadFileRDto Download(string FileUID)
        {
            RestRequest request = new RestRequest("DownloadFile", Method.POST);
            request.AddParameter("FileUID", FileUID);
            IRestResponse response = this.Client.Execute(request);

            if (response.ResponseStatus == ResponseStatus.Error)
            {
                throw new Exception("file download api exception - " + response.ErrorException.Message, response.ErrorException);
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(string.Format("file download api error - url:{0}, statusCode:{1}, responseContent:{2}", response.ResponseUri.ToString(), response.StatusDescription, response.Content));
            }

            string ContentDisposition = response.Headers.FirstOrDefault(t => t.Name == "Content-Disposition").Value.ToString();
            string filename = Uri.UnescapeDataString(ContentDisposition.Split(';')[1].Replace(" filename=", ""));

            return new DownloadFileRDto()
            {
                FileName = filename,
                File = response.RawBytes,
            };
        }

        //public FileResult DownloadFile(string FileUID)
        //{
        //    RestRequest request = new RestRequest("DownloadFile", Method.POST);
        //    request.AddParameter("FileUID", FileUID);
        //    IRestResponse response = this.Client.Execute(request);

        //    if (response.ResponseStatus == ResponseStatus.Error)
        //    {
        //        throw new Exception("file download api exception - " + response.ErrorException.Message, response.ErrorException);
        //    }

        //    if (response.StatusCode != HttpStatusCode.OK)
        //    {
        //        throw new Exception(string.Format("file download api error - url:{0}, statusCode:{1}, responseContent:{2}", response.ResponseUri.ToString(), response.StatusDescription, response.Content));
        //    }

        //    string ContentDisposition = response.Headers.FirstOrDefault(t => t.Name == "Content-Disposition").Value.ToString();
        //    string filename = Uri.UnescapeDataString(ContentDisposition.Split(';')[1].Replace(" filename=", ""));

        //    //System.Web.HttpContext Context = this.HttpContext.ApplicationInstance.Context;
        //     ContentDisposition = response.Headers.FirstOrDefault(t => t.Name == "Content-Disposition").Value.ToString();
        //    string ContentType = response.Headers.FirstOrDefault(t => t.Name == "Content-Type").Value.ToString();
        //    filename = Uri.UnescapeDataString(ContentDisposition.Split(';')[1].Replace(" filename=", ""));

        //    return File(response.RawBytes, ContentType);
        //}
       
    }
}
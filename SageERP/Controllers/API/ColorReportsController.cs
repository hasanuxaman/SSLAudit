using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Exceptional;

namespace ShampanERP.Controllers.API
{
    [Authorize]
    public class ColorReportsController : Controller
    {
        //ColorReports
       
        public byte[] DownloadFile(string serverUrl)
        {
            var client = new System.Net.WebClient();
            client.Headers.Add("Content-Type", "application/pdf");

            Byte[] buffer = client.DownloadData(serverUrl);

            client.Headers.Add("content-length", buffer.Length.ToString());
            return buffer;


        }
    }
}

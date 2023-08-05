using com.sun.org.apache.xml.@internal.resolver.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mpp_reader_lambda
{
    internal class GetFile
    {
        public static async Task<string> DownloadFile(string url)
        {
            string pathToSave = System.IO.Path.GetTempPath();
            pathToSave = Path.Combine(pathToSave, "temp.mpp");

            using (var httpClient = new HttpClient())
            {
                //var json = await httpClient.GetStringAsync(url);

                //// Now parse with JSON.Net
                //JObject o = JObject.Parse(json);
                //return o.ToString();

                var httpResult = await httpClient.GetAsync(url);
                using var resultStream = await httpResult.Content.ReadAsStreamAsync();
                using var fileStream = File.Create(pathToSave);
                resultStream.CopyTo(fileStream);

                Console.WriteLine(pathToSave);

                return pathToSave;
            }
        }
    }
}

using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ImageGalleryDemo
{
    class DataFetcher
    {
    async Task<string> GetDatafromService(string searchstring)
        {
            string readText = null;
            try
            {
                String url = @" https://imagefetcherapi.azurewebsites.net/api/fetch_images?query=" + searchstring + "&max_count=5";
                using (HttpClient c = new HttpClient())
                {
                    readText = await c.GetStringAsync(url);
                }
            }
            catch
            {
                readText = File.ReadAllText(@"Data/sampleData.json");
            }
            return readText;
         }
        public async Task<List<ImageItem>> GetImageData(string search)
        {
            string data = await GetDatafromService(search);
            return JsonConvert.DeserializeObject<List<ImageItem>>(data);
        }
    }
    
}

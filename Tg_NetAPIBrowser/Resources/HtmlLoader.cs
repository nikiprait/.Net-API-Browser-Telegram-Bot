using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tg_NetAPIBrowser.Resources
{
    class HtmlLoader
    {
        readonly HttpClient client;
        readonly string Url;

        public HtmlLoader(IParserSettings settings)
        {
            client = new HttpClient();
            Url = $"{settings.BaseUrl}/{settings.Prefix}/";
        }
         
        public async Task<string> GetSourceByPageName(string name)
        {
            var currentUrl = Url.Replace("{CurrentName}", name);
            var response = await client.GetAsync(currentUrl);
            string source = null;

            if (response!=null && response.StatusCode == HttpStatusCode.OK)
            {
                source = await response.Content.ReadAsStringAsync();
            }

            return source;
        }
    }
}

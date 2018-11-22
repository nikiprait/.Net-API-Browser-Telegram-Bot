
namespace Tg_NetAPIBrowser.Resources.MSDN
{
    class MSDNSettings : IParserSettings
    {
        public string BaseUrl { get; set; } = "";

        public string Prefix { get; set; } = "{CurrentName}";
    }
}

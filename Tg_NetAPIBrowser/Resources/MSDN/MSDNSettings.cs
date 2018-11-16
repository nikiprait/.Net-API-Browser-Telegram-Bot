
using System;

namespace Tg_NetAPIBrowser.Resources.MSDN
{
    class MSDNSettings : IParserSettings
    {
        public string BaseUrl { get; set; } = "https://docs.microsoft.com/ru-ru/dotnet/api";

        public string Prefix { get; set; } = "{CurrentName}";
    }
}

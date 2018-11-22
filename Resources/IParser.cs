using AngleSharp.Dom.Html;

namespace Tg_NetAPIBrowser.Resources
{
    interface IParser<T> where T : class
    {
        T Parse(IHtmlDocument document);
    }
}

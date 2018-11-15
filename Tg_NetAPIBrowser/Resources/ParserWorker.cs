using AngleSharp.Parser.Html;
using System;

namespace Tg_NetAPIBrowser.Resources
{
    class ParserWorker<T> where T : class
    {
        IParser<T> parser;
        IParserSettings parserSettings;

        HtmlLoader loader;

        #region Properties

        public IParser<T> Parser
        {
            get
            {
                return parser;
            }
            set
            {
                parser = value;
            }
        }

        public IParserSettings Settings
        {
            get
            {
                return parserSettings;
            }
            set
            {
                parserSettings = value;
                loader = new HtmlLoader(value);
            }
        }

        #endregion

        public event Action<object, T> OnNewData;
        public bool turner = false;
        public ParserWorker(IParser<T> parser)
        {
            this.parser = parser;
        }

        public ParserWorker(IParser<T> parser, IParserSettings parserSettings) : this(parser)
        {
            this.parserSettings = parserSettings;
        }
        public async void Worker(string name)
        {
            var source = await loader.GetSourceByPageName(name);
            var domParser = new HtmlParser();
            var document = await domParser.ParseAsync(source);
            var result = parser.Parse(document);
            
            OnNewData?.Invoke(this, result);
            
        }
    }
}

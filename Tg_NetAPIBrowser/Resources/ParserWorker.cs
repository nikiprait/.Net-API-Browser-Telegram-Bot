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

<<<<<<< HEAD
        public event Action<object, T, string> OnNewData;
        public bool turner = false;

=======
        public event Action<object, T> OnNewData;
        public bool turner = false;
>>>>>>> 7dbea807ea2851a27bc9330cb0c1fcb8765d209b
        public ParserWorker(IParser<T> parser)
        {
            this.parser = parser;
        }

        public ParserWorker(IParser<T> parser, IParserSettings parserSettings) : this(parser)
        {
            this.parserSettings = parserSettings;
        }
<<<<<<< HEAD
        public async void Worker(string name, string ChatId)
=======
        public async void Worker(string name)
>>>>>>> 7dbea807ea2851a27bc9330cb0c1fcb8765d209b
        {
            var source = await loader.GetSourceByPageName(name);
            var domParser = new HtmlParser();
            var document = await domParser.ParseAsync(source);
            var result = parser.Parse(document);
            
<<<<<<< HEAD
            OnNewData?.Invoke(this, result, ChatId);
=======
            OnNewData?.Invoke(this, result);
>>>>>>> 7dbea807ea2851a27bc9330cb0c1fcb8765d209b
            
        }
    }
}

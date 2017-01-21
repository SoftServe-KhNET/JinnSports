using System.Configuration;

namespace JinnSports.Parser.App.Configuration.Parser
{
    public class ParserInstanceCollection : ConfigurationElementCollection
    {
        public ParserInstanceElement this[int index]
        {
            get { return this.BaseGet(index) as ParserInstanceElement; }
        }

        public new ParserInstanceElement this[string name]
        {
            get { return this.BaseGet(name) as ParserInstanceElement; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ParserInstanceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ParserInstanceElement)element).Profile;
        }
    }
}
using System.Configuration;

namespace JinnSports.Parser.App.Configuration.Proxy
{
    public class ProxyInstanceCollection : ConfigurationElementCollection
    {
        public ProxyInstanceElement this[int index]
        {
            get { return this.BaseGet(index) as ProxyInstanceElement; }
        }

        public new ProxyInstanceElement this[string name]
        {
            get { return this.BaseGet(name) as ProxyInstanceElement; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ProxyInstanceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ProxyInstanceElement)element).Profile;
        }
    }
}
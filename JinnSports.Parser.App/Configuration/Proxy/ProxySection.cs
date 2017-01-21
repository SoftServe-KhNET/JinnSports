using System.Configuration;

namespace JinnSports.Parser.App.Configuration.Proxy
{
    public class ProxySection : ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public ProxyInstanceCollection Instances
        {
            get { return (ProxyInstanceCollection)this[string.Empty]; }
            set { this[string.Empty] = value; }
        }
    }
}
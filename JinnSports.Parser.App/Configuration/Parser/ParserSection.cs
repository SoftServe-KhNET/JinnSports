using System.Configuration;

namespace JinnSports.Parser.App.Configuration.Parser
{
    public class ParserSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public ParserInstanceCollection Instances
        {
            get { return (ParserInstanceCollection)this[string.Empty]; }
            set { this[string.Empty] = value; }
        }
    }
}
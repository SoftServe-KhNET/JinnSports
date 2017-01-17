using System.Configuration;

namespace JinnSports.Parser.App
{
    public class MyConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public MyConfigInstanceCollection Instances
        {
            get { return (MyConfigInstanceCollection)this[string.Empty]; }
            set { this[string.Empty] = value; }
        }
    }
}
using System.Configuration;

namespace JinnSports.BLL.Utilities
{
    public sealed class NewsConfiguration : ConfigurationSection
    {
        public static NewsConfiguration Configuration { get; } = ConfigurationManager.GetSection("NewsConfiguration") as NewsConfiguration;

        [ConfigurationProperty("maxNumberOfNews", DefaultValue = 10, IsRequired = false, IsKey = false)]
        [IntegerValidator(MinValue = 1, MaxValue = 100)]
        public int MaxNumberOfNews
        {
            get { return (int)this["maxNumberOfNews"]; }
            set { this["maxNumberOfNews"] = value; }
        }

        [ConfigurationProperty("", IsRequired = true, IsKey = false, IsDefaultCollection = true)]
        public NewsSources Sources
        {
            get
            {
                NewsSources sources = (NewsSources)base[string.Empty];
                return sources;
            }
        }

        public class NewsSources : ConfigurationElementCollection
        {
            public override ConfigurationElementCollectionType CollectionType => 
                ConfigurationElementCollectionType.BasicMap;

            protected override string ElementName => "source";

            public NewsSourcesElement this[int index]
            {
                get
                {
                    return (NewsSourcesElement)BaseGet(index);
                }
                set
                {
                    if (this.BaseGet(index) != null)
                    {
                        this.BaseRemoveAt(index);
                    }
                    this.BaseAdd(index, value);
                }
            }

            protected override ConfigurationElement CreateNewElement()
            {
                return new NewsSourcesElement();
            }

            protected override object GetElementKey(ConfigurationElement element)
            {
                return ((NewsSourcesElement)element).Link;
            }

            protected override bool IsElementName(string elementName)
            {
                if (string.IsNullOrWhiteSpace(elementName) || elementName != "source")
                {
                    return false;
                }

                return true;
            }
        }

        public class NewsSourcesElement : ConfigurationElement
        {
            [ConfigurationProperty("link", IsKey = true, IsRequired = true)]
            public string Link
            {
                get { return (string)this["link"]; }
                set { this["link"] = value; }
            }
        }
    }
}
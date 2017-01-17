using System.Configuration;

namespace JinnSports.Parser.App
{
    public class MyConfigInstanceCollection : ConfigurationElementCollection
    {
        public MyConfigInstanceElement this[int index]
        {
            get { return this.BaseGet(index) as MyConfigInstanceElement; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new MyConfigInstanceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            //set to whatever Element Property you want to use for a key
            return ((MyConfigInstanceElement)element).Path;
        }
    }
}
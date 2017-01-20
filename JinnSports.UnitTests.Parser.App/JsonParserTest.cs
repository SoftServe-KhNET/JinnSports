using DTO.JSON;
using JinnSports.Parser.App.JsonParsers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace JinnSports.UnitTests.Parser.App
{
    [TestFixture]
    public class JsonParserTest
    {
        private JsonParser jsonParser;

        public JsonParserTest()
        {
            this.jsonParser = new JsonParser();
        }

        [Test]
        public void CheckReturnRuJsonString()
        {
            Regex pattern = new Regex("^{.+}$");
            string jsonString = this.jsonParser.GetJsonFromUrl();

            Assert.True(pattern.IsMatch(jsonString));
        }

        [Test]
        public void CheckReturnEnJsonString()
        {
            Regex pattern = new Regex("^{[^А-Яа-я]+}$");
            string jsonString = this.jsonParser.GetJsonFromUrl(this.jsonParser.SiteUri, Locale.EN);

            Assert.True(pattern.IsMatch(jsonString));
        }

        [Test]
        public void CheckDeserializeJson()
        {
            Assert.NotNull(this.jsonParser.DeserializeJson(this.jsonParser.GetJsonFromUrl()));
        }

        [Test]
        public void CheckReturnSportEvents()
        {
            List<SportEventDTO> eventList = this.jsonParser.GetSportEventsList(this.jsonParser.DeserializeJson(this.jsonParser.GetJsonFromUrl()));

            Assert.True(eventList.Count > 0);
        }
    }
}

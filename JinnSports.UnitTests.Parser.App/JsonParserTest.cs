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
        JsonParser jsonParser;

        public JsonParserTest()
        {
            jsonParser = new JsonParser();
        }

        [Test]
        public void CheckReturnRuJsonString()
        {
            Regex pattern = new Regex("^{.+}$");
            string jsonString = jsonParser.GetJsonFromUrl();

            Assert.True(pattern.IsMatch(jsonString));
        }

        [Test]
        public void CheckReturnEnJsonString()
        {
            Regex pattern = new Regex("^{[^А-Яа-я]+}$");
            string jsonString = jsonParser.GetJsonFromUrl(jsonParser.SiteUri, Locale.EN);

            Assert.True(pattern.IsMatch(jsonString));
        }

        [Test]
        public void CheckDeserializeJson()
        {
            Assert.NotNull(jsonParser.DeserializeJson(jsonParser.GetJsonFromUrl()));
        }

        [Test]
        public void CheckReturnSportEvents()
        {
            List<SportEventDTO> eventList = jsonParser.GetSportEventsList(jsonParser.DeserializeJson(jsonParser.GetJsonFromUrl()));

            Assert.True(eventList.Count > 0);
        }
    }
}

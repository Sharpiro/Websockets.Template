using Newtonsoft.Json;

namespace Cards.Core
{
    public class Card
    {
        [JsonProperty("deckPosition")]
        public int DeckPosition { get; set; }
        [JsonProperty("suite")]
        public string Suite { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("displayName")]
        public string DisplayName => $"{Value} of {Suite}";
        [JsonProperty("number")]
        public int Number { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
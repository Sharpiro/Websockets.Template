namespace Websockets.Template.CoreX.CardApp
{
    public class Card
    {
        public int DeckPosition { get; set; }
        public string Suite { get; set; }
        public string Value { get; set; }
        public string DisplayName => $"{Value} of {Suite}";
    }
}

using System.Collections.Generic;

namespace Websockets.Template.CoreX.CardApp
{
    public class Player
    {
        public int Number { get; set; }
        public List<Card> Hand { get; set; }

        public Player()
        {
            Hand = new List<Card>();
        }
    }
}

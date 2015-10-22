using System.Collections.Generic;

namespace Websockets.Template.CoreX.CardApp
{
    public class Player
    {
        public string SocketId { get; set; }
        public int SocketNumber { get; set; }
        public List<Card> Hand { get; set; }

        public Player()
        {
            Hand = new List<Card>();
        }
    }
}

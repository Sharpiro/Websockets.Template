namespace Websockets.Template.CoreX.CardApp
{
    public class Players
    {
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }

        public Players()
        {
            Player1 = new Player
            {
                Number = 1
            };

            Player2 = new Player
            {
                Number = 2
            };
        }
    }
}

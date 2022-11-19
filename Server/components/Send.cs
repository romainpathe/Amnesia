using Server.components.Uno;

namespace Server.components
{
    public class Send
    {
        public Player player;
        public byte[] data;
        public Send(Player player, byte[] data)
        {
            this.player = player;
            this.data = data;
        }
    }
}
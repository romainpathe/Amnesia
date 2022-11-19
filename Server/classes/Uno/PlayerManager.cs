using System.Collections.Generic;
using System.Net.Sockets;
using Server.components.Uno;

namespace Server.classes.Uno
{
    public class PlayerManager
    {
        public readonly List<Player> Players = new List<Player>();
        
        public void AddPlayer(Player player)
        {
            Players.Add(player);
        }
        
        
    }
}
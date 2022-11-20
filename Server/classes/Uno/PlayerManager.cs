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
        
        public void RemovePlayer(Player player)
        {
            Players.Remove(player);
        }
        
        public void RemovePlayerBySocket(Socket socket)
        {
            Players.RemoveAll(player => player.Socket == socket);
        }
        
    }
}
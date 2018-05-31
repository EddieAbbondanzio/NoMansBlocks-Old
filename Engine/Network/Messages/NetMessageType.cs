using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network.Messages {
    /// <summary>
    /// Identifier for various network messages
    /// sent across the network.
    /// </summary>
    public enum NetMessageType : byte {
        Info                = 0 , //Debug junk...
        ConnectionRequest   = 1 , //New client wishing to join
        ConnectionAccepted  = 2 ,
        ClientGreeting      = 3 , 
        Disconnected        = 4 , //Server disconnected you, or alert to server of leaving client.
        LobbySync           = 5 , //Initial lobby recieved message
        PlayerJoined        = 6 , //Alert that another player joined lobby
        PlayerLeft          = 7 , //Alert that another player left the lobby
        TeamChat            = 8 , //Text chat to team members
        LobbyChat           = 9 , //Text chat to lobby
        Command             = 10,
        TimeSyncRequest     = 11,
        TimeSync            = 12,
    }
}

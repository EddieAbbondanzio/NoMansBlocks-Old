using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Network {
    /// <summary>
    /// Identifier for various network messages
    /// sent across the network.
    /// </summary>
    public enum NetMessageType : byte {
        Info,                   //Debug junk...
        ConnectionRequest,      //New client wishing to join
        ConnectionAccepted,
        ClientGreeting,
        Disconnected,           //Server disconnected you.
        LobbySync,              //Initial lobby recieved message
        PlayerJoined,           //Alert that another player joined lobby
        PlayerLeft,             //Alert that another player left the lobby
        TeamChat,               //Text chat to team members
        LobbyChat,              //Text chat to lobby
        Command,
    }
}

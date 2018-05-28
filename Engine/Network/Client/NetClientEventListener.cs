using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Voxelated.Network {
    public sealed class NetClientEventListener : INetEventListener {
        public void OnNetworkError(NetEndPoint endPoint, int socketErrorCode) {
            throw new NotImplementedException();
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency) {
            throw new NotImplementedException();
        }

        public void OnNetworkReceive(NetPeer peer, NetDataReader reader) {
            throw new NotImplementedException();
        }

        public void OnNetworkReceiveUnconnected(NetEndPoint remoteEndPoint, NetDataReader reader, UnconnectedMessageType messageType) {
            throw new NotImplementedException();
        }

        public void OnPeerConnected(NetPeer peer) {
            throw new NotImplementedException();
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) {
            throw new NotImplementedException();
        }
    }
}

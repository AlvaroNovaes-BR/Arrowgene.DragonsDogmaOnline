﻿using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;
using Arrowgene.Networking.Tcp.Consumer.BlockingQueueConsumption;
using Arrowgene.Networking.Tcp.Server.AsyncEvent;

namespace Arrowgene.Ddon.Server.Network
{
    public class Consumer<TClient> : ThreadedBlockingQueueConsumer where TClient : Client
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(Consumer<TClient>));
        private readonly Dictionary<PacketId, IPacketHandler<TClient>> _packetHandlerLookup;
        private readonly Dictionary<ITcpSocket, TClient> _clients;
        private readonly object _lock;
        private readonly ServerSetting _setting;
        private readonly IClientFactory<TClient> _clientFactory;

        public Action<TClient> ClientDisconnected;
        public Action<TClient> ClientConnected;

        public Consumer(ServerSetting setting, AsyncEventSettings socketSetting, IClientFactory<TClient> clientFactory)
            : base(socketSetting, setting.Name)
        {
            _setting = setting;
            _clientFactory = clientFactory;
            _lock = new object();
            _clients = new Dictionary<ITcpSocket, TClient>();
            _packetHandlerLookup = new Dictionary<PacketId, IPacketHandler<TClient>>();
        }

        public void Clear()
        {
            _packetHandlerLookup.Clear();
        }

        public void AddHandler(IPacketHandler<TClient> packetHandler)
        {
            if (_packetHandlerLookup.ContainsKey(packetHandler.Id))
            {
                Logger.Error($"PacketHandlerId: {packetHandler.Id.Name} already exists");
            }
            else
            {
                _packetHandlerLookup.Add(packetHandler.Id, packetHandler);
            }
        }

        protected override void HandleReceived(ITcpSocket socket, byte[] data)
        {
            if (!socket.IsAlive)
            {
                return;
            }

            TClient client;
            lock (_lock)
            {
                if (!_clients.ContainsKey(socket))
                {
                    Logger.Error(socket, "Client does not exist in lookup");
                    return;
                }

                client = _clients[socket];
            }

            List<IPacket> packets = client.Receive(data);
            foreach (IPacket packet in packets)
            {
                HandlePacket(client, packet);
            }
        }

        private void HandlePacket(TClient client, IPacket packet)
        {
            if (!_packetHandlerLookup.ContainsKey(packet.Id))
            {
                Logger.LogUnhandledPacket(client, packet);
                return;
            }

            IPacketHandler<TClient> packetHandler = _packetHandlerLookup[packet.Id];
            try
            {
                packetHandler.Handle(client, packet);
            }
            catch (Exception ex)
            {
                Logger.Exception(client, ex);
                Logger.LogPacketError(client, packet);
            }
        }

        protected override void HandleDisconnected(ITcpSocket socket)
        {
            TClient client;
            lock (_lock)
            {
                if (!_clients.ContainsKey(socket))
                {
                    Logger.Error(socket, $"Disconnected client does not exist in lookup");
                    return;
                }

                client = _clients[socket];
                _clients.Remove(socket);
            }

            Action<TClient> onClientDisconnected = ClientDisconnected;
            if (onClientDisconnected != null)
            {
                try
                {
                    onClientDisconnected.Invoke(client);
                }
                catch (Exception ex)
                {
                    Logger.Exception(client, ex);
                }
            }

            Logger.Info($"Disconnected: {client.Identity}");
        }

        protected override void HandleConnected(ITcpSocket socket)
        {
            TClient client = _clientFactory.NewClient(socket);
            lock (_lock)
            {
                _clients.Add(socket, client);
            }

            Logger.Info($"Connected: {client.Identity}");

            Action<TClient> onClientConnected = ClientConnected;
            if (onClientConnected != null)
            {
                try
                {
                    onClientConnected.Invoke(client);
                }
                catch (Exception ex)
                {
                    Logger.Exception(client, ex);
                }
            }
        }
    }
}

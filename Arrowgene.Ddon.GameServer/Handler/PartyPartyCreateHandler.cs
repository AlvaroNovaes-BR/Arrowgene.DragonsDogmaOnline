﻿using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyCreateHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(PartyPartyCreateHandler));


        public PartyPartyCreateHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_PARTY_PARTY_CREATE_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            client.Send(InGameDump.Dump_103);
            client.Send(InGameDump.Dump_105);
        }
    }
}

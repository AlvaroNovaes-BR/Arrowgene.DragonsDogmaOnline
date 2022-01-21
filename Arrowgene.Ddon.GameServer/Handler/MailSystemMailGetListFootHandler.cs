﻿using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MailSystemMailGetListFootHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MailSystemMailGetListFootHandler));


        public MailSystemMailGetListFootHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_MAIL_SYSTEM_MAIL_GET_LIST_FOOT_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            client.Send(InGameDump.Dump_87);
        }
    }
}

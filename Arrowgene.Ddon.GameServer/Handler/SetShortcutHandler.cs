using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SetShortcutHandler : StructurePacketHandler<GameClient, C2SSetShortcutReq>
    {
        //private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpGetWarpPointListHandler));


        public SetShortcutHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSetShortcutReq> request)
        {
            S2CSetShortcutRes response = new S2CSetShortcutRes();
          //  response.WarpPointID = 0;
          //  response.Rim = 0;

            client.Send(response);
        }
    }
}

using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CLobbyChatMsgNotice : IPacketStructure {

        // Unidentified variables from the PS4 version:
        //  ucType (u8) (use CDataLobbyChatMsgType when identified)
        //  unHandleId (u32)

        public PacketId Id => PacketId.S2C_LOBBY_LOBBY_CHAT_MSG_NTC;

        public byte Type { get; set; }
        public uint Unk1 { get; set; } // Probably HandleId
        public CDataCommunityCharacterBaseInfo CharacterBaseInfo { get; set; }
        public byte Unk2 { get; set; }
        public uint Unk3 { get; set; }
        public uint Unk4 { get; set; }
        public uint Unk5 { get; set; }
        public string Message { get; set; }

        public S2CLobbyChatMsgNotice() {
            Type = 0;
            Unk1 = 0;
            CharacterBaseInfo = new CDataCommunityCharacterBaseInfo();
            Unk2 = 0;
            Unk3 = 0;
            Unk4 = 0;
            Unk5 = 0;
            Message = string.Empty;
        }

        public class Serializer : PacketEntitySerializer<S2CLobbyChatMsgNotice> {

            public override void Write(IBuffer buffer, S2CLobbyChatMsgNotice obj)
            {
                if (obj.Unk3 == 2)
                {
                    uint ui = obj.Unk3; byte b = (byte)ui;
                    WriteByte(buffer, obj.Type);
                    WriteUInt32(buffer, obj.Unk1);
                    WriteUInt32(buffer, obj.CharacterBaseInfo.CharacterId);
                    WriteMtString(buffer, obj.CharacterBaseInfo.CharacterName.FirstName);
                    WriteMtString(buffer, obj.CharacterBaseInfo.CharacterName.LastName);
                    WriteMtString(buffer, "BSP");
                    WriteByte(buffer, b);
                    WriteUInt32(buffer, obj.Unk4);
                    WriteUInt32(buffer, obj.Unk5);
                    WriteMtString(buffer, obj.Message);
                }
                else
                {
                    WriteByte(buffer, obj.Type);
                    WriteUInt32(buffer, obj.Unk1);
                    WriteEntity<CDataCommunityCharacterBaseInfo>(buffer, obj.CharacterBaseInfo);
                    WriteByte(buffer, obj.Unk2);
                    WriteUInt32(buffer, obj.Unk3);
                    WriteUInt32(buffer, obj.Unk4);
                    WriteMtString(buffer, obj.Message);
                }
            }

            public override S2CLobbyChatMsgNotice Read(IBuffer buffer)
            {
                S2CLobbyChatMsgNotice obj = new S2CLobbyChatMsgNotice();
                obj.Type = ReadByte(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.CharacterBaseInfo = ReadEntity<CDataCommunityCharacterBaseInfo>(buffer);
                obj.Unk2 = ReadByte(buffer);
                obj.Unk3 = ReadUInt32(buffer);
                obj.Unk4 = ReadUInt32(buffer);
                obj.Message = ReadMtString(buffer);
                return obj;
            }
        }

    }

}

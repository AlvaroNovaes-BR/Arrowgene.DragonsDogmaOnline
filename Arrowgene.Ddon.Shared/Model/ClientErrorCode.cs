﻿namespace Arrowgene.Ddon.Shared.Model
{
    public class ClientErrorCode : IAsset
    {
        public int Id { get; set; }
        public uint ErrorId => (uint)Id;
        public uint MessageId { get; set; }
        public string ErrorCode { get; set; }
        public string MessageJp { get; set; }
        public string MessageEn { get; set; }
    }
}

using Entities.Abstract;
using Entity.Attributes;
using Entity.Enums;
using Newtonsoft.Json;

namespace Entity.Concrete
{
    public class Message : DeleteEntity
    {
        public Guid Id { get; set; }
        [Masked]
        public string Text { get; set; }
        public short MessageStatus { get; set; } = (short)MessageStatuses.Sent;
        public Guid FromId { get; set; }
        public virtual User FromUser { get; set; }
        public Guid ToId { get; set; }
        public virtual User ToUser { get; set; }
        public int SendDate { get; set; }
        public int SendTime { get; set; }
        public int? ReceivedDate { get; set; }
        public int? ReceivedTime { get; set; }
        public int? ReadDate { get; set; }
        public int? ReadTime { get; set; }
    }
}

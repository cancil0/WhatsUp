using Entity.Attributes;

namespace Entity.Dto.Response
{
    public class GetChatResponse
    {
        public Guid Id { get; set; }
        [Masked]
        public string Text { get; set; }
        public short MessageStatus { get; set; }
        public Guid FromId { get; set; }
        public Guid ToId { get; set; }
        public int SendDate { get; set; }
        public int SendTime { get; set; }
        public int? ReceivedDate { get; set; }
        public int? ReceivedTime { get; set; }
    }
}

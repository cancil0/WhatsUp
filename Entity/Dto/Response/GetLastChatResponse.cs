namespace Entity.Dto.Response
{
    public class GetLastChatResponse
    {
        public string Text { get; set; }
        public int SendDate { get; set; }
        public int SendTime { get; set; }
        public string ToUserNameSurname { get; set; }
        public Guid ToUserId { get; set; }
        public string FromUserNameSurname { get; set; }
        public Guid FromUserId { get; set; }
    }
}

using Entity.Concrete;
using Entity.Dto.Response;

namespace Business.Abstract
{
    public interface IMessageService
    {
        List<GetChatResponse> GetChat(Guid fromUserId, Guid toUserId);
        List<GetLastChatResponse> GetUserLastChats(Guid userId);
    }
}

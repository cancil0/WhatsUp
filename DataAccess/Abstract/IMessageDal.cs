using Entity.Concrete;
using Infrastructure.Repository;

namespace DataAccess.Abstract
{
    public interface IMessageDal : IGenericDal<Message>
    {
        List<Message> GetChat(Guid fromUserId, Guid toUserId, bool throwException = true);
        List<Message> GetUserLastChats(Guid userId);
    }
}

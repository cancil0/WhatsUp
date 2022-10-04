using Core.Extension;
using DataAccess.Abstract;
using Entity.Concrete;
using Entity.Enums;
using Infrastructure.Concrete;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class MessageDal : GenericDal<Message>, IMessageDal
    {
        public MessageDal(Context context) : base(context) { }

        public List<Message> GetChat(Guid fromUserId, Guid toUserId, bool throwException)
        {
            var messages = DbSet
                .Where(x => x.FromId == fromUserId && x.ToId == toUserId)
                .OrderByDescending(x => x.SendDate).ThenByDescending(x => x.SendTime)
                .ToList();

            if (!messages.Any() && throwException)
            {
                throw new AppException("DataAccess.MessageDal.ChatNotFound", ExceptionTypes.NotFound.GetValue(), fromUserId.ToString(), toUserId.ToString());
            }

            return messages;
        }

        public List<Message> GetUserLastChats(Guid userId)
        {
            return DbSet.Where(x => (x.FromId == userId || x.ToId == userId) && x.MessageStatus == (short)MessageStatuses.Sent)
                .OrderByDescending(x => x.SendDate)
                .ThenByDescending(x => x.SendTime)
                .ToList();
        }
    }
}

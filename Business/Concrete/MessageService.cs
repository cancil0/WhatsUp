using Business.Abstract;
using Core.Attributes;
using Core.Concrete;
using Core.Extension;
using DataAccess.Abstract;
using Entity.Concrete;
using Entity.Dto.Response;
using Entity.Enums;

namespace Business.Concrete
{
    public class MessageService : BaseService<Message, IMessageDal>, IMessageService
    {
        private readonly IMessageDal messageDal;
        public MessageService(IMessageDal messageDal)
        {
            this.messageDal = messageDal;
        }

        [UnitofWork]
        public List<GetChatResponse> GetChat(Guid fromUserId, Guid toUserId)
        {
            var messages = messageDal.GetChat(fromUserId, toUserId);
            foreach (var message in messages)
            {
                message.MessageStatus = (short)MessageStatuses.Read;
                message.ReadDate = DateTime.Now.DateToInt();
                message.ReadTime = DateTime.Now.TimeToInt();
            }

            return messages.Select(x => new GetChatResponse
            {
                MessageStatus = (short)MessageStatuses.Read,
                FromId = x.FromId,
                Id = x.Id,
                ReceivedDate = x.ReceivedDate,
                ReceivedTime = x.ReceivedTime,
                SendDate = x.SendDate,
                SendTime = x.SendTime,
                Text = x.Text,
                ToId = x.ToId
            }).ToList();
        }

        [UnitofWork]
        public List<GetLastChatResponse> GetUserLastChats(Guid userId)
        {
            var messages = Service.GetUserLastChats(userId);

            if (!messages.Any())
            {
                return null;
            }

            foreach (var message in messages.Where(x => x.FromId == userId).ToList())
            {
                message.MessageStatus = (short)MessageStatuses.Received;
                message.ReceivedDate = DateTime.Now.DateToInt();
                message.ReceivedTime = DateTime.Now.TimeToInt();
            }

            return messages.Select(x => new GetLastChatResponse()
            {
                Text = x.Text,
                ToUserNameSurname = string.Format("{0} {1}", x.ToUser.Name, x.ToUser.Surname),
                FromUserNameSurname = string.Format("{0} {1}", x.FromUser.Name, x.FromUser.Surname),
                FromUserId = x.FromId,
                ToUserId = x.ToId,
                SendDate = x.SendDate,
                SendTime = x.SendTime
            }).ToList();
        }
    }
}

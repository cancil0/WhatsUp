using Business.Abstract;
using Core.Attributes;
using Core.Concrete;
using DataAccess.Abstract;
using Entity.Concrete;
using Entity.Dto.Response;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : BaseController<IMessageDal, Message, Guid>
    {
        private readonly IMessageService messageService;
        public MessageController(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        /// <summary>
        /// Get User Messages
        /// </summary>
        /// <param name="fromUserId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetChat")]
        [ApiLogger(Response = false)]
        public ActionResult<List<GetChatResponse>> GetChat([FromQuery] Guid fromUserId, [FromQuery] Guid toUserId)
        {
            return Ok(messageService.GetChat(fromUserId, toUserId));
        }

        /// <summary>
        /// Get user chats with user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserLastChats")]
        [ApiLogger(Response = false)]
        public ActionResult<List<GetChatResponse>> GetUserLastChats([FromQuery] Guid userId)
        {
            return Ok(messageService.GetUserLastChats(userId));
        }
    }
}

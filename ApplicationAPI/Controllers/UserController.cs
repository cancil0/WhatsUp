using Business.Abstract;
using Core.Concrete;
using DataAccess.Abstract;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController<IUserDal, User, Guid>
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
    }
}

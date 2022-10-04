using Business.Abstract;
using Core.Concrete;
using DataAccess.Abstract;
using Entity.Concrete;

namespace Business.Concrete
{
    public class UserService : BaseService<User, IUserDal>, IUserService
    {
    }
}

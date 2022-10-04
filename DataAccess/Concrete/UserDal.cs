using DataAccess.Abstract;
using Entity.Concrete;
using Infrastructure.Concrete;
using Infrastructure.Repository;

namespace DataAccess.Concrete
{
    public class UserDal : GenericDal<User>, IUserDal
    {
        public UserDal(Context context) : base(context)
        {
        }
    }
}

using Entity.Concrete;
using Infrastructure.Repository;

namespace DataAccess.Abstract
{
    public interface IUserDal : IGenericDal<User>
    {
    }
}

using AutoMapper;
using Core.Abstract;
using Core.Attributes;
using Core.Extension;
using Entity.Enums;
using Infrastructure.Repository;

namespace Core.Concrete
{
    public class BaseService<TBaseEntity, TService> : IBaseService<TBaseEntity, TService> 
        where TBaseEntity : class
        where TService : class, IGenericDal<TBaseEntity>
    {
        protected T Resolve<T>() => Extensions.Resolve<T>();
        protected TService Service = Extensions.Resolve<TService>();
        protected Mapper IMapper => Resolve<Mapper>();
        protected ILoggerService Logger => Resolve<ILoggerService>();

        public TBaseEntity Get(params object[] keys)
        {
            var entity = Service.Get(keys);
            if (entity == null)
            {
                throw new AppException("Core.Entity.NotFound", ExceptionTypes.NotFound.GetValue(), keys.ToString());
            }
            return entity;
        }

        [UnitofWork]
        public void Create(TBaseEntity baseEntity)
        {
            if (baseEntity != null)
            {
                Service.Insert(baseEntity);
            }
        }

        [UnitofWork]
        public void Update(TBaseEntity baseEntity)
        {
            if (baseEntity != null)
            {
                Service.Update(baseEntity);
            }
        }

        [UnitofWork]
        public void Delete(TBaseEntity baseEntity)
        {
            if (baseEntity != null)
            {
                Service.Delete(baseEntity);
            }
        }

        [UnitofWork]
        public void Delete(params object[] keys)
        {
            var entity = Service.Get(keys);
            if (entity is null)
            {
                throw new AppException("Core.Entity.NotFound", ExceptionTypes.NotFound.GetValue(), keys.ToString());
            }
            Service.Delete(entity);
        }  
    }
}

namespace Core.Abstract
{
    public interface IBaseService<TBaseEntity, TService>
    {
        TBaseEntity Get(params object[] keys);
        void Create(TBaseEntity baseEntity);
        void Update(TBaseEntity baseEntity);
        void Delete(TBaseEntity baseEntity);
        void Delete(params object[] keys);
    }
}

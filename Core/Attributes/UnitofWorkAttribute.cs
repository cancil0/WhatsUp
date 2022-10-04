namespace Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class UnitofWorkAttribute : Attribute
    {
        public bool IsTransactional { get; set; }
        public UnitofWorkAttribute()
        {

        }
    }
}

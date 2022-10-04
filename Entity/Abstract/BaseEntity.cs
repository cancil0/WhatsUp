namespace Entities.Abstract
{
    public abstract class BaseEntity : IBaseEntity
    {
        public bool IsDeleted { get; set; }
        public int CreatedDate { get; set; }
        public int CreatedTime { get; set; }
        public int? UpdatedDate { get; set; }
        public int? UpdatedTime { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}

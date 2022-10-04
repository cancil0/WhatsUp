namespace Entities.Abstract
{
    public abstract class AuditEntity : IAuditEntity
    {
        public int CreatedDate { get; set; }
        public int CreatedTime { get; set; }
        public int? UpdatedDate { get; set; }
        public int? UpdatedTime { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}

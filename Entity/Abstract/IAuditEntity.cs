namespace Entities.Abstract
{
    public interface IAuditEntity
    {
        int CreatedDate { get; set; }
        int CreatedTime { get; set; }
        int? UpdatedDate { get; set; }
        int? UpdatedTime { get; set; }
        string CreatedBy { get; set; }
        string UpdatedBy { get; set; }
    }
}

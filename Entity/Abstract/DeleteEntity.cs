namespace Entities.Abstract
{
    public abstract class DeleteEntity : IDeleteEntity
    {
        public bool IsDeleted { get; set; }
    }
}

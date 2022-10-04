using Entities.Abstract;
using Entity.Attributes;

namespace Entity.Concrete
{
    public class User : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [Masked(4, 4)]
        public string MobilePhone { get; set; }
        public string UserName { get; set; }
        public int BirthDate { get; set; }
        public virtual ICollection<Message> FromMessages { get; set; }
        public virtual ICollection<Message> ToMessages { get; set; }
    }
}

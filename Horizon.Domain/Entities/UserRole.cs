

namespace Horizon.Domain.Entities
{
    public class UserRole : BaseEntity
    {
        public Guid UserId { get; set; }
        public UserInfo User { get; set; } = null!;
        public Guid RoleId { get; set; }
        public RoleInfo Role { get; set; } = null!;
    }
}

namespace Horizon.Domain.Interfaces.Services.CurrentUserServices
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string? Email { get; }
        IEnumerable<string> Roles { get; }
        bool IsAuthenticated { get; }
        bool IsInRole(string role);
    }
}

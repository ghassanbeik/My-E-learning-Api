

namespace Horizon.Application.DTOs
{
    public record TagDto(Guid Id, string Name, string? Description, int UsageCount);
    public record CreateTagDto(string Name, string? Description);
}


namespace Horizon.Application.Common
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base($"{name} with key '{key}' was not found.") { }

        public NotFoundException(string message)
            : base(message) { }
    }

    public class ValidationException : Exception
    {
        public List<string> Errors { get; }

        public ValidationException(List<string> errors)
            : base("One or more validation failures occurred.")
            => Errors = errors;

        public ValidationException(string error)
            : base(error)
            => Errors = new List<string> { error };
    }

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message = "Unauthorized") : base(message) { }
    }

    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message = "You do not have permission to perform this action.") : base(message) { }
    }

    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }
}

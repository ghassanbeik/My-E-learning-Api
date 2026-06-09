namespace Horizon.API.Middleware
{
    public class CurrentUserMiddleware
    {
        private readonly RequestDelegate _next;

        public CurrentUserMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User?.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.FindFirst("userId")?.Value;
                if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out var parsedId))
                    context.Items["UserId"] = parsedId;
            }

            await _next(context);
        }
    }

}

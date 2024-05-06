namespace Ipz
{
    public static class HttpContextExtensions
    {
        private const string CurrentUserIdKey = "CurrentUserId";

        public static string GetCurrentUserId(this HttpContext context)
        {
#pragma warning disable CS8603 // Возможно, возврат ссылки, допускающей значение NULL.
            return context.Items[CurrentUserIdKey] as string;
#pragma warning restore CS8603 // Возможно, возврат ссылки, допускающей значение NULL.
        }

        public static void SetCurrentUserId(this HttpContext context, string currentUserId)
        {
            context.Items[CurrentUserIdKey] = currentUserId;
        }
    }
}

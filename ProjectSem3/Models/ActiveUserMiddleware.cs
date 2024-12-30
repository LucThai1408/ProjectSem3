namespace ProjectSem3.Models
{
    public class ActiveUsersMiddleware
    {
        private readonly RequestDelegate _next;
        private static int _activeUsers = 0;
        private static readonly object _lock = new object();

        public ActiveUsersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            const string sessionKey = "IsActive";

            // Kiểm tra nếu session chưa tồn tại
            if (context.Session.GetString(sessionKey) == null)
            {
                // Đánh dấu session đã được khởi tạo
                context.Session.SetString(sessionKey, "true");

                // Tăng bộ đếm
                lock (_lock)
                {
                    _activeUsers++;
                }
            }

            // Xử lý khi response kết thúc
            context.Response.OnCompleted(() =>
            {
                // Nếu session hết hạn hoặc bị xóa, giảm bộ đếm
                if (context.Session.GetString(sessionKey) == null)
                {
                    lock (_lock)
                    {
                        if (_activeUsers > 0)
                            _activeUsers--;
                    }
                }

                return Task.CompletedTask;
            });

            await _next(context);
        }

        public static int GetActiveUsersCount()
        {
            lock (_lock)
            {
                return _activeUsers;
            }
        }
    }

}

namespace MVCCivicaEmployeeMaster
{
    public class JwtTokenMiddleware
    {
        private readonly RequestDelegate _next;
        public JwtTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var JwtToken = context.Request.Cookies["JwtToken"];
            if (!string.IsNullOrWhiteSpace(JwtToken))
            {
                context.Request.Headers["Authorization"] = "Bearer " + JwtToken;
            }
            await _next(context);
            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                //Redirect to login page
                context.Response.Redirect("/Auth/Login");
            }
        }
    }
}

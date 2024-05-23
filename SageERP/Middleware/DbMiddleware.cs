using System.Security.Claims;
using SageERP.Models;
using Shampan.Models;

namespace SageERP.Middleware
{
    public class DBMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public DBMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context, DbConfig dbConfig)
        {
            // Get the authenticated user
            ClaimsPrincipal user = context.User;

            // Check if the user is authenticated
            if (user.Identity is { IsAuthenticated: true })
            {
                // Access user's claims
                var dbClaim = user.FindFirst(ClaimNames.Database);
                var sageDbClaim = user.FindFirst(ClaimNames.SageDatabase);

                string? dbName = dbClaim?.Value;
                string? sageDbName = sageDbClaim?.Value;
                dbConfig.DbName = dbName;
                dbConfig.SageDbName = sageDbName;


                //New Addition
                //var dbAuthClaim = user.FindFirst(ClaimNames.AuthDatabase);
                //string? dbAuthName = dbAuthClaim?.Value;
                //dbConfig.AuthDB = dbAuthName;
                //end

            }



            await _next(context);




		}

        //public async Task Invoke(HttpContext context)
        //{
        //    // Check if the user is authenticated
        //    if (!context.User.Identity.IsAuthenticated)
        //    {
        //        // Session timeout occurred, redirect to the login page
        //        context.Response.Redirect("/Login/Index"); // Replace with your login page route
        //        return;
        //    }

        //    // Continue processing the request
        //    await _next(context);
        //}
    }


}

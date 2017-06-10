using Jose;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jdash_netcore_tutorial
{
    public class JDashAuthorizer
    {

        public static void Register(IApplicationBuilder app, string endpoint)
        {
            app.Map(new PathString(endpoint), (appBuilder) =>
            {
                appBuilder.Run(async (httpContext) =>
                {
                    var jwtToken = CreateToken(httpContext);
                    await httpContext.Response.WriteAsync(jwtToken);
                });
            });
        }


        static string CreateToken(HttpContext context)
        {
            DateTime issued = DateTime.Now;
            DateTime expire = DateTime.Now.AddHours(10);

            var payload = new Dictionary<string, object>()
            {
                { "data" , new { user = "CURRENT USER NAME/ID" } },
                {"sub", "TODO : REPLACE WITH API KEY"},
                {"iat", ToUnixTime(issued)},
                {"exp", ToUnixTime(expire)} // optional(recommended)
            };

            string token = JWT.Encode(payload, Encoding.UTF8.GetBytes("TODO : REPLACE WITH SECRET KEY"), JwsAlgorithm.HS256);
            return token;
        }

        /// <remarks>
        /// Take a look at http://stackoverflow.com/a/33113820
        /// </remarks>
        static byte[] Base64UrlDecode(string arg)
        {
            return Convert.FromBase64String(arg); // Standard base64 decoder
        }

        static long ToUnixTime(DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }


    }
}

using Nancy;
using Nancy.Security;
using Nancy.Authentication.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NancyInWebApp.Modules
{
    public class Login: NancyModule
    {
        public Login(): base("/Login")
        {
            this.RequiresHttps();
            Get["/"] = parameters => {
                //var userAgent = Request.Headers.UserAgent;

                //if (string.IsNullOrEmpty(userAgent))
                //    return HttpStatusCode.Unauthorized;

                return View["login"];
            };

            Post["/"] = parameters => {
                var username = Request.Form.userName;
                var password = Request.Form.password;

                return this.LoginWithoutRedirect(Guid.NewGuid());
            };
        }
    }
}
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
        public Login()
        {
            this.RequiresHttps();
            Get["/login"] = parameters => {
                return View["login", new { ValidationError = Request.Query.error.HasValue, LoggedOut = Request.Query.loggedOut.HasValue, LoggedIn = Request.Query.loggedIn.HasValue }];
            };

            Post["/login"] = parameters => {
                var username = Request.Form.userName;
                var password = Request.Form.password;

                //todo: validate user name and password against DB

                if(username.Value.Equals("Mathieu") && password.Value.Equals("abc"))
                    return this.Login(Guid.NewGuid(), null, "/Login?loggedIn=1");

                return Response.AsRedirect(string.Format("/login?returnUrl={0}&error=1", (string)Request.Query.returnUrl.Value));
            };

            Get["/logout"] = parameters =>
            {
                return this.Logout("/loggedOut");
            };

            Get["/loggedOut"] = parameters => Response.AsRedirect("/login?loggedOut=1");;
        }
    }
}
using Nancy.Authentication;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NancyInWebApp.Modules
{
    public class Home:Nancy.NancyModule
    {
        public Home(): base("/secure")
        {
            this.RequiresAuthentication();
            this.RequiresHttps();
            Get["/"] = parameters => {
                var cookies = Request.Cookies;
                return "hello!"; 
            };
            
        }
    }
}
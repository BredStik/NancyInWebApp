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
        public Home()
        {
            
            //this.RequiresHttps();
            
            Get["/"] = _ => View["Default"];

            Get["/secure"] = parameters => {
                this.RequiresAuthentication();
                return "hello!"; 
            };
            
        }
    }
}

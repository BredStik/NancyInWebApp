using Nancy.Authentication.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NancyInWebApp
{
    public class CustomBootstrapper: Nancy.DefaultNancyBootstrapper
    {

        protected override void RequestStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines, Nancy.NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);            

            var formsAuthConfiguration = new FormsAuthenticationConfiguration
            {
                RedirectUrl = "~/login",
                UserMapper = new UserMapper(),
                DisableRedirect = context.Request.Headers.Keys.Contains("Client") && context.Request.Headers["Client"].Contains("RichClient")
            };

            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
        }
    }

    public class UserMapper : IUserMapper {

        public Nancy.Security.IUserIdentity GetUserFromIdentifier(Guid identifier, Nancy.NancyContext context)
        {
            return new UserIdentity("Mathieu");
        }
    }
}
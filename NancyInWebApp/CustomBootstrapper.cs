using Nancy.Authentication.Forms;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
                RequiresSSL = true,
                DisableRedirect = (context.Request.Headers.Keys.Contains("Client") && context.Request.Headers["Client"].Contains("RichClient")) || context.Request.Url.Path.Contains("/login")
            };

            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
        }

        protected override void ApplicationStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
        }
    }

    public class UserMapper : IUserMapper {

        private static readonly IDictionary<Guid, UserIdentityContainer> _cachedIdentities = new ConcurrentDictionary<Guid, UserIdentityContainer>();

        public Nancy.Security.IUserIdentity GetUserFromIdentifier(Guid identifier, Nancy.NancyContext context)
        {
            RemoveExpiredIdentities();

            if (_cachedIdentities.ContainsKey(identifier))
                return _cachedIdentities[identifier].Identity;

            //todo: build from database with claims
            var identity = new UserIdentity("Mathieu");

            _cachedIdentities.Add(identifier, new UserIdentityContainer(identity, DateTime.Now.AddSeconds(10)));           
            
            return identity;
        }

        private void RemoveExpiredIdentities()
        {
            var expiredIdentityKeys = _cachedIdentities.Where(x => x.Value.IsExpired).Select(x => x.Key);

            foreach (var identityKey in expiredIdentityKeys)
            {
                _cachedIdentities.Remove(identityKey);
            }
        }

        private class UserIdentityContainer
        {
            private readonly UserIdentity _identity;
            private readonly DateTime _expires;

            public UserIdentityContainer(UserIdentity identity, DateTime expires)
            {
                _identity = identity;
                _expires = expires;
            }

            public UserIdentity Identity { get { return _identity; } }
            public bool IsExpired { get { return DateTime.Now >= _expires; } }
        }
    }
}
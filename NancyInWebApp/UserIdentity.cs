using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NancyInWebApp
{
    public class UserIdentity: IUserIdentity
    {
        private readonly string _userName;
        private readonly string[] _claims;

        public UserIdentity(string userName, params string[] claims)
        {
            _userName = userName;
            _claims = claims;
        }

        public IEnumerable<string> Claims
        {
            get { return _claims; }
        }

        public string UserName
        {
            get { return _userName; }
        }
    }
}

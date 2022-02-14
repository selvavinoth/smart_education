using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;

namespace SmartEdu.SessionHolder
{
    /// <summary>
    /// It is created  for custom authentication and works well with form authentication in web.config and base controller.
    /// custom principal wraps the user identity and roles and bound to Http context and Current Thread.
    /// See the url <see cref="http://bradygaster.com/custom-authentication-with-mvc-3.0"/>
    /// </summary>
    public class CustomPrincipal : IPrincipal
    {
        /// <summary>
        /// it is an constructor for customprincipal class.
        /// </summary>
        /// <param name="identity"> custom identity object wrapped in prinicpal object</param>
        public CustomPrincipal(CustomIdentity identity)
        {
            this.Identity = identity;
        }

        #region IPrincipal Members
        /// <summary>
        /// This property holds custom identity object.
        /// </summary>
        public IIdentity Identity { get; private set; }

        /// <summary>
        /// to check whether the identity is given an role.
        /// </summary>
        /// <param name="role"> denotes the role name</param>
        /// <returns>true if identity is attached to role. otherwise returns false</returns>
        public bool IsInRole(string role)
        {
            return true; // need to be updated later
        }

        #endregion
    }

    /// <summary>
    /// It is created  for custom authentication and works well with form authentication in web.config and base controller.
    /// custom identity is the part of the custom principal and herein it is used for storing logged-in user name.
    /// </summary>

    public class CustomIdentity : IIdentity
    {
        /// <summary>
        ///  It is an constructor for the idenitity.
        /// </summary>
        /// <param name="name">name of the identity.</param>
        public CustomIdentity(string name)
        {
            this.Name = name;
        }

        #region IIdentity Members
        /// <summary>
        /// returns authentication type.
        /// </summary>
        public string AuthenticationType
        {
            get { return "Custom"; }
        }

        /// <summary>
        /// returns true if the identity name is not null or empty.
        /// </summary>
        public bool IsAuthenticated
        {
            get { return !string.IsNullOrEmpty(this.Name); }
        }

        public string Name { get; private set; }

        #endregion
    }
}
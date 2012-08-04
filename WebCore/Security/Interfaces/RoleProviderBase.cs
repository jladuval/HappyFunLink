namespace WebCore.Security.Interfaces
{
    using System.Web.Security;
    using Entities;

    public abstract class RoleProviderBase : RoleProvider
    {
        public abstract void AddUserToRole(string email, string roleName);

        public abstract void AddUserToRole(User user, string roleName);

        public abstract void RemoveUserFromRole(string email, string roleName);
    }
}

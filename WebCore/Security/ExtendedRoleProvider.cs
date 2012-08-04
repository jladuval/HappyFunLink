namespace WebCore.Security
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Data.Entity;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Data.Interfaces;
    using Entities;
    using Interfaces;

    public class ExtendedRoleProvider : RoleProviderBase
    {
        #region Constants and Fields

        private const string ApplicationNameProperty = "applicationName";

        private readonly IRepository<User> _users;

        private readonly IRepository<Role> _roles;

        private readonly IUnitOfWork _uow;

        #endregion

        #region Constructors and Destructors

        public ExtendedRoleProvider()
            : this(
                DependencyResolver.Current.GetService<IUnitOfWork>(),
                DependencyResolver.Current.GetService<IRepository<User>>(),
                DependencyResolver.Current.GetService<IRepository<Role>>())
        {
        }

        public ExtendedRoleProvider(IUnitOfWork uow, IRepository<User> users, IRepository<Role> roles)
        {
            _uow = uow;
            _users = users;
            _roles = roles;
        }

        #endregion

        #region Public Properties

        public override string ApplicationName { get; set; }

        #endregion

        #region Public Methods and Operators

        public override void Initialize(string name, NameValueCollection config)
        {
            if (null == config)
            {
                throw new ArgumentNullException("config");
            }

            if (string.IsNullOrEmpty(name))
            {
                name = GetType().Name;
            }

            ApplicationName = config[ApplicationNameProperty];

            base.Initialize(name, config);
        }

        public override void AddUserToRole(string email, string roleName)
        {
            var user = _users.Find(e => e.Email == email).SingleOrDefault();
            ErrorUtility.CheckIfNotNull(user, email);

            AddUserToRole(user, roleName);
        }

        public override void AddUserToRole(User user, string roleName)
        {
            if (null == user)
                throw new ArgumentNullException("user");

            var role = _roles.Find(e => e.RoleName == roleName).SingleOrDefault();
            ErrorUtility.CheckIfNotNull(role, roleName);

            user.Roles = new Collection<Role> { role };
            _uow.Commit();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                throw ErrorUtility.CreateArgumentNullOrEmptyException("roleName");

            if (RoleExists(roleName))
                throw new InvalidOperationException(
                    string.Format(CultureInfo.InvariantCulture, "Role '{0}' already exists", roleName));

            var role = new Role { RoleName = roleName };
            _roles.Create(role);
            _uow.Commit();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            var role = _roles.Find(e => e.RoleName == roleName).Include(e => e.Users).SingleOrDefault();
            ErrorUtility.CheckIfNotNull(role, roleName);

            if (throwOnPopulatedRole && role.Users.Any())
                throw new InvalidOperationException(
                    string.Format(CultureInfo.InvariantCulture, "Role '{0}' is already populated", roleName));

            if (role.Users.Any()) return false;

            _roles.Delete(role);
            _uow.Commit();
            return true;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            return _roles.GetAll().Select(e => e.RoleName).ToArray();
        }

        public override string[] GetRolesForUser(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw ErrorUtility.CreateArgumentNullOrEmptyException("email");

            var user = _users.Find(e => e.Email == email).Include(e => e.Roles).SingleOrDefault();
            ErrorUtility.CheckIfNotNull(user, email);

            return user.Roles.Select(e => e.RoleName).ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                throw ErrorUtility.CreateArgumentNullOrEmptyException("roleName");

            var role = _roles.Find(e => e.RoleName == roleName).Include(e => e.Users).SingleOrDefault();
            ErrorUtility.CheckIfNotNull(role, roleName);

            return role.Users.Select(e => e.Email).ToArray();
        }

        public override bool IsUserInRole(string email, string roleName)
        {
            return GetRolesForUser(email).Contains(roleName);
        }

        public override void RemoveUserFromRole(string email, string roleName)
        {
            var user = _users.Find(e => e.Email == email).Include(e => e.Roles).SingleOrDefault();
            ErrorUtility.CheckIfNotNull(user, email);

            var role = user.Roles.SingleOrDefault(e => e.RoleName == roleName);
            ErrorUtility.CheckIfNotNull(role, roleName);

            user.Roles.Remove(role);
            _uow.Commit();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            return _roles.Find(e => e.RoleName == roleName).Any();
        }

        #endregion
    }
}

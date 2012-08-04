namespace WebCore
{
    using System;
    using System.Globalization;
    using Entities;

    public class ErrorUtility
    {
        public static ArgumentException CreateArgumentNullOrEmptyException(string name)
        {
            return new ArgumentException(string.Format(CultureInfo.InvariantCulture,
                "Argument cannot be null or empty: {0}", name));
        }

        public static void CheckIfNotNull(object value, string name)
        {
            if (null == value)
                throw new ArgumentNullException(name);
        }

        public static void CheckArgument(string value, string name)
        {
            if (string.IsNullOrEmpty(value))
                throw CreateArgumentNullOrEmptyException(name);
        }

        public static void CheckIfNotNull(object value, string param, string template)
        {
            if (null == value)
                throw new InvalidOperationException(
                    string.Format(CultureInfo.InvariantCulture, template, param));
        }

        public static void CheckIfNotNull(User value, string email)
        {
            CheckIfNotNull(value, email, "Cannot find '{0}' user");
        }

        public static void CheckIfNotNull(Activation value, string email)
        {
            CheckIfNotNull(value, email, "Cannot find activation info for '{0}'");
        }

        public static void CheckIfNotNull(Role value, string role)
        {
            CheckIfNotNull(value, role, "Cannot find '{0}' role");
        }

    }
}

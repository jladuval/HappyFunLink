namespace Entities
{
    using System;
    using System.Collections.Generic;

    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        public virtual ICollection<Activation> Activations { get; set; }

        public virtual SiteRegistration SiteRegistration { get; set; }
    }
}

namespace Entities
{
    using System;

    public class Activation
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public string ConfirmationToken { get; set; }

        public DateTime? ActivatedDate { get; set; }
    }
}

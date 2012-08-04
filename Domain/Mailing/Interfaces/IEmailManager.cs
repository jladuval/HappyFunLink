namespace Domain.Mailing.Interfaces
{
    using Entities;

    public interface IEmailManager
    {
        void SendActivationEmail(User user, string activationToken);

        void SendWelcomeEmail(User user);
    }
}

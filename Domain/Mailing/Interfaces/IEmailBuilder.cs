namespace Domain.Mailing.Interfaces
{
    using System.Net.Mail;
    using Entities;

    public interface IEmailBuilder
    {
        MailMessage Welcome(User user);

        MailMessage Activation(User user, string activationToken);
    }
}

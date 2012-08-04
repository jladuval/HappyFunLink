namespace Domain.Mailing
{
    using System.Net.Mail;
    using Entities;
    using Interfaces;
    using global::Mvc.Mailer;

    public class EmailManager : IEmailManager
    {
        private readonly IEmailBuilder _mailer;

        public EmailManager(IEmailBuilder mailer)
        {
            _mailer = mailer;
        }

        public void SendActivationEmail(User user, string activationToken)
        {
            var message = _mailer.Activation(user, activationToken);
            SendMessage(message);
        }

        public void SendWelcomeEmail(User user)
        {
            var message = _mailer.Welcome(user);
            SendMessage(message);
        }

        private static void SendMessage(MailMessage message)
        {
            try
            {
                message.Send();
            }
            catch (SmtpException)
            {
            }
        }
    }
}

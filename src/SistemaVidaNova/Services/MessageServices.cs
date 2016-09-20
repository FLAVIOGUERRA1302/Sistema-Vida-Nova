using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;

namespace SistemaVidaNova.Services
{
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Sistema Vida Nova", "criarEmailSistemaVidaNova@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            using (var client = new SmtpClient())
            {
                client.LocalDomain = "some.domain.com";//colocar o dominio do sistema ex: vidanova.com.br
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.SslOnConnect).ConfigureAwait(false);
                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
            
        }

        public Task SendSmsAsync(string number, string message)
        {
            // não vou implementar
            return Task.FromResult(0);
        }
    }
}

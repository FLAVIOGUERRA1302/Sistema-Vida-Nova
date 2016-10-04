using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using System.Security.Cryptography.X509Certificates;
using Google.Apis.Auth.OAuth2;
using MimeKit.Text;
using SistemaVidaNova.Models;

namespace SistemaVidaNova.Services
{
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public void  SendEmailAsync(string email, string subject, string message, List<MimePart> attachments)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Sistema Vida Nova", "vidanovasistema@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            if (attachments.Count > 0) // se contem anexos
            {
                var multipart = new Multipart("mixed");
                multipart.Add(new TextPart(TextFormat.Html) { Text =  message });// texto da mensagem
                foreach(var attach in attachments) // anexos
                {
                    multipart.Add(attach);
                }
                emailMessage.Body = multipart;


            }
            else
            {
                emailMessage.Body = new TextPart(TextFormat.Html) { Text = "<div>" + message + "</div>" };
            }
            
            
            using (var client = new SmtpClient())
            {
                //client.LocalDomain = "localhost";//colocar o dominio do sistema ex: vidanova.com.br
                                
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                

                client.Connect("smtp.gmail.com", 465, true);// 465 587

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                
                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("vidanovasistema@gmail.com", "Vid@nova");

                client.Send(emailMessage);
                client.Disconnect(true);
            }
            

        }


        public void SendEmailAsync(string email, string subject, string message )
        {
            List<MimePart> attachments = new List<MimePart>();
            SendEmailAsync(email, subject, message, attachments);
        }



        public Task SendSmsAsync(string number, string message)
        {
            // não vou implementar
            return Task.FromResult(0);
        }

        public void SendEmailAsync(List<string> emails, string subject, string message, List<MimePart> attachments)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Sistema Vida Nova", "vidanovasistema@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", "vidanovasistema@gmail.com"));
            foreach(string email in emails)
            {
                emailMessage.Bcc.Add(new MailboxAddress("", email));
            }
            emailMessage.Subject = subject;
            if (attachments.Count > 0) // se contem anexos
            {
                var multipart = new Multipart("mixed");
                multipart.Add(new TextPart(TextFormat.Html) { Text = message });// texto da mensagem
                foreach (var attach in attachments) // anexos
                {
                    multipart.Add(attach);
                }
                emailMessage.Body = multipart;


            }
            else
            {
                emailMessage.Body = new TextPart(TextFormat.Html) { Text = "<div>" + message + "</div>" };
            }


            using (var client = new SmtpClient())
            {
                //client.LocalDomain = "localhost";//colocar o dominio do sistema ex: vidanova.com.br

                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;


                client.Connect("smtp.gmail.com", 465, true);// 465 587

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");


                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("vidanovasistema@gmail.com", "Vid@nova");

                client.Send(emailMessage);
                client.Disconnect(true);
            }


        }
    }
}

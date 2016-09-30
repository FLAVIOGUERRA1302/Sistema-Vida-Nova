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

namespace SistemaVidaNova.Services
{
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public void  SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Sistema Vida Nova", "vidanovasistema@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(TextFormat.Html) { Text = "<div>"+message+"</div>"  };

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





           


            /*
            var certificate = new X509Certificate2(@"C:\path\to\certificate.p12", "password", X509KeyStorageFlags.Exportable);
            var credential = new ServiceAccountCredential(new ServiceAccountCredential
                .Initializer("AIzaSyDikzVxptTIOS4d0Nvx5bTLt41BgDwZ-PU@developer.gserviceaccount.com")
            {
                // Note: other scopes can be found here: https://developers.google.com/gmail/api/auth/scopes
                Scopes = new[] { "https://mail.google.com/" },
                User = "vidanovasistema@gmail.com"
            }.FromCertificate(certificate));

            //You can also use FromPrivateKey(privateKey) where privateKey
            // is the value of the fiel 'private_key' in your serviceName.json file

            bool result = await credential.RequestAccessTokenAsync(cancel.Token);

            // Note: result will be true if the access token was received successfully
            Now that you have an access token(credential.Token.AccessToken), you can use it with MailKit as if it were the password:

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587);

                // use the OAuth2.0 access token obtained above as the password
                client.Authenticate("mymail@gmail.com", credential.Token.AccessToken);

                client.Send(message);
                client.Disconnect(true);
            }*/

        }

        public Task SendSmsAsync(string number, string message)
        {
            // não vou implementar
            return Task.FromResult(0);
        }
    }
}

using MimeKit;
using SistemaVidaNova.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Services
{
    
    public interface IEmailSender
    {
        void SendEmailAsync(string email, string subject, string message);
        void SendEmailAsync(string email, string subject, string message, List<MimePart> attachments);

        void SendEmailAsync(List<string> emails, string subject, string message, List<MimePart> attachments);
    }
}

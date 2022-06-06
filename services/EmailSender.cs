﻿using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xSteak.services
{
    public class EmailSender : IEmailSender
    {
        public EmailOption Options { get; set; }

        public EmailSender(IOptions<EmailOption> EmailOptions)
        {
            Options = EmailOptions.Value;

        }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendGridKey, subject, message, email);
        }

        private Task Execute(string sendGridKey, string subject, string message, string email)
        {
            var client = new SendGridClient(sendGridKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("karimmahdy43@gmail.com", "xSteak Restaurant"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = email
            };
            msg.AddTo(new EmailAddress(email));
            try
            {
                return client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {

            }
            return null;
        }
    }
}

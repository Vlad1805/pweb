using System.Net;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MobyLabWebProgramming.Core.Errors;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Configurations;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

public class MailService : IMailService
{
    private readonly MailConfiguration _mailConfiguration;

    public MailService(IOptions<MailConfiguration> mailConfiguration) => _mailConfiguration = mailConfiguration.Value;

    public async Task<ServiceResponse> SendMail(string recipientEmail, string subject, string body, bool isHtmlBody = false, 
        string? senderTitle = default, CancellationToken cancellationToken = default)
    {
        if (!_mailConfiguration.MailEnable)
        {
            return ServiceResponse.ForSuccess();
        }

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(senderTitle ?? _mailConfiguration.MailAddress, _mailConfiguration.MailAddress));
        message.To.Add(new MailboxAddress(recipientEmail, recipientEmail));
        message.Subject = subject;
        message.Body = new TextPart(isHtmlBody ? "html" : "plain") { Text = body };

        try
        {
            using var client = new SmtpClient();
            await client.ConnectAsync(_mailConfiguration.MailHost, _mailConfiguration.MailPort, SecureSocketOptions.Auto, cancellationToken);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(_mailConfiguration.MailUser, _mailConfiguration.MailPassword, cancellationToken);
            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);
        }
        catch
        {
            return ServiceResponse.FromError(new(HttpStatusCode.ServiceUnavailable, "Mail couldn't be send!", ErrorCodes.MailSendFailed));
        }

        return ServiceResponse.ForSuccess();
    }
}

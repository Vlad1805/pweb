using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

public interface IMailService
{
    public Task<ServiceResponse> SendMail(string recipientEmail, string subject, string body, bool isHtmlBody = false,
        string? senderTitle = default, CancellationToken cancellationToken = default);
}

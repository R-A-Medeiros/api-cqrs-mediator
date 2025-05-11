using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArch.Application.Members.Commands.Notifications;

public class MemberCreatedEmailHandler : INotificationHandler<MemberCreatedNotification>
{
    private readonly ILogger<MemberCreatedEmailHandler>? _logger;
    public MemberCreatedEmailHandler(ILogger<MemberCreatedEmailHandler>? logger)
    {
        _logger = logger;
    }

    Task INotificationHandler<MemberCreatedNotification>.Handle(MemberCreatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Confirmation email sent for : {notification.Member.LastName}");

        //logica para enviar Email

        return Task.CompletedTask;
    }
}

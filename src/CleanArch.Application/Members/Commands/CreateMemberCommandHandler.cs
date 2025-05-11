using CleanArch.Application.Members.Commands.Notifications;
using CleanArch.Domain.Abstractions;
using CleanArch.Domain.Entities;
using FluentValidation;
using MediatR;

namespace CleanArch.Application.Members.Commands;

public class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, Member>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IValidator<CreateMemberCommand> _validator;

    public CreateMemberCommandHandler(IUnitOfWork unitOfWork, IMediator mediator, IValidator<CreateMemberCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _validator = validator;
    }
    public async Task<Member> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        _validator.Validate(request);

        var newMember = new Member(request.FirstName, request.LastName, request.Gender, request.Email, request.IsActive);

        await _unitOfWork.MemberRepository.AddMember(newMember);
        await _unitOfWork.CommitAsync();

        await _mediator.Publish(new MemberCreatedNotification(newMember), cancellationToken);

        return newMember;
    }
}

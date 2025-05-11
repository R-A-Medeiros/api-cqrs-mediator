using CleanArch.Domain.Entities;
using MediatR;

namespace CleanArch.Application.Members.Queries;

public class GetMembersQuery : IRequest<IEnumerable<Member>>
{
}

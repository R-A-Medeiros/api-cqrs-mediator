using System.Threading.Tasks;
using CleanArch.Domain.Abstractions;
using CleanArch.Domain.Entities;
using CleanArch.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CleanArch.Infrastructure.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly AppDbContext _db;

    public MemberRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Member> AddMember(Member member)
    {
        if(member is null)
            throw new ArgumentNullException(nameof(member));

        await _db.Members.AddAsync(member);
        return member;
    }

    public async Task<Member> DeleteMember(int memberId)
    {
        var member = await GetMemberById(memberId);

        if (member is null)
            throw new InvalidOperationException("Member not found");

        _db.Members.Remove(member);
        return member;

    }

    public async Task<Member> GetMemberById(int memberId)
    {
        var member = await _db.Members.FindAsync(memberId);

        if (member is null)
            throw new InvalidOperationException("Member not found");

        return member;
    }

    public async Task<IEnumerable<Member>> GetMembers()
    {
        var memberList = await _db.Members.ToListAsync();
        return memberList ?? Enumerable.Empty<Member>();
    }

    public void UpdateMember(Member member)
    {
       if (member is null)
            throw new ArgumentNullException(nameof(member));

       _db.Members.Update(member);
    }
}

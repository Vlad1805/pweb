using MobyLabWebProgramming.Core.Entities;
using Ardalis.Specification;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class UserSpec : BaseSpec<UserSpec, User>
{
    public UserSpec(Guid id) : base(id)
    {
    }

    public UserSpec(string email)
    {
        Query.Where(e => e.Email == email);
    }
}
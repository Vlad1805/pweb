using System.Linq.Expressions;
using Ardalis.Specification;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

public abstract class BaseSpec<TDerived, T> : Specification<T> where TDerived : BaseSpec<TDerived, T> where T : BaseEntity
{
    public sealed override ISpecificationBuilder<T> Query => base.Query;

    protected BaseSpec(bool orderByCreatedAt = true) => Query.OrderBy(x => x.CreatedAt, orderByCreatedAt);
    protected BaseSpec(Guid id) => Query.Where(e => e.Id == id);
    protected BaseSpec(ICollection<Guid> ids, bool orderByCreatedAt = true) => Query.Where(e => ids.Contains(e.Id)).OrderBy(e => e.CreatedAt, orderByCreatedAt);
}

public abstract class BaseSpec<TDerived, T, TOut> : Specification<T, TOut> where TDerived : BaseSpec<TDerived, T, TOut> where T : BaseEntity
{
    public sealed override ISpecificationBuilder<T, TOut> Query => base.Query;
    protected abstract Expression<Func<T, TOut>> Spec { get; }
    protected TDerived Derived => (TDerived) this;

    protected BaseSpec(bool orderByCreatedAt = true) => Query.Select(Derived.Spec).OrderBy(x => x.CreatedAt, orderByCreatedAt);
    protected BaseSpec(Guid id) => Query.Select(Derived.Spec).Where(e => e.Id == id);
    protected BaseSpec(ICollection<Guid> ids, bool orderByCreatedAt = true) => Query.Select(Derived.Spec).Where(e => ids.Contains(e.Id)).OrderBy(e => e.CreatedAt, orderByCreatedAt);
}
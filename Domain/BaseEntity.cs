using System;

namespace Domain;

public abstract class BaseEntity<TId>
{
    public TId Id { get; protected set; } = default!;
    protected BaseEntity() { }
}
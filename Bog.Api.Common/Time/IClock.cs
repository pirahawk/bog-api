using System;

namespace Bog.Api.Common.Time
{
    public interface IClock
    {
        DateTimeOffset Now { get; }
    }
}
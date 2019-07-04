using System;

namespace Bog.Api.Common.Time
{
    public class Clock : IClock
    {
        public DateTimeOffset Now => DateTimeOffset.UtcNow;
    }
}
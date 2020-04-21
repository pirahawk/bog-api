using System;
using Bog.Api.Common.Time;

namespace Bog.Api.Common.Tests.Time
{
    public class MockClock : IClock
    {
        public DateTimeOffset? MockTime { get; set; }

        public DateTimeOffset Now => MockTime.GetValueOrDefault();

        public MockClock()
        {
            MockTime = new DateTimeOffset(2020, 1, 1, 12, 12, 12, TimeSpan.FromSeconds(0));
        }
    }
}
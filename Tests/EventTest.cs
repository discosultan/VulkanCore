using VulkanCore.Tests.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace VulkanCore.Tests
{
    public class EventTest : HandleTestBase
    {
        [Fact]
        public void SetEvent()
        {
            using (Event evt = Device.CreateEvent())
                evt.Set();
        }

        [Fact]
        public void ResetEvent()
        {
            using (Event evt = Device.CreateEvent())
                evt.Reset();
        }

        [Fact]
        public void GetEventStatus()
        {
            using (Event evt = Device.CreateEvent())
            {
                Result status = evt.GetStatus();
                Assert.Equal(Result.EventReset, status);

                evt.Set();
                status = evt.GetStatus();
                Assert.Equal(Result.EventSet, status);

                evt.Reset();
                status = evt.GetStatus();
                Assert.Equal(Result.EventReset, status);
            }
        }

        public EventTest(DefaultHandles defaults, ITestOutputHelper output) : base(defaults, output) { }
    }
}

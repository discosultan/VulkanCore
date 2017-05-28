using VulkanCore.Tests.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace VulkanCore.Tests
{
    public class RenderPassTest : HandleTestBase
    {
        [Fact]
        public void GetRenderAreaGranularity()
        {
            using (RenderPass renderPass = CreateRenderPass())
            {
                Extent2D granularity = renderPass.GetRenderAreaGranularity();
                Assert.True(granularity.Width > 0);
                Assert.True(granularity.Height > 0);
            }
        }

        [Fact]
        public void CreateFramebuffer()
        {
            var renderPassCreateInfo = new RenderPassCreateInfo(
                new[] { new SubpassDescription() }
            );
            using (RenderPass renderPass = Device.CreateRenderPass(renderPassCreateInfo))
            {
                var framebufferCreateInfo = new FramebufferCreateInfo(null, 2, 2);
                using (renderPass.CreateFramebuffer(framebufferCreateInfo)) { }
                using (renderPass.CreateFramebuffer(framebufferCreateInfo, CustomAllocator)) { }
            }
        }

        private RenderPass CreateRenderPass()
        {
            var attachments = new[]
            {
                new AttachmentDescription
                {
                    Samples = SampleCounts.Count1,
                    Format = Format.B8G8R8A8UNorm,
                    InitialLayout = ImageLayout.PresentSrcKhr,
                    FinalLayout = ImageLayout.PresentSrcKhr,
                    LoadOp = AttachmentLoadOp.Clear,
                    StoreOp = AttachmentStoreOp.Store,
                    StencilLoadOp = AttachmentLoadOp.DontCare,
                    StencilStoreOp = AttachmentStoreOp.DontCare
                }
            };
            var subpasses = new[]
            {
                new SubpassDescription
                {
                    ColorAttachments = new[] { new AttachmentReference(0, ImageLayout.ColorAttachmentOptimal) }
                }
            };

            var createInfo = new RenderPassCreateInfo(subpasses, attachments);
            return Device.CreateRenderPass(createInfo);
        }

        public RenderPassTest(DefaultHandles defaults, ITestOutputHelper output) : base(defaults, output) { }
    }
}

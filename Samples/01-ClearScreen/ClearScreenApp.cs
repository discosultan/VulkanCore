using System;
using VulkanCore.Khr;

namespace VulkanCore.Samples.ClearScreen
{
    public class ClearScreenApp : VulkanApp
    {
        public ClearScreenApp(IntPtr hInstance, IWindow window) : base(hInstance, window)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            RecordCommandBuffers();
        }

        protected override void OnResized()
        {
            base.OnResized();
            // Command buffers are automatically reset on resize by parent class.
            RecordCommandBuffers();
        }

        protected override void Draw(Timer timer)
        {
            // Acquire drawing image.
            int imageIndex = Swapchain.AcquireNextImage(semaphore: ImageAvailableSemaphore);

            GraphicsQueue.Submit(
                ImageAvailableSemaphore,
                PipelineStages.Transfer,
                CommandBuffers[imageIndex],
                RenderingFinishedSemaphore
            );

            PresentQueue.PresentKhr(RenderingFinishedSemaphore, Swapchain, imageIndex);
        }

        private void RecordCommandBuffers()
        {
            var imageSubresourceRange = new ImageSubresourceRange(ImageAspects.Color);
            for (int i = 0; i < SwapchainImages.Length; i++)
            {
                var barrierFromPresentToClear = new ImageMemoryBarrier(
                    SwapchainImages[i], imageSubresourceRange,
                    Accesses.MemoryRead, Accesses.TransferWrite,
                    ImageLayout.Undefined, ImageLayout.TransferDstOptimal);

                var barrierFromClearToPresent = new ImageMemoryBarrier(
                    SwapchainImages[i], imageSubresourceRange,
                    Accesses.TransferWrite, Accesses.MemoryRead,
                    ImageLayout.TransferDstOptimal, ImageLayout.PresentSrcKhr);
                
                CommandBuffer cmdBuffer = CommandBuffers[i];
                cmdBuffer.Begin(new CommandBufferBeginInfo(CommandBufferUsages.SimultaneousUse));

                cmdBuffer.CmdPipelineBarrier(
                    PipelineStages.Transfer, PipelineStages.Transfer,
                    imageMemoryBarriers: new[] { barrierFromPresentToClear });
                cmdBuffer.CmdClearColorImage(
                    SwapchainImages[i], 
                    ImageLayout.TransferDstOptimal,
                    new ClearColorValue(new ColorF4(0.39f, 0.58f, 0.93f, 1.0f)),
                    imageSubresourceRange);
                cmdBuffer.CmdPipelineBarrier(
                    PipelineStages.Transfer, PipelineStages.BottomOfPipe,
                    imageMemoryBarriers: new[] { barrierFromClearToPresent });

                cmdBuffer.End();
            }
        }
    }
}

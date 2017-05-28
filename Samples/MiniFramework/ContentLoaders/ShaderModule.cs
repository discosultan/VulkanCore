using System.IO;

namespace VulkanCore.Samples.ContentLoaders
{
    internal static partial class Loader
    {
        public static ShaderModule LoadShaderModule(IVulkanAppHost host, VulkanContext ctx, string path)
        {
            const int defaultBufferSize = 4096;
            using (Stream stream = host.Open(path))
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms, defaultBufferSize);
                return ctx.Device.CreateShaderModule(new ShaderModuleCreateInfo(ms.ToArray()));
            }
        }
    }
}

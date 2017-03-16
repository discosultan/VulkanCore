using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace VulkanCore.Samples
{
    public class ContentManager : IDisposable
    {
        private readonly GraphicsDevice _device;
        private readonly string _contentRoot;
        private readonly Dictionary<string, IDisposable> _cachedContent = new Dictionary<string, IDisposable>();

        public ContentManager(GraphicsDevice device, string contentRoot)
        {
            _device = device;
            _contentRoot = contentRoot;
        }

        public async Task<T> LoadAsync<T>(string contentName)
        {
            if (_cachedContent.TryGetValue(contentName, out IDisposable value))
                return (T)value;

            string path = Path.Combine(_contentRoot, contentName);
            Type type = typeof(T);
            if (type == typeof(ShaderModule))
            {
                value = await LoadShaderAsync(path);
            }
            else if (type == typeof(Texture))
            {
                value = await LoadTextureAsync(path);
            }

            _cachedContent.Add(contentName, value);
            return (T)value;
        }

        public void Dispose()
        {
            foreach (IDisposable value in _cachedContent.Values)
                value.Dispose();
            _cachedContent.Clear();
        }

        private async Task<IDisposable> LoadShaderAsync(string path)
        {
            using (var stream = File.Open(path, FileMode.Open))
            {
                var bytes = new byte[stream.Length];
                await stream.ReadAsync(bytes, 0, (int)stream.Length);
                return _device.Logical.CreateShaderModule(new ShaderModuleCreateInfo(bytes));
            }
        }

        private async Task<IDisposable> LoadTextureAsync(string path)
        {
            string contentExtension = Path.GetExtension(path);
            if (contentExtension.Equals("ktx", StringComparison.OrdinalIgnoreCase))
            {
            }
            throw new NotImplementedException();
        }
    }
}

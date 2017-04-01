using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VulkanCore.Samples
{
    public class ContentManager : IDisposable
    {
        private readonly IVulkanAppHost _host;
        private readonly VulkanContext _device;
        private readonly string _contentRoot;
        private readonly Dictionary<string, IDisposable> _cachedContent = new Dictionary<string, IDisposable>();

        public ContentManager(IVulkanAppHost host, VulkanContext device, string contentRoot)
        {
            _host = host;
            _device = device;
            _contentRoot = contentRoot;
        }

        public T Load<T>(string contentName)
        {
            if (_cachedContent.TryGetValue(contentName, out IDisposable value))
                return (T)value;

            string path = Path.Combine(_contentRoot, contentName);
            Type type = typeof(T);
            if (type == typeof(ShaderModule))
            {
                value = LoadShader(path);
            }
            else if (type == typeof(VulkanImage))
            {
                value = LoadTexture(path);
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

        private IDisposable LoadShader(string path)
        {
            const int defaultBufferSize = 4096;
            using (Stream stream = _host.Open(path))
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms, defaultBufferSize);
                return _device.Device.CreateShaderModule(new ShaderModuleCreateInfo(ms.ToArray()));
            }
        }

        private IDisposable LoadTexture(string path)
        {
            string contentExtension = Path.GetExtension(path);
            TextureData textureData;
            if (contentExtension.Equals(".ktx", StringComparison.OrdinalIgnoreCase))
            {
                textureData = ReadKtxTextureData(path);
            }
            else
            {
                throw new NotImplementedException();
            }
            return VulkanImage.Texture2D(_device, textureData);
        }

        private static readonly byte[] KtxIdentifier =
        {
            0xAB, 0x4B, 0x54, 0x58, 0x20, 0x31, 0x31, 0xBB, 0x0D, 0x0A, 0x1A, 0x0A
        };
        private TextureData ReadKtxTextureData(string path)
        {
            using (var reader = new BinaryReader(_host.Open(path)))
            {
                byte[] identifier = reader.ReadBytes(12);

                if (!identifier.SequenceEqual(KtxIdentifier))
                    throw new InvalidOperationException("File is not in Khronos Texture format.");

                int endienness = reader.ReadInt32();
                int glType = reader.ReadInt32();
                int glTypeSize = reader.ReadInt32();
                int glFormat = reader.ReadInt32();
                int glInternalFormat = reader.ReadInt32();
                int glBaseInternalFormat = reader.ReadInt32();
                int pixelWidth = reader.ReadInt32();
                int pixelHeight = reader.ReadInt32();
                int pixelDepth = reader.ReadInt32();
                int numberOfArrayElements = reader.ReadInt32();
                int numberOfFaces = reader.ReadInt32();
                int numberOfMipmapLevels = reader.ReadInt32();
                int bytesOfKeyValueData = reader.ReadInt32();

                // Skip key-value data.
                reader.ReadBytes(bytesOfKeyValueData);

                // Some of the values may be 0 - ensure at least 1.
                pixelWidth = Math.Max(pixelWidth, 1);
                pixelHeight = Math.Max(pixelHeight, 1);
                pixelDepth = Math.Max(pixelDepth, 1);
                numberOfArrayElements = Math.Max(numberOfArrayElements, 1);
                numberOfFaces = Math.Max(numberOfFaces, 1);
                numberOfMipmapLevels = Math.Max(numberOfMipmapLevels, 1);

                int numberOfSlices = Math.Max(numberOfFaces, numberOfArrayElements);

                var data = new TextureData
                {
                    Mipmaps = new TextureData.Mipmap[numberOfMipmapLevels],
                    Format = Format.R5G5B5A1UNormPack16
                };

                for (int i = 0; i < numberOfMipmapLevels; i++)
                {
                    var mipmap = new TextureData.Mipmap();
                    mipmap.Size = reader.ReadInt32();
                    mipmap.Extent = new Extent3D(pixelWidth, pixelHeight, pixelDepth);
                    mipmap.Data = reader.ReadBytes(mipmap.Size);
                    data.Mipmaps[i] = mipmap;
                    break; // TODO: impl
                    //for (int j = 0; j < numberOfArrayElements; j++)
                    //{
                    //    for (int k = 0; k < numberOfFaces; k++)
                    //    {
                    //        for (int l = 0; l < pixelDepth; l++)
                    //        {
                    //            //for (int row = 0;
                    //            //    row < )
                    //        }
                    //    }
                    //}
                }
                return data;
            }
        }
    }
}

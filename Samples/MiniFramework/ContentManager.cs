using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            const int bufferSize = 4096;
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.Asynchronous))
            {
                var shaderBytecode = new byte[stream.Length];
                var buffer = new byte[bufferSize];
                int bytesRead = 0, totalBytes = 0;
                while((bytesRead = await stream.ReadAsync(buffer, 0, bufferSize).ConfigureAwait(false)) > 0)
                {
                    System.Buffer.BlockCopy(buffer, 0, shaderBytecode, totalBytes, bytesRead);
                    totalBytes += bytesRead;
                }
                return _device.Logical.CreateShaderModule(new ShaderModuleCreateInfo(shaderBytecode));
            }
        }

        private async Task<IDisposable> LoadTextureAsync(string path)
        {
            string contentExtension = Path.GetExtension(path);
            TextureData textureData;
            if (contentExtension.Equals(".ktx", StringComparison.OrdinalIgnoreCase))
            {
                textureData = await ReadKtxTextureDataAsync(path);
            }
            else
            {
                throw new NotImplementedException();
            }
            return new Texture(_device, textureData);
        }

        private static readonly byte[] KtxIdentifier =
        {
            0xAB, 0x4B, 0x54, 0x58, 0x20, 0x31, 0x31, 0xBB, 0x0D, 0x0A, 0x1A, 0x0A
        };
        private async Task<TextureData> ReadKtxTextureDataAsync(string path)
        {
            using (var reader = new BinaryReader(File.OpenRead(path)))
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

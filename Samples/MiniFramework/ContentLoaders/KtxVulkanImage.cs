using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VulkanCore.Samples.ContentLoaders
{
    internal static partial class Loader
    {
        // Ktx spec: https://www.khronos.org/opengles/sdk/tools/KTX/file_format_spec/

        private static readonly byte[] KtxIdentifier =
        {
            0xAB, 0x4B, 0x54, 0x58, 0x20, 0x31, 0x31, 0xBB, 0x0D, 0x0A, 0x1A, 0x0A
        };

        // OpenGL internal color formats are described in table 8.12 at
        // https://khronos.org/registry/OpenGL/specs/gl/glspec44.core.pdf
        private static readonly Dictionary<int, Format> _glInternalFormatToVkFormat = new Dictionary<int, Format>()
        {
            [32855] = Format.R5G5B5A1UNormPack16,
            [32856] = Format.R8G8B8A8UNorm
        };

        public static VulkanImage LoadKtxVulkanImage(IVulkanAppHost host, VulkanContext ctx, string path)
        {
            using (var reader = new BinaryReader(host.Open(path)))
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

                if (!_glInternalFormatToVkFormat.TryGetValue(glInternalFormat, out Format format))
                    throw new NotImplementedException("glInternalFormat not mapped to VkFormat.");

                var data = new TextureData
                {
                    Mipmaps = new TextureData.Mipmap[numberOfMipmapLevels],
                    Format = format
                };

                for (int i = 0; i < numberOfMipmapLevels; i++)
                {
                    var mipmap = new TextureData.Mipmap
                    {
                        Size = reader.ReadInt32(),
                        Extent = new Extent3D(pixelWidth, pixelHeight, pixelDepth)
                    };
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

                return VulkanImage.Texture2D(ctx, data);
            }
        }
    }
}

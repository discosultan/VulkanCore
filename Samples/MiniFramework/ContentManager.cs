using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace VulkanCore.Samples
{
    public class ContentManager : IDisposable
    {
        private readonly string _contentRoot;

        public ContentManager(string contentRoot)
        {
            _contentRoot = contentRoot;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        //public Task<T> LoadAsync<T>(string contentName)
        //{
        //    string path = Path.Combine(_contentRoot, contentName);
        //    string contentExtension = Path.GetExtension(contentName);
        //    if (contentExtension.Equals("ktx", StringComparison.OrdinalIgnoreCase))
        //    {
        //        return 
        //    }
        //}

        //private 
    }
}

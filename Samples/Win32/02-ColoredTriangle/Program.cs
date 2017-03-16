using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace VulkanCore.Samples.Triangle
{
    public class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main() => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {
            using (var triangleApp = new TriangleApp(
                Process.GetCurrentProcess().Handle, 
                new Win32Window(Assembly.GetExecutingAssembly().GetName().Name)))
            {
                await triangleApp.InitializeAsync();
                triangleApp.Run();
            }
        }
    }
}

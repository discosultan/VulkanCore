using System;
using System.Reflection;
using System.Threading.Tasks;

namespace VulkanCore.Samples.ColoredTriangle
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
            using (var window = new Win32Window(
                Assembly.GetExecutingAssembly().GetName().Name,
                new ColoredTriangleApp()))
            {
                window.Initialize();
                window.Run();
            }
        }
    }
}

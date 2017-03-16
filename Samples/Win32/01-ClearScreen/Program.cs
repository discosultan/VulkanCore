using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace VulkanCore.Samples.ClearScreen
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main() => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {
            using (var clearScreenApp = new ClearScreenApp(
                Process.GetCurrentProcess().Handle,
                new Win32Window(Assembly.GetExecutingAssembly().GetName().Name)))
            {
                await clearScreenApp.InitializeAsync();
                clearScreenApp.Run();
            }
        }
    }
}

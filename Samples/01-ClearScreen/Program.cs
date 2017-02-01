using System;
using System.Diagnostics;
using System.Reflection;

namespace VulkanCore.Samples.ClearScreen
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            using (var clearScreenApp = new ClearScreenApp(
                Process.GetCurrentProcess().Handle,
                new Win32Window(Assembly.GetExecutingAssembly().GetName().Name)))
            {
                clearScreenApp.Initialize();
                clearScreenApp.Run();
            }
        }
    }
}

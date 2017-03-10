using System;
using System.Diagnostics;
using System.Reflection;

namespace VulkanCore.Samples.Triangle
{
    public class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            using (var triangleApp = new TriangleApp(
                Process.GetCurrentProcess().Handle, 
                new Win32Window(Assembly.GetExecutingAssembly().GetName().Name)))
            {
                triangleApp.Initialize();
                triangleApp.Run();
            }
        }
    }
}

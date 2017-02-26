using System;
using System.Diagnostics;
using System.Reflection;

namespace VulkanCore.Samples.Cube
{
    public class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            using (var cubeApp = new CubeApp(
                Process.GetCurrentProcess().Handle, 
                new Win32Window(Assembly.GetExecutingAssembly().GetName().Name)))
            {
                cubeApp.Initialize();
                cubeApp.Run();
            }
        }
    }
}

using System;
using System.Reflection;

namespace VulkanCore.Samples.ClearScreen
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            using (var window = new Win32Window(
                Assembly.GetExecutingAssembly().GetName().Name,
                new ClearScreenApp()))
            {
                window.Initialize();
                window.Run();
            }
        }
    }
}

using System;
using System.Reflection;

namespace VulkanCore.Samples.ComputeParticles
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            using (var window = new MacOSWindow(
                Assembly.GetExecutingAssembly().GetName().Name,
                new ComputeParticlesApp()))
            {
                window.Initialize();
                window.Run();
            }
        }
    }
}

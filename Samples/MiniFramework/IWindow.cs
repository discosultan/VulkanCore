using System;

namespace VulkanCore.Samples
{
    public enum Platform
    {
        Android, Win32
    }

    public interface IWindow
    {
        IntPtr Handle { get; }
        int Width { get; }
        int Height { get; }
        Platform Platform { get; }

        void Initialize(Action onResized);
        void Run(Action<Timer> tick);
    }
}

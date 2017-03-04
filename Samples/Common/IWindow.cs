using System;

namespace VulkanCore.Samples
{
    public interface IWindow
    {
        IntPtr Handle { get; }
        int Width { get; }
        int Height { get; }

        void Initialize(Action onResized);
        void Run(Action<Timer> tick);
    }
}

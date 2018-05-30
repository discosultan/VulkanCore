using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace VulkanCore.Samples
{
    public class MacOSWindow : IVulkanAppHost
    {
        private readonly string _title;
        private readonly Timer _timer = new Timer();
        private readonly VulkanApp _app;

        private NativeMacOS.NativeApp _nativeApp;
        private NativeMacOS.NativeWindow _nativeWindow;
        private NativeMacOS.NativeMetalView _nativeMetalView;

        private bool _appPaused;
        private bool _running;

        private int _frameCount;
        private float _timeElapsed;

        public MacOSWindow(string title, VulkanApp app)
        {
            _title = title;
            _app = app;
        }

        public IntPtr WindowHandle => _nativeMetalView.NativeMetalViewPointer;
        public IntPtr InstanceHandle => Process.GetCurrentProcess().Handle;
        public int Width { get; private set; } = 1280;
        public int Height { get; private set; } = 720;
        public Platform Platform => Platform.MacOS;

        public Stream Open(string path) => new FileStream(Path.Combine("bin", path), FileMode.Open, FileAccess.Read);

        public void Initialize()
        {
            _nativeApp = new NativeMacOS.NativeApp();
            _nativeWindow = new NativeMacOS.NativeWindow(_nativeApp, new NativeMacOS.Size(Width, Height));
            _nativeWindow.MinSize = new NativeMacOS.Size(200f, 200f);
            _nativeWindow.Title = _title;
            _nativeWindow.BeginResizing += () =>
            {
                _appPaused = true;
                _timer.Stop();
            };
            _nativeWindow.EndResizing += () =>
            {
                _appPaused = false;
                _timer.Start();
                _app.Resize();
            };
            _nativeWindow.Resized += size =>
            {
                Width = (int)size.Width;
                Height = (int)size.Height;
            };
            _nativeWindow.CloseRequested += () =>
            {
                _running = false;
            };
            _nativeMetalView = new NativeMacOS.NativeMetalView(_nativeWindow);

            _app.Initialize(this);
            _running = true;
            _timer.Start();
        }

        public void Run()
        {
            _running = true;
            _timer.Reset();
            while (_running)
            {
                _nativeApp.ProcessEvents();
                _timer.Tick();
                if (!_appPaused)
                {
                    CalculateFrameRateStats();
                    _app.Tick(_timer);
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }

        public void Dispose()
        {
            _app.Dispose();
            _nativeWindow.Dispose();
            _nativeApp.Dispose();
        }

        private void CalculateFrameRateStats()
        {
            _frameCount++;

            if (_timer.TotalTime - _timeElapsed >= 1.0f)
            {
                float fps = _frameCount;
                float mspf = 1000.0f / fps;

                _nativeWindow.Title = $"{_title}    Fps: {fps}    Mspf: {mspf}";

                // Reset for next average.
                _frameCount = 0;
                _timeElapsed += 1.0f;
            }
        }
    }
}

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace VulkanCore.Samples
{
    public class Win32Window : IVulkanAppHost
    {
        private readonly string _title;
        private readonly Timer _timer = new Timer();
        private readonly VulkanApp _app;
        private Form _form;

        private bool _appPaused; // Is the application paused?
        private bool _minimized; // Is the application minimized?
        private bool _maximized; // Is the application maximized?
        private bool _resizing;  // Are the resize bars being dragged?
        private bool _running;   // Is the application running?
        private FormWindowState _lastWindowState = FormWindowState.Normal;

        private int _frameCount;
        private float _timeElapsed;

        public Win32Window(string title, VulkanApp app)
        {
            _title = title;
            _app = app;
        }

        public IntPtr WindowHandle => _form.Handle;
        public IntPtr InstanceHandle => Process.GetCurrentProcess().Handle;
        public int Width { get; private set; } = 1280;
        public int Height { get; private set; } = 720;
        public Platform Platform => Platform.Win32;

        public Stream Open(string path) => new FileStream(path, FileMode.Open, FileAccess.Read);

        public void Initialize()
        {
            _form = new Form
            {
                Text = _title,
                FormBorderStyle = FormBorderStyle.Sizable,
                ClientSize = new System.Drawing.Size(Width, Height),
                StartPosition = FormStartPosition.CenterScreen,
                MinimumSize = new System.Drawing.Size(200, 200),
                Visible = false
            };
            _form.ResizeBegin += (sender, e) =>
            {
                _appPaused = true;
                _resizing = true;
                _timer.Stop();
            };
            _form.ResizeEnd += (sender, e) =>
            {
                _appPaused = false;
                _resizing = false;
                _timer.Start();
                _app.Resize();
            };
            _form.Activated += (sender, e) =>
            {
                _appPaused = _form.WindowState == FormWindowState.Minimized;
                _timer.Start();
            };
            _form.Deactivate += (sender, e) =>
            {
                _appPaused = true;
                _timer.Stop();
            };
            _form.HandleDestroyed += (sender, e) => _running = false;
            _form.Resize += (sender, e) =>
            {
                Width = _form.ClientSize.Width;
                Height = _form.ClientSize.Height;
                // When window state changes.
                if (_form.WindowState != _lastWindowState)
                {
                    _lastWindowState = _form.WindowState;
                    if (_form.WindowState == FormWindowState.Maximized)
                    {
                        _appPaused = false;
                        _minimized = false;
                        _maximized = true;
                        _app.Resize();
                    }
                    else if (_form.WindowState == FormWindowState.Minimized)
                    {
                        _appPaused = true;
                        _minimized = true;
                        _maximized = false;
                    }
                    else if (_form.WindowState == FormWindowState.Normal)
                    {
                        if (_minimized) // Restoring from minimized state?
                        {
                            _appPaused = false;
                            _minimized = false;
                            _app.Resize();
                        }
                        else if (_maximized) // Restoring from maximized state?
                        {
                            _appPaused = false;
                            _maximized = false;
                            _app.Resize();
                        }
                        else if (_resizing)
                        {
                            // If user is dragging the resize bars, we do not resize 
                            // the buffers here because as the user continuously 
                            // drags the resize bars, a stream of WM_SIZE messages are
                            // sent to the window, and it would be pointless (and slow)
                            // to resize for each WM_SIZE message received from dragging
                            // the resize bars. So instead, we reset after the user is 
                            // done resizing the window and releases the resize bars, which 
                            // sends a WM_EXITSIZEMOVE message.
                        }
                        else // API call such as SetWindowPos or setting fullscreen state.
                        {
                            _app.Resize();
                        }
                    }
                }
                else if (!_resizing) // Resize due to snapping.
                {
                    _app.Resize();
                }
            };
            _app.Initialize(this);
        }

        public void Run()
        {
            _running = true;
            _form.Show();
            _form.Update();
            _timer.Reset();
            while (_running)
            {
                Application.DoEvents();
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
        }

        private void CalculateFrameRateStats()
        {
            _frameCount++;

            if (_timer.TotalTime - _timeElapsed >= 1.0f)
            {
                float fps = _frameCount;
                float mspf = 1000.0f / fps;

                _form.Text = $"{_title}    Fps: {fps}    Mspf: {mspf}";

                // Reset for next average.
                _frameCount = 0;
                _timeElapsed += 1.0f;
            }
        }
    }
}

using System;
using System.Threading;
using System.Windows.Forms;

namespace VulkanCore.Samples
{
    public class Win32Window : IWindow
    {
        private readonly string _title;
        private readonly Timer _timer = new Timer();
        private Form _form;

        private bool _appPaused;            // Is the application paused?
        private bool _minimized;            // Is the application minimized?
        private bool _maximized;            // Is the application maximized?
        private bool _resizing;             // Are the resize bars being dragged?
        private bool _running;              // Is the application running?
        private FormWindowState _lastWindowState = FormWindowState.Normal;

        private int _frameCount;
        private float _timeElapsed;

        public Win32Window(string title)
        {
            _title = title;
        }

        public IntPtr Handle => _form.Handle;
        public int Width { get; private set; } = 1280;
        public int Height { get; private set; } = 720;

        public void Initialize(Action onResized)
        {
            _form = new Form
            {
                Text = _title,
                FormBorderStyle = FormBorderStyle.Sizable,
                ClientSize = new System.Drawing.Size(Width, Height),
                StartPosition = FormStartPosition.CenterScreen,
                MinimumSize = new System.Drawing.Size(200, 200)
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
                onResized();
            };
            _form.Activated += (sender, e) =>
            {
                _appPaused = false;
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
                        onResized();
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
                            onResized();
                        }
                        else if (_maximized) // Restoring from maximized state?
                        {
                            _appPaused = false;
                            _maximized = false;
                            onResized();
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
                            onResized();
                        }
                    }
                }
                else if (!_resizing) // Resize due to snapping.
                {
                    onResized();
                }
            };
        }

        public void Run(Action<Timer> tick)
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
                    tick(_timer);
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
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

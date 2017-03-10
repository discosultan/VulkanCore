using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using System.Threading;

namespace VulkanCore.Samples
{
    public class AndroidWindow : View, IWindow
    {
        private readonly Timer _timer = new Timer();

        private bool _appPaused; // Is the application paused?
        private bool _running;   // Is the application running?

        private int _frameCount;
        private float _timeElapsed;

        public Platform Platform => Platform.Android;

        public AndroidWindow(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }
        public AndroidWindow(Context context) : base(context) { }
        public AndroidWindow(Context context, IAttributeSet attrs) : base(context, attrs) { }
        public AndroidWindow(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }
        public AndroidWindow(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes) { }

        public void Initialize(Action onResized)
        {
            //throw new NotImplementedException();

            //this.
        }

        public void Run(Action<Timer> tick)
        {
            _running = true;
            _timer.Reset();
            //while (_running)
            //{
            //    _timer.Tick();
            //    if (!_appPaused)
            //    {
            //        CalculateFrameRateStats();
            //        tick(_timer);
            //    }
            //    else
            //    {
            //        Thread.Sleep(100);
            //    }
            //}
        }

        private void CalculateFrameRateStats()
        {
            _frameCount++;

            if (_timer.TotalTime - _timeElapsed >= 1.0f)
            {
                float fps = _frameCount;
                float mspf = 1000.0f / fps;

                // TODO: show FPS somewhere

                // Reset for next average.
                _frameCount = 0;
                _timeElapsed += 1.0f;
            }
        }
    }
}

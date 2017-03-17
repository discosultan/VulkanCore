using System;
using Android.Content;
using Android.Runtime;
using Android.Views;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;

namespace VulkanCore.Samples
{
    public class VulkanSurfaceView : SurfaceView, ISurfaceHolderCallback, IVulkanAppHost
    {
        private static readonly int _renderDueTime = (int)Math.Ceiling(1000.0f / 60.0f);

        private readonly Timer _timer = new Timer();
        private System.Threading.Timer _animationTimer;
        private readonly VulkanApp _app;

        public IntPtr WindowHandle { get; private set; }
        public IntPtr InstanceHandle => Handle;

        public VulkanSurfaceView(Context ctx, VulkanApp app) : base(ctx)
        {
            _app = app;
            Holder.AddCallback(this);
            SetWillNotDraw(false);
        }

        public Platform Platform => Platform.Android;

        public Stream Open(string path) => Context.Assets.Open(path);


        #region ISurfaceHolderCallback implementation

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            WindowHandle = ANativeWindow_fromSurface(JNIEnv.Handle, holder.Surface.Handle);

            _app.Initialize(this);
            _app.Resize();

            _timer.Start();
            _animationTimer = new System.Threading.Timer(RequestAnimationFrameCallback, null, _renderDueTime, Timeout.Infinite);
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Android.Graphics.Format format, int width, int height)
        {
            _app.Resize();
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            _timer.Stop();
            _animationTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _animationTimer.Dispose();
            _app.Dispose();
            ANativeWindow_release(WindowHandle);
        }

        #endregion

        private void RequestAnimationFrameCallback(object state)
        {
            try
            {
                Android.App.Application.SynchronizationContext.Send(Tick, state);
            }
            finally
            {
                _animationTimer.Change(_renderDueTime, Timeout.Infinite);
            }
        }

        private void Tick(object state)
        {
            _timer.Tick();
            _app.Tick(_timer);
        }

        [DllImport("android")]
        private static extern IntPtr ANativeWindow_fromSurface(IntPtr jni, IntPtr surface);

        [DllImport("android")]
        private static extern void ANativeWindow_release(IntPtr surface);
    }
}

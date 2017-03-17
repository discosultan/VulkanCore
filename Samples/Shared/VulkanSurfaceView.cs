using System;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Graphics;
using System.Runtime.InteropServices;
using System.IO;

namespace VulkanCore.Samples
{
    public class VulkanSurfaceView : SurfaceView, ISurfaceHolderCallback, IVulkanAppHost
    {
        private readonly Timer _timer = new Timer();
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

        public Stream Load(string path) => Context.Assets.Open(path);


        #region ISurfaceHolderCallback implementation

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            if (WindowHandle == IntPtr.Zero)
                WindowHandle = ANativeWindow_fromSurface(JNIEnv.Handle, holder.Surface.Handle);

            if (!_app.Initialized)
                _app.Initialize(this);
            else
                _app.Resize();

            _timer.Start();
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Android.Graphics.Format format, int width, int height)
        {
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            _timer.Stop();
        }

        #endregion


        protected override void OnDraw(Canvas canvas)
        {
            _timer.Tick();
            _app.Tick(_timer);
        }

        protected override void Dispose(bool disposing)
        {
            if (WindowHandle != IntPtr.Zero)
                ANativeWindow_release(WindowHandle);
            base.Dispose(disposing);
        }

        [DllImport("android")]
        private static extern IntPtr ANativeWindow_fromSurface(IntPtr jni, IntPtr surface);

        [DllImport("android")]
        private static extern void ANativeWindow_release(IntPtr surface);
    }
}

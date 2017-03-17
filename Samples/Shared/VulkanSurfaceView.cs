using System;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Graphics;
using System.Runtime.InteropServices;

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

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Android.Graphics.Format format, int width, int height)
        {
            _app.ResizeAsync().Wait();
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            WindowHandle = ANativeWindow_fromSurface(JNIEnv.Handle, holder.Surface.Handle);
            _app.InitializeAsync(this).Wait();
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            _app.Dispose();
            ANativeWindow_release(WindowHandle);
        }

        protected override void OnDraw(Canvas canvas)
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

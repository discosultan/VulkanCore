using Android.Content;
using Android.Runtime;
using Android.Views;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using AndroidApplication = Android.App.Application;
using AndroidFormat = Android.Graphics.Format;

namespace VulkanCore.Samples
{
    public class VulkanSurfaceView : SurfaceView, ISurfaceHolderCallback, IVulkanAppHost
    {
        // Threading timer for render loop.
        private static readonly int _renderDueTime = (int)Math.Ceiling(1000.0f / 60.0f);
        private System.Threading.Timer _tickTimer;
        private bool _appPaused;

        // Game timer used by samples for elapsed/total duration.
        private readonly Timer _gameTimer = new Timer();

        private VulkanApp _app;

        public VulkanSurfaceView(Context ctx, VulkanApp app) : base(ctx)
        {
            Initialize(app);
        }

        public IntPtr WindowHandle { get; private set; }
        public IntPtr InstanceHandle => Handle;
        public Platform Platform => Platform.Android;

        public Stream Open(string path) => Context.Assets.Open(path);

        private void Initialize(VulkanApp app)
        {
            _app = app;
            Holder.AddCallback(this);
        }

        private void Tick()
        {
            _gameTimer.Tick();
            _app.Tick(_gameTimer);
        }

        #region ISurfaceHolderCallback implementation

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            WindowHandle = ANativeWindow_fromSurface(JNIEnv.Handle, holder.Surface.Handle);

            _appPaused = false;
            _app.Initialize(this);

            _gameTimer.Start();
            _tickTimer = new System.Threading.Timer(state =>
            {
                AndroidApplication.SynchronizationContext.Send(_ => { if (!_appPaused) Tick(); }, state);
                _tickTimer.Change(_renderDueTime, Timeout.Infinite);
            }, null, _renderDueTime, Timeout.Infinite);
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] AndroidFormat format, int width, int height)
        {
            _app.Resize();
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            _gameTimer.Stop();
            _tickTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _tickTimer.Dispose();
            _app.Dispose();
            ANativeWindow_release(WindowHandle);
            _appPaused = true;
        }

        #endregion

        [DllImport("android")]
        private static extern IntPtr ANativeWindow_fromSurface(IntPtr jni, IntPtr surface);

        [DllImport("android")]
        private static extern void ANativeWindow_release(IntPtr surface);
    }
}

using Android.App;
using Android.OS;

namespace VulkanCore.Samples.ColoredTriangle
{
    [Activity(Label = "ColoredTriangle", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var surfaceView = new VulkanSurfaceView(this, new ColoredTriangleApp());
            SetContentView(surfaceView);
        }
    }
}


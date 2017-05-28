using Android.App;
using Android.OS;
using Android.Content.PM;

namespace VulkanCore.Samples.TexturedCube
{
    [Activity(
        Label = "TexturedCube",
        ScreenOrientation = ScreenOrientation.SensorPortrait,
        MainLauncher = true,
        Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var surfaceView = new VulkanSurfaceView(this, new TexturedCubeApp());
            SetContentView(surfaceView);
        }
    }
}


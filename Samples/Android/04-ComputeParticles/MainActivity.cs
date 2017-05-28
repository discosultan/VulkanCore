using Android.App;
using Android.Content.PM;
using Android.OS;

namespace VulkanCore.Samples.ComputeParticles
{
    [Activity(
        Label = "ComputeParticles",
        ScreenOrientation = ScreenOrientation.SensorPortrait,
        MainLauncher = true,
        Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var surfaceView = new VulkanSurfaceView(this, new ComputeParticlesApp());
            SetContentView(surfaceView);
        }
    }
}


using Android.App;
using Android.Content.PM;
using Android.OS;

namespace VulkanCore.Samples.ClearScreen
{
    [Activity(
        Label = "ClearScreen",
        ScreenOrientation = ScreenOrientation.SensorPortrait,
        MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var surfaceView = new VulkanSurfaceView(this, new ClearScreenApp());
            SetContentView(surfaceView);
        }
    }
}

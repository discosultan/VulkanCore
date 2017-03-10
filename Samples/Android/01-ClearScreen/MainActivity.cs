using Android.App;
using Android.Widget;
using Android.OS;
using Process = System.Diagnostics.Process;

namespace VulkanCore.Samples.ClearScreen
{
    [Activity(Label = "ClearScreen", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var view = new AndroidWindow(ApplicationContext);
            LinearLayout layout = FindViewById<LinearLayout>(Resource.Id.Layout);
            layout.AddView(view);

            using (var clearScreenApp = new ClearScreenApp(
                Process.GetCurrentProcess().Handle,
                view))
            {
                clearScreenApp.Initialize();
                clearScreenApp.Run();
            }
        }
    }
}


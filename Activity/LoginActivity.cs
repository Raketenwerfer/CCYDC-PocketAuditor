using Android.App;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;

namespace PocketAuditor.Fragment
{
    [Activity(Label = "LoginActivity", MainLauncher = true)] 
    public class LoginActivity : AppCompatActivity
    {
        Button login;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.login);

            login = FindViewById<Button>(Resource.Id.login); 

            login.Click += Login_Click;
        }

        private void Login_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(MenuActivity));
        }
    }
}
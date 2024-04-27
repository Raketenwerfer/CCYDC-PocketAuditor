using Android.App;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using MySqlConnector;
using Pocket_Auditor_Admin_Panel.Auxiliaries;
using Pocket_Auditor_Admin_Panel.Classes;
using PocketAuditor.Class;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PocketAuditor.Fragment
{
    [Activity(Label = "Pocket Auditor", MainLauncher = true)] 
    public class LoginActivity : AppCompatActivity
    {
        Button login;
        EditText username;
        EditText password;

        // CCS AP: 172.176.8.101
        // Home: 192.168.254.102
        public DatabaseInitiator dbInit = new DatabaseInitiator("192.168.254.102", "ccydc_database", "root", ";");
        public DataSharingService DSS = new DataSharingService();
        List<mdl_Users> _Users = new List<mdl_Users>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Create your application here
            PullUsers();

            DSS = DataSharingService.GetInstance();
            DSS.SetDatabase(dbInit);
            DSS.SET_U(_Users);
            SetContentView(Resource.Layout.login);

            login = FindViewById<Button>(Resource.Id.login);
            username = FindViewById<EditText>(Resource.Id.username);
            password = FindViewById<EditText>(Resource.Id.password);

            login.Click += Login_Click;
        }

        private void Login_Click(object sender, EventArgs e)
        {
            DSS.SetAuditor(null, null);
            string encpass = EncryptionService(password.Text);

            foreach (mdl_Users x in _Users)
            {
                if (x.Username == username.Text && x.Password == encpass)
                {
                    DSS.SetAuditor(x.Username, x.UserID);
                    StartActivity(typeof(MenuActivity));
                    Toast.MakeText(this, "Logged in successfully!", ToastLength.Short).Show();
                    break;
                }
                else
                {
                    Toast.MakeText(this, "Username or password is incorrect!", ToastLength.Short).Show();
                }
            }
        }


        public void PullUsers()
        {
            _Users.Clear();

            string query = "SELECT * FROM users";

            MySqlConnection conn = dbInit.GetConnection();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader read = cmd.ExecuteReader())
                    {
                        while (read.Read())
                        {
                            int id = read.GetInt32(read.GetOrdinal("UserID"));
                            string username = read.GetString(read.GetOrdinal("Username"));
                            string password = read.GetString(read.GetOrdinal("Password"));
                            string type = read.GetString(read.GetOrdinal("UserType"));
                            string status = read.GetString(read.GetOrdinal("UserStatus"));

                            mdl_Users a = new mdl_Users(id, username, password, type, status);
                            _Users.Add(a);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
            finally
            {
                DSS.SET_U(_Users);
                conn.Close();
            }
        }
        public static string EncryptionService(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider class.
            using (MD5 md5 = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to a hexadecimal string.
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
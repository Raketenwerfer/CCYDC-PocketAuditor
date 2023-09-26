using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using AndroidX.DrawerLayout.Widget;
using SQLite;
using PocketAuditor.Database;
using PocketAuditor.Class;
using Android.Database;
using Android.Database.Sqlite;
using System;
using System.Collections.Generic;
using PocketAuditor.Activity;
using Android.Content;

namespace PocketAuditor.Fragment
{
    [Activity(Label = "ManageActivity")]
    public class ManageActivity : AppCompatActivity/*, NavigationView.IOnNavigationItemSelectedListener*/
    {
        private DrawerLayout drawerLayout;
        private ImageView openham, addCategory, backMenu;

        public DB_Initiator handler;
        public SQLiteDatabase SQLDB;

        private NavigationView navView;

        private List<CategoryModel> _Categories = new List<CategoryModel>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.activity_drawer);

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_Layout);
            ActionBarDrawerToggle toogle = new ActionBarDrawerToggle(this, drawerLayout, Resource.String.nav_close, Resource.String.nav_open);
            drawerLayout.AddDrawerListener(toogle);
            toogle.SyncState();

            openham = FindViewById<ImageView>(Resource.Id.hamburger);
            openham.Click += Openham_Click;

            addCategory = FindViewById<ImageView>(Resource.Id.addImage);
            addCategory.Click += AddCategory_Click;

            backMenu = FindViewById<ImageView>(Resource.Id.returnMenu);
            backMenu.Click += BackMenu_Click; 

            handler = new DB_Initiator(this);
            SQLDB = handler.WritableDatabase;

            PullCategories();

            navView = FindViewById<NavigationView>(Resource.Id.nav_view);
            //navView.SetNavigationItemSelectedListener(this);

            InitializeNavView(_Categories);

            ////Enable the toolbar navigation icon to open the navigation drawer
            //SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
            //SupportActionBar.SetDefaultDisplayHomeAsUpEnabled(true);
            //SupportActionBar.SetHomeButtonEnabled(true);

            navView.NavigationItemSelected += (sender, e) =>
            {
                IMenuItem selItem = e.MenuItem;

                // This section handles click events. It will read the item title
                // then passes the title name to another method to work off with it

                if (selItem != null)
                {
                    string __selection = selItem.TitleFormatted.ToString();

                    SelectionEvent(__selection);
                }


                DrawerLayout dL = FindViewById<DrawerLayout>(Resource.Id.drawer_Layout);
                dL.CloseDrawer(GravityCompat.Start);
            };
        }

        private void BackMenu_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MenuActivity));
            StartActivity(intent);
        }

        private void AddCategory_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(AddCategoryActivity));
            StartActivity(intent);
        }

        private void Openham_Click(object sender, EventArgs e)
        {
            if (!drawerLayout.IsDrawerOpen(GravityCompat.Start))
            {
                drawerLayout.OpenDrawer(GravityCompat.Start);
            } 
        }

        public void PullCategories()
        {
            int q_CatID;
            string q_CatTitle = null;
            string catQuery = "SELECT * FROM Category_tbl";

            ICursor cList = SQLDB.RawQuery(catQuery, new string[] { });

            if (cList.Count > 0)
            {
                cList.MoveToFirst();

                do
                {
                    q_CatID = cList.GetInt(cList.GetColumnIndex("Category_ID"));
                    q_CatTitle = cList.GetString(cList.GetColumnIndex("CategoryTitle"));

                    CategoryModel a = new CategoryModel(q_CatID, q_CatTitle);

                    _Categories.Add(a);
                }
                while (cList.MoveToNext());

                cList.Close();
            }
        }

        void InitializeNavView(List<CategoryModel> x01i)
        {
            navView.Menu.Clear();

            foreach (CategoryModel catSets in x01i)
            {
                IMenuItem menuItem = navView.Menu.Add(catSets.CategoryTitle);
                menuItem.SetCheckable(true);
                menuItem.SetChecked(false);
            }
        }
        
        void SelectionEvent (string z01)
        {
            Toast.MakeText(ApplicationContext, "Selected Item: " + z01, ToastLength.Short).Show();
        }

        // This method will refresh the menu items. Call this when we start implementing
        // methods to add new categories
        void RefreshNavView(List<CategoryModel> x02r)
        {
            _Categories = x02r;

            InitializeNavView(x02r);
        }
    }
}
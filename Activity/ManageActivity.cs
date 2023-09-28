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
using Android.Content;

namespace PocketAuditor.Fragment
{
    [Activity(Label = "ManageActivity")]
    public class ManageActivity : AppCompatActivity/*, NavigationView.IOnNavigationItemSelectedListener*/
    {
        private readonly EditText CateName_eT;
        private DrawerLayout drawerLayout;
        private ImageView openham, addCategory, backMenu;

        public DB_Initiator handler;
        public SQLiteDatabase SQLDB;

        private NavigationView navView;

        private List<CategoryModel> _Categories = new List<CategoryModel>();
        private readonly object alertdialogBuilder;

        readonly string get_sequence = "SELECT COUNT(*) FROM Category_tbl";
        int sequence;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.activity_drawer);

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_Layout);
            //ActionBarDrawerToggle toogle = new ActionBarDrawerToggle(this, drawerLayout, Resource.String.nav_close, Resource.String.nav_open);
            //drawerLayout.AddDrawerListener(toogle);
            //toogle.SyncState();

            openham = FindViewById<ImageView>(Resource.Id.hamburger);
            openham.Click += Openham_Click;

            addCategory = FindViewById<ImageView>(Resource.Id.addCategory);
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

        private void GetRowSequenceCount()
        {
            ICursor gseq = SQLDB.RawQuery(get_sequence, new string[] { });

            //if (gseq.MoveToFirst())
            //{
            //    sequence = gseq.GetInt(0); // Use index 0 to get the count
            //}
            sequence = gseq.GetInt(gseq.GetColumnIndex("COLUMN(*)")); //error
        }

        private void BackMenu_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MenuActivity));
            StartActivity(intent);
        }

        private void AddCategory_Click(object sender, EventArgs e)
        {
            LayoutInflater layoutInflater = LayoutInflater.FromContext(this);
            View mView = layoutInflater.Inflate(Resource.Layout.add_category, null);

            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
            builder.SetView(mView);

            var userContent = mView.FindViewById<EditText>(Resource.Id.ACName_eT);
            builder.SetCancelable(false)

                .SetPositiveButton("Save", delegate
                {
                    GetRowSequenceCount();
                    sequence++;

                    var _db = new SQLiteConnection(handler._ConnPath);

                    _db.Execute("INSERT INTO Category_tbl(Category_ID, CategoryTitle, CategoryStatus)" +
                                    "VALUES (?, ?, ?)", sequence, userContent.Text, "ACTIVE"); // error

                    Toast.MakeText(Application.Context, "New Category Inserted", ToastLength.Short).Show();

                    _db.Commit();
                })

                .SetNegativeButton("Cancel", delegate
                {
                    builder.Dispose();
                });

            Android.App.AlertDialog alertAddDialog = builder.Create();
            alertAddDialog.Show();

            //Intent intent = new Intent(this, typeof(AddCategoryActivity));
            //StartActivity(intent);
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
            string catQuery = "SELECT * FROM Category_tbl";

            ICursor cList = SQLDB.RawQuery(catQuery, new string[] { });

            if (cList.Count > 0)
            {
                cList.MoveToFirst();

                do
                {
                    q_CatID = cList.GetInt(cList.GetColumnIndex("Category_ID"));
                    string q_CatTitle = cList.GetString(cList.GetColumnIndex("CategoryTitle"));

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
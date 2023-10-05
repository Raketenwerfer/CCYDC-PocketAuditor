using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
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
using AndroidX.Core.View;

namespace PocketAuditor.Fragment
{
    [Activity(Label = "ManageCQ")]
    public class ManageCQ : AppCompatActivity
    {
        private readonly EditText CateName_eT;
        private DrawerLayout drawerLayout;
        TextView TxtDisCate;

        private ImageView openham, addCategory, backMenu, editCategory;

        public DB_Initiator handler;
        public SQLiteDatabase SQLDB;

        private NavigationView navView;

        private List<CategoryModel> _Categories = new List<CategoryModel>();
        private readonly object alertdialogBuilder;

        readonly string get_sequence = "SELECT COUNT(*) FROM Category_tbl";
        int sequence;

        private string selectedCategoryName;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.activity_drawer);

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_Layout);
            
            TxtDisCate = FindViewById<TextView>(Resource.Id.txtDC);

            openham = FindViewById<ImageView>(Resource.Id.hamburger);
            openham.Click += Openham_Click;

            addCategory = FindViewById<ImageView>(Resource.Id.addCategory);
            addCategory.Click += AddCategory_Click;

            backMenu = FindViewById<ImageView>(Resource.Id.returnMenu);
            backMenu.Click += BackMenu_Click;

            editCategory = FindViewById<ImageView>(Resource.Id.editCat);
            editCategory.Click += EditCategoryName;

            handler = new DB_Initiator(this);
            SQLDB = handler.WritableDatabase;

            PullCategories();

            navView = FindViewById<NavigationView>(Resource.Id.nav_view);

            InitializeNavView(_Categories); 

            navView.NavigationItemSelected += (sender, e) =>
            {
                IMenuItem selItem = e.MenuItem;

                // This section handles click events. It will read the item title
                // then passes the title name to another method to work off with it

                if (selItem != null)
                {
                    /*string __selection*/ 
                    selectedCategoryName = selItem.TitleFormatted.ToString(); //new
                    TxtDisCate.Text = selectedCategoryName;

                    Toast.MakeText(ApplicationContext,  selectedCategoryName, ToastLength.Short).Show();
                }

                DrawerLayout dL = FindViewById<DrawerLayout>(Resource.Id.drawer_Layout);
                dL.CloseDrawer(GravityCompat.Start);
            };
        }

        private void EditCategoryName(object sender, EventArgs e)
        {
            LayoutInflater inflater = LayoutInflater.FromContext(this);
            View mView = inflater.Inflate(Resource.Layout.edit_category, null);

            Android.App.AlertDialog.Builder build = new Android.App.AlertDialog.Builder(this);
            build.SetView(mView);

            var content = mView.FindViewById<EditText>(Resource.Id.ECName_eT);
            content.Text = selectedCategoryName; //Set the editText text to the selected category name 4 editing

            build.SetCancelable(false)

                .SetPositiveButton("Update", delegate
                {
                    //Retrieve the edited category Name from the EditText
                    string updatedCategoryName = content.Text;

                    // Perform the update operation with updatedCategoryName
                    // Update the category in your database or wherever it's stored

                })
                .SetNegativeButton("Discard", delegate
                {
                    build.Dispose();
                });

            Android.App.AlertDialog alertEditDialog = build.Create();
            alertEditDialog.Show();
        }

        private void GetRowSequenceCount()
        {
            ICursor gseq = SQLDB.RawQuery(get_sequence, new string[] { });

            // Ma save sa application pero dli sa db hahahaha.
            if (gseq.MoveToFirst())
            {
                sequence = gseq.GetInt(0); // Use index 0 to get the count
            }
            else
            {
                sequence = 0;
            }

            gseq.Close();
            //sequence = gseq.GetInt(gseq.GetColumnIndex("COLUMN(*)")); //error
        }

        private void BackMenu_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(ManageMenu));
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

                .SetPositiveButton("Add", delegate
                {
                    string categoryTitle = userContent.Text.Trim();

                    if (!string.IsNullOrEmpty(categoryTitle))
                    {
                        GetRowSequenceCount();
                        sequence++;

                        var _db = new SQLiteConnection(handler._ConnPath);

                        _db.Execute("INSERT INTO Category_tbl(Category_ID, CategoryTitle, CategoryStatus)" +
                                        "VALUES (?, ?, ?)", sequence, categoryTitle, "ACTIVE");

                        Toast.MakeText(Application.Context, "New Category Inserted", ToastLength.Short).Show();

                        _db.Commit();

                        RefreshNavView(_Categories);
                        InitializeNavView(_Categories);

                        _db.Close();

                    }
                    else
                    {
                        Toast.MakeText(Application.Context, "Category title cannot be empty", ToastLength.Short).Show();
                    }
                })

                .SetNegativeButton("Cancel", delegate
                {
                    builder.Dispose();
                });

            Android.App.AlertDialog alertAddDialog = builder.Create();
            alertAddDialog.Show();
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
            string catQuery = "SELECT * FROM Category_tbl WHERE CategoryStatus = 'ACTIVE'";

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

        private void EditCategory()
        {
            // Pull string from edit category title pop-up here
            string InsertEditTextHere = null;

            var _db = new SQLiteConnection(handler._ConnPath);

            _db.Execute("UPDATE Category_tbl" +
                        "SET CategoryTitle = ?" +
                        "WHERE ? = CategoryTitle", InsertEditTextHere, InsertEditTextHere);

            Toast.MakeText(Application.Context, "Category Renamed!", ToastLength.Short).Show();

            _db.Commit();

            RefreshNavView(_Categories);
            InitializeNavView(_Categories);

            _db.Close();
        }


        private void DeleteCategory()
        {
            // Pull string from edit category title pop-up here
            string InsertEditTextHere = null;


            var _db = new SQLiteConnection(handler._ConnPath);

            _db.Execute("UPDATE Category_tbl" +
                        "SET CategoryStatus = INACTIVE" +
                        "WHERE ? = CategoryTitle", InsertEditTextHere);

            Toast.MakeText(Application.Context, "Category Renamed!", ToastLength.Short).Show();

            _db.Commit();

            RefreshNavView(_Categories);
            InitializeNavView(_Categories);

            _db.Close();
        }

        private void InitializeNavView(List<CategoryModel> x01i)
        {
            navView.Menu.Clear();

            foreach (CategoryModel catSets in x01i)
            {
                IMenuItem menuItem = navView.Menu.Add(catSets.CategoryTitle);
                menuItem.SetCheckable(true);
                menuItem.SetChecked(false);
            }
        }
        
        private void RefreshNavView(List<CategoryModel> x02r)
        {
            _Categories = x02r;

            InitializeNavView(x02r);
        }
    }
}
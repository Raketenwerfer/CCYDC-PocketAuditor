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
using System.Linq;
using AndroidX.RecyclerView.Widget;
using PocketAuditor.Adapter;

namespace PocketAuditor.Fragment
{
    [Activity(Label = "ManageCQ")]
    public class ManageCQ : AppCompatActivity
    {
        private readonly EditText CateName_eT;
        private DrawerLayout drawerLayout;
        private RecyclerView question_recycler;
        private QuestionAdapter adapter;

        TextView TxtDisCate;

        private ImageView openham, addCategory, backMenu, editCategory, addNewQuestion;

        public DB_Initiator handler;
        public SQLiteDatabase SQLDB;

        private NavigationView navView;

        private List<CategoryModel> _Categories = new List<CategoryModel>();
        private List<QuestionModel> _Entries = new List<QuestionModel>();
        private readonly object alertdialogBuilder;

        readonly string get_sequence = "SELECT COUNT(*) FROM Category_tbl";
        readonly string get_question_seq = "SELECT COUNT(*) FROM Entry_tbl";
        int sequence, q_sequence;

        private string selectedCategoryName;
        private int selectedCategoryID;

        // Establish Database Connection: var _db = new SQLiteConnection(handler._ConnPath);
        // Do Queries: _db.Execute(query_here);
        // Close Connection: _db.Close();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.activity_drawer);

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_Layout);
            question_recycler = FindViewById<RecyclerView>(Resource.Id.Questionrecycler);
            
            TxtDisCate = FindViewById<TextView>(Resource.Id.txtDC);

            openham = FindViewById<ImageView>(Resource.Id.hamburger);
            openham.Click += Openham_Click;

            addCategory = FindViewById<ImageView>(Resource.Id.addCategory);
            addCategory.Click += AddCategory_Click;

            backMenu = FindViewById<ImageView>(Resource.Id.returnMenu);
            backMenu.Click += BackMenu_Click;

            editCategory = FindViewById<ImageView>(Resource.Id.editCat);
            editCategory.Click += EditCategoryName;

            addNewQuestion = FindViewById<ImageView>(Resource.Id.newQuestion);
            addNewQuestion.Click += AddNewQuestion_Click; 

            handler = new DB_Initiator(this);
            SQLDB = handler.WritableDatabase;

            navView = FindViewById<NavigationView>(Resource.Id.nav_view);

            adapter = new QuestionAdapter(_Entries);
            question_recycler.SetAdapter(adapter);


            PullEntries();
            InitializeNavView(_Categories); 

            navView.NavigationItemSelected += (sender, e) =>
            {
                IMenuItem selItem = e.MenuItem;

                // Check for long press on a menu item
                //selItem.ActionView.LongClick += (view, args) =>
                //{
                //    Android.App.AlertDialog.Builder delBuilder = new Android.App.AlertDialog.Builder(this);
                //    delBuilder.SetTitle("Delete Category");
                //    delBuilder.SetMessage("Are you sure you want to delete this category?");
                //    delBuilder.SetPositiveButton("Yes", (s, a) =>
                //    {

                //    });
                //    delBuilder.SetNegativeButton("No", (s, a) =>
                //    {
                //        delBuilder.Dispose();
                //    });
                //    delBuilder.Show();
                //};


                // This section handles click events. It will read the item title
                // then passes the title name to another method to work off with it

                if (selItem != null)
                {
                    selectedCategoryName = selItem.TitleFormatted.ToString(); //new
                    TxtDisCate.Text = selectedCategoryName;

                    PullEntries();
                    Toast.MakeText(ApplicationContext,  selectedCategoryName, ToastLength.Short).Show();
                }

                DrawerLayout dL = FindViewById<DrawerLayout>(Resource.Id.drawer_Layout);
                dL.CloseDrawer(GravityCompat.Start);
            };
        }

        #region Methods for Categories Display and CRUD

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
                    EditCategory(updatedCategoryName);

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

            // Ma save sa application pero dli sa db server hahahaha.
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

                        InitializeNavView(_Categories);

                        navView.Invalidate(); //This is supposed to refresh the NavView

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
            _Categories.Clear();

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

        private void EditCategory(string updatedCategoryName)
        {
            var _db = new SQLiteConnection(handler._ConnPath);

            // Use placeholders and parameters in your SQL query
            _db.Execute("UPDATE Category_tbl " +
                        "SET CategoryTitle = ? " +
                        "WHERE CategoryTitle = ?", updatedCategoryName, selectedCategoryName);

            Toast.MakeText(Application.Context, "Category Renamed!", ToastLength.Short).Show();

            _db.Commit();

            InitializeNavView(_Categories);

            navView.Invalidate(); //This is supposed to refresh the NavView

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

            Toast.MakeText(Application.Context, "Category Deleted", ToastLength.Short).Show();

            _db.Commit();

            InitializeNavView(_Categories);

            _db.Close();
        }

        private void InitializeNavView(List<CategoryModel> x01i)
        {

            PullCategories();

            navView.Menu.Clear();

            foreach (CategoryModel catSets in x01i)
            {
                IMenuItem menuItem = navView.Menu.Add(catSets.CategoryTitle);
                menuItem.SetCheckable(true);
                menuItem.SetChecked(false);
            }
        }

        #endregion


        #region Methods for Questions Display and CRUD

        // Acquire ID Method, in-case for future use
        //
        //private List<string> GetCategoryIdsFromDatabase() //new
        //{
        //    List<string> categoryIds = new List<string>();

        //    using (var handler = new DB_Initiator(this))
        //    {
        //        // Assuming you have a Category table with a column "CategoryID"
        //        SQLiteDatabase db = handler.ReadableDatabase;
        //        string[] projection = { "CategoryID" };

        //        using (ICursor cursor = db.Query("Category", projection, null, null, null, null, null))
        //        {
        //            if (cursor != null)
        //            {
        //                while (cursor.MoveToNext())
        //                {
        //                    int columnIndex = cursor.GetColumnIndex("CategoryID");
        //                    string categoryId = cursor.GetString(columnIndex);
        //                    categoryIds.Add(categoryId);
        //                }
        //                cursor.Close();
        //            }
        //        }
        //    }
        //    return categoryIds;
        //}

        private void AddNewQuestion_Click(object sender, EventArgs e)
        {
            LayoutInflater layoutInflater = LayoutInflater.FromContext(this);
            View mView = layoutInflater.Inflate(Resource.Layout.add_new_question, null);

            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
            builder.SetView(mView);

            var userContent = mView.FindViewById<EditText>(Resource.Id.ANQuestion_eT);

            builder.SetCancelable(false)
                .SetPositiveButton("Add", delegate
                {
                    string newQuestion = userContent.Text;

                    AddQuestionToDatabase(newQuestion);

                })
                .SetNegativeButton("Cancel", delegate
                {
                    builder.Dispose();
                });
            builder.Create().Show();
        }

        private void AddQuestionToDatabase(/*string selectedCategoryId,*/ string newQuestion)
        {
            ContentValues values = new ContentValues();
            //values.Put("CategoryID", selectedCategoryId);
            values.Put("Indicator", newQuestion);

            try
            {
                GetQuestionSequence();
                q_sequence++;

                var _db = new SQLiteConnection(handler._ConnPath);

                // Insert the new question into the database
                _db.Execute("INSERT INTO Entry_tbl(EntryID,  QuesNo, Indicator, ScoreValue, EntryStatus)" +
                    "VALUES (?, ?, ?, ? ,?)", q_sequence,  q_sequence, newQuestion, 1, "ACTIVE");

                if (q_sequence != -1)
                {
                    // The insertion was successful
                    Toast.MakeText(Application.Context, "New Question is Added in the Selected Category", ToastLength.Long).Show();
                    // You can perform any necessary actions here
                }
                else
                {
                    // The insertion failed
                    Toast.MakeText(Application.Context, "Failed to add question", ToastLength.Short).Show();
                    // Handle the error as needed
                }

                _db.Close();
            }
            catch (Exception ex)
            {
                Toast.MakeText(Application.Context, "An error occurred: " + ex.Message, ToastLength.Short).Show();
            }

            PullEntries();
        }

        private void GetQuestionSequence()
        {
            ICursor gseq = SQLDB.RawQuery(get_question_seq, new string[] { });

            // Ma save sa application pero dli sa db hahahaha.
            if (gseq.MoveToFirst())
            {
                q_sequence = gseq.GetInt(0); // Use index 0 to get the count
            }
            else
            {
                q_sequence = 0;
            }

            gseq.Close();
        }

        private void PullEntries()
        {
            _Entries.Clear();

            int q_EntryID, q_CatID, q_QuesNo, q_ScoreValue;
            string q_Indicator, q_Status;
            string entryQuery = "SELECT * FROM Entry_tbl WHERE EntryStatus = 'ACTIVE'";

            ICursor cList = SQLDB.RawQuery(entryQuery, new string[] { });

            if (cList.Count > 0)
            {
                cList.MoveToFirst();

                do
                {
                    q_EntryID = cList.GetInt(cList.GetColumnIndex("EntryID"));
                    q_CatID = cList.GetInt(cList.GetColumnIndex("CategoryID"));
                    q_QuesNo = cList.GetInt(cList.GetColumnIndex("QuesNo"));
                    q_Indicator = cList.GetString(cList.GetColumnIndex("Indicator"));
                    q_ScoreValue = cList.GetInt(cList.GetColumnIndex("ScoreValue"));
                    q_Status = cList.GetString(cList.GetColumnIndex("EntryStatus"));

                    QuestionModel a = new QuestionModel(q_EntryID, q_CatID, q_QuesNo, q_Indicator, q_ScoreValue, q_Status);

                    _Entries.Add(a);
                }
                while (cList.MoveToNext());

                cList.Close();
            }
        }

        // Testing a singular method that handles both edit and delete functions
        private void EditEntry(int updatedQuesNo, string updatedQuesName, int updatedScoreValue, string updatedEntryStatus)
        {
            // reads through every item in _Categories, searches for a match
            // in selectedCategoryModel, then takes that entry's CategoryID

            // ICursor error, cannot find index

            foreach (CategoryModel cm in _Categories)
            {
                try
                {
                    if (selectedCategoryName == cm.CategoryTitle)
                    {
                        selectedCategoryID = cm.CategoryID;
                    }
                }
                catch
                {
                    Toast.MakeText(Application.Context, "No valid category found", ToastLength.Short).Show();
                }
                
            }

            var _db = new SQLiteConnection(handler._ConnPath);

            // Use placeholders and parameters in your SQL query
            _db.Execute("UPDATE Entry_tbl " +
                        "SET QuesNo = ?, " +
                            "Indicator = ?, " +
                            "ScoreValue = ?, " +
                            "EntryStatus = ?" +
                        "WHERE CategoryID = ?", updatedQuesNo, updatedQuesName, updatedScoreValue, updatedEntryStatus, selectedCategoryID);

            Toast.MakeText(Application.Context, "Indicator Entry Updated!", ToastLength.Short).Show();

            _db.Commit();

            _db.Close();

            PullEntries();
        }

        #endregion

    }
}
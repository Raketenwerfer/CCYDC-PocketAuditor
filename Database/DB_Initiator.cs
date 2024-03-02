using Android.App;
using Android.Content;
using Android.Database.Sqlite;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PocketAuditor.Database
{
    public class DB_Initiator : SQLiteOpenHelper
    {
        private readonly static string dbname = "CCYDC.db";
        private readonly static string dir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        private readonly static int ver = 1;
        private readonly Context context;

        public string _ConnPath;

        public DB_Initiator(Context context) : base(context, dbname, null, ver)
        {
            this.context = context;
        }

        private string DBPath()
        {
            return Path.Combine(dir, dbname);
        }

        public override SQLiteDatabase WritableDatabase
        {
            get
            {
                return Init();
            }
        }

        public SQLiteDatabase Init()
        {
            SQLiteDatabase sqliteDB = null;
            string path = DBPath();
            _ConnPath = path;
            Stream streamSQLite = null;
            FileStream streamWriter = null;
            Boolean isSQLiteInit = false;

            try
            {
                if (File.Exists(path))
                    isSQLiteInit = true;

                else
                {
                    streamSQLite = context.Assets.Open(dbname);
                    streamWriter = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);

                    if (streamSQLite != null && streamWriter != null)
                    {
                        if (CopySQLiteDB(streamSQLite, streamWriter))
                            isSQLiteInit = true;
                    }
                }
                if (isSQLiteInit)
                    sqliteDB = SQLiteDatabase.OpenDatabase(path, null, DatabaseOpenFlags.OpenReadonly);
            }
            catch { }

            return sqliteDB;
        }

        private bool CopySQLiteDB(Stream streamSQLite, FileStream streamWriter)
        {
            bool isSuccess = false;
            int lenght = 256;
            Byte[] buffer = new Byte[lenght];

            try
            {
                int bytesRead = streamSQLite.Read(buffer, 0, lenght);
                while (bytesRead > 0)
                {
                    streamWriter.Write(buffer, 0, bytesRead);
                    bytesRead = streamSQLite.Read(buffer, 0, lenght);
                }
                isSuccess = true;
            }
            catch { }
            finally
            {
                streamSQLite.Close();
                streamWriter.Close();
            }
            return isSuccess;
        }

        
        public override void OnCreate(SQLiteDatabase db)
        {
            throw new NotImplementedException();
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            throw new NotImplementedException();
        }
    }
}
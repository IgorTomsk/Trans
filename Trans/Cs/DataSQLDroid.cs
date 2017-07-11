using System;
using System.IO;
using Mono.Data.Sqlite;
using System.Data;
using System.Collections.Generic;


namespace Trans
{
    public static class DataSQLDroid
    {
  
        public static SqliteConnection ConnectionDB { get; set; }

        public static bool LoadBase()
        {
            string db = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Translate.db3");

            bool exists = File.Exists(db);
            if (!exists)
                CreateDataBase();
            else
                ConnectionDB = new SqliteConnection("Data Source=" + db);
            return true;
        }

        static Boolean CreateDataBase()
        {
            try
            {
                string db = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Translate.db3");

                SqliteConnection.CreateFile(db);
                ConnectionDB = new SqliteConnection("Data Source=" + db);

                using (var commander = ConnectionDB.CreateCommand())
                {
                    ConnectionDB.Open();
                    commander.CommandText = "CREATE TABLE History (TextFrom ntext, TextTo ntext, Direction ntext, IsFavorite int, TranData ntext)";
                    commander.CommandType = CommandType.Text;
                    commander.ExecuteNonQuery();
                    ConnectionDB.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can not CreateDataBase becase: " + ex);
                return false;
            }
        }


        public static List<HistoryData> DataRead()
        {
            try
            {
                LoadBase();
                ConnectionDB.Close();

                var list = new List<HistoryData>();

                using (var cmd = ConnectionDB.CreateCommand())
                {
                    ConnectionDB.Open();
                   
                    cmd.CommandText = "Select TextFrom, TextTo, Direction, IsFavorite, TranData  from History";
                 
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var l = new HistoryData();
                            l.textFrom = reader[0].ToString();
                            l.textTo = reader[1].ToString();
                            l.direction = reader[2].ToString();
                            l.isFaforite = Convert.ToInt32(reader[3].ToString());
                            l.transData = reader[4].ToString();

                            list.Add(l);
                    
                        }
                    }
                    ConnectionDB.Close();
                }
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ConnectionDB.Close();
                return new List<HistoryData>();
            }
        }
      
    }
}


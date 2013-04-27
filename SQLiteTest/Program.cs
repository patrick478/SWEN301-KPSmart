using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;

namespace SQLiteTest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (File.Exists("test.sqlite"))
                File.Delete("test.sqlite");

            SQLiteConnection con = new SQLiteConnection(String.Format("Data Source={0};Version={1};", "test.sqlite", 3));
            con.Open();

            SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE `test` ('id' INTEGER PRIMARY KEY AUTOINCREMENT, 'created' TIMESTAMP DEFAULT (CURRENT_TIMESTAMP), 'iteration' INTEGER)", con);
            cmd.ExecuteNonQuery();

            using (SQLiteTransaction transaction = con.BeginTransaction())
            {
                for (int i = 0; i < 10; i++)
                {
                    SQLiteCommand cmd2 = new SQLiteCommand("INSERT INTO `test` (iteration) VALUES (?);", con, transaction);
                    cmd2.Parameters.Add(new SQLiteParameter("iteration", i));
                    cmd2.ExecuteNonQuery();
                }

                transaction.Commit();
            }

            SQLiteTransaction transaction2 = con.BeginTransaction();         
                try
                {
                    for (int i = 0; i < 10; i++)
                    {
                        SQLiteCommand cmd2 = new SQLiteCommand("INSERT INTO `blarg` (iteration) VALUES (?);", con,
                                                               transaction2);
                        cmd2.Parameters.Add(new SQLiteParameter("iteration", i));
                        cmd2.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    transaction2.Rollback();
                    transaction2.Dispose();
                }
            

            SQLiteCommand cmd3 = new SQLiteCommand("SELECT COUNT(*) FROM `test`", con);
            long n = (long)cmd3.ExecuteScalar();

            Console.WriteLine("There are {0} rows", n);
            Console.ReadLine();
        }
    }
}

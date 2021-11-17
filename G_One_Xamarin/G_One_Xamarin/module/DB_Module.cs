using System;

namespace G_One_Xamarin.module
{
    using MySqlConnector;
    using Xamarin.Forms;

    internal class DbModule
    {
        private static MySqlConnection Conn()
        {
            const string server = "gone.gvsolgryn.de";
            const string user = "g_one";
            const string password = "g_one";
            const string database = "G_One_DB";
            const string port = "3306";

            var connStr = $"Server={server};Port={port};User={user};Password={password};Database={database};SslMode=None;";

            try
            {
                var conn = new MySqlConnection(connStr);

                conn.Open();

                return conn;
            }

            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert(
                    "DB CMD Error",
                    "관리자에게 문의하세요.\n" + ex.Message,
                    "확인");

                return null;
            }
            
        }

        private static MySqlCommand Command(string sql)
        {
            try
            {
                var cmd = new MySqlCommand(sql, Conn());

                return cmd;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert(
                    "DB CMD Error",
                    "관리자에게 문의하세요.\n" + ex.Message,
                    "확인");

                return null;
            }
        }

        public MySqlDataReader TableLoad(string sql)
        {
            try
            {
                var table = Command(sql).ExecuteReader();

                return table;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("DB CMD Error", "관리자에게 문의하세요.\n" + ex.Message, "확인");

                return null;
            }
        }

        public MySqlCommand Execute(string sql, string[] parameter, string[] data)
        {
            try
            {
                var cmd = new MySqlCommand(sql, Conn())
                {
                    CommandText = sql
                };

                for (var i = 0; i < parameter.Length; i++)
                {
                    cmd.Parameters.AddWithValue(parameter[i], data[i]);
                }
                
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("DB CMD Error", "관리자에게 문의하세요.\n" + ex.Message, "확인");

                return null;
            }
        }
    }
}

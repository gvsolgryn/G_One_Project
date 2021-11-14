using System;

namespace G_One_Xamarin.module
{
    using MySqlConnector;
    using Xamarin.Forms;
    using System.Windows;

    class DB_Module
    {
        public MySqlConnection Conn()
        {
            string server = "gone.gvsolgryn.de";
            string user = "g_one";
            string password = "g_one";
            string database = "G_One_DB";
            string port = "3306";

            string connStr = $"Server={server};Port={port};User={user};Password={password};Database={database};SslMode=None;";

            var conn = new MySqlConnection(connStr);

            conn.Open();

            return conn;
        }

        public MySqlCommand Command(string sql)
        {
            try
            {
                var cmd = new MySqlCommand(sql, Conn());

                return cmd;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("DB CMD Error", "관리자에게 문의하세요.\n" + ex.Message, "확인");
                return null;
            }
        }

        public MySqlDataReader TableLoad(string sql)
        {
            try
            {
                MySqlDataReader table = this.Command(sql).ExecuteReader();

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

                for (int i = 0; i < parameter.Length; i++)
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

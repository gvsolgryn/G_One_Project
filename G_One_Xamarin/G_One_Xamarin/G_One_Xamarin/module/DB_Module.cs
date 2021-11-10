using System;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace G_One.Module
{
    using MySqlConnector;

    class DB_Module
    {
        public MySqlConnection Conn()
        {
            string server = "gone.gvsolgryn.de";
            string user = "g_on";
            string password = "g_one";
            string database = "G_One_DB";
            string port = "3306";

            string connStr = $"Server={server};Port={port};User={user};Password={password};Database={database};SslMode=None;";

            var conn = new MySqlConnection(connStr);

            conn.Open();

            return conn;
        }

        async void Alert(string title, string ex, string ok)
        {
            await Application.Current.MainPage.DisplayAlert(title, ex, ok);
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
                //MessageBox.Show("Command Error : " + ex.Message);
                //DisplayAlert("DB에러", ex.Message, "확인");
                Alert("DB에러", ex.Message, "확인");
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
                //MessageBox.Show("Table Load Error : " + ex.Message);
                Alert("DB에러", ex.Message, "확인");

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
                //MessageBox.Show(ex.Message);
                Alert("DB에러", ex.Message, "확인");

                return null;
            }
        }
    }
}

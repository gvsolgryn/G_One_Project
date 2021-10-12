using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_One_HID_Listener.module
{
    using MySql.Data;
    using MySql.Data.MySqlClient;

    class DB_Module
    {
        public MySqlConnection Conn()
        {
            string server = "gone.gvsolgryn.de";
            string user = "gvsolgryn";
            string password = "tkdeh3554";
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

                var checkSql = sql.ToLower();

                return cmd;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);

                return null;
            }
        }

    }
}

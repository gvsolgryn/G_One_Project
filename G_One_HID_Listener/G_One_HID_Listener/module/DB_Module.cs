using System;
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

            string connStr = $"Server={server};uid={user};Password={password};Database={database};Port={port};";
            Console.WriteLine(connStr);

            var conn = new MySqlConnection(connStr);

            return conn;
        }

        public MySqlCommand Command(string sql)
        {
            try
            {
                Conn().Open();
                var cmd = new MySqlCommand(sql, Conn());
                if (cmd.ExecuteNonQuery() == 1)
                {
                    Console.WriteLine("True");
                }
                else
                {
                    Console.WriteLine("False");
                }

                return cmd;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}

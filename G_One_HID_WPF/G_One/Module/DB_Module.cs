using System;

namespace G_One.Module
{
    using MySql.Data.MySqlClient;
    using System.Windows;

    class DB_Module
    {
        /// <summary>
        /// DB 서버에 연결을 하는 메서드입니다.
        /// Server = "호스트이름 혹은 IP"
        /// user = "유저 이름"
        /// password = "비밀번호"
        /// port = "DB Port"
        /// 를 수정하여 DB 서버를 변경 할 수 있습니다.
        /// </summary>
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
       
        /// <summary>
        /// DB SQL 을 MySQLCommand 에 입력하는 메서드
        /// </summary>
        /// <param name="sql">DB 에서 사용하는 SQL 문</param>
        /// <returns>SQL이 입력된 MySQLCommand 반환</returns>
        public MySqlCommand Command(string sql)
        {
            try
            {
                var cmd = new MySqlCommand(sql, Conn());

                return cmd;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Command Error : " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// cmd 에 입력 된 SQL 중 DB 데이터를 불러오는 문장이 있으면
        /// 이 메서드를 이용하여 DB 로드를 합니다.
        /// </summary>
        /// <param name="sql">SQL 문장</param>
        /// <returns>DB 데이터</returns>
        public MySqlDataReader TableLoad(string sql)
        {
            try
            {
                MySqlDataReader table = this.Command(sql).ExecuteReader();

                return table;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Table Load Error : " + ex.Message);

                return null;
            }
        }

        /// <summary>
        /// DB 에 SQL 문을 적용할 때 사용하는 메서드
        /// </summary>
        /// <param name="sql">DB SQL 문장</param>
        /// <param name="parameter">SQL 문에서 쓰이는 파라미터 값</param>
        /// <param name="data">파라미터에 들어가는 데이터 값</param>
        /// <returns>널값 리턴</returns>
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
                MessageBox.Show(ex.Message);

                return null;
            }
        }
    }
}

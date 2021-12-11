using System;

namespace G_One_Xamarin.module
{
    using MySqlConnector;
    using Xamarin.Forms;

    internal class DbModule
    {
        /// <summary>
        /// DB 서버에 연결을 하는 메서드입니다.
        /// Server = "호스트이름 혹은 IP"
        /// user = "유저 이름"
        /// password = "비밀번호"
        /// port = "DB Port"
        /// 를 수정하여 DB 서버를 변경 할 수 있습니다.
        /// </summary>
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

        /// <summary>
        /// MySQL Command 에 SQL 쿼리문을 등록하는 메서드
        /// </summary>
        /// <param name="sql">쿼리</param>
        /// <returns>SQL 이 적용된 Command</returns>
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

        /// <summary>
        /// DB 에서 데이터를 불러올 때 사용하는 메서드
        /// </summary>
        /// <param name="sql">쿼리</param>
        /// <returns>DB 데이터 값</returns>
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

        /// <summary>
        /// SQL문을 실행 시키는 메서드
        /// </summary>
        /// <param name="sql">쿼리</param>
        /// <param name="parameter">쿼리에서 사용되는 파라미터 값</param>
        /// <param name="data">파라미터 값에 들어가는 데이터 값</param>
        /// <returns></returns>
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace G_One
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 

    using Module;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DB_Load()
        {
            int sensor_count = 0;
            var sensor = string.Empty;
            var status = string.Empty;

            var db = new DB_Module();

            string sql = "SELECT * FROM sensor_status";

            try
            {
                var table = db.TableLoad(sql);

                while (table.Read())
                {
                    sensor = table["sensor"].ToString();
                    status = table["status"].ToString();
                    sensor_count++;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void BtnAdd()
        {
            var btnTest = new Button
            {
                Name = "Btn_Add",
                Content = "버튼 추가 테스트"
            };

            this.MainStackPanel.Children.Add(btnTest);
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            BtnAdd();
        }

        private void MainWindows_Load(object sender, RoutedEventArgs e)
        {
            DB_Load();
        }

    }
}

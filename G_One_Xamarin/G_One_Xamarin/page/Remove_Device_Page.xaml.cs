using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using G_One_Xamarin.module;

namespace G_One_Xamarin.page
{
    public class RemovePickerData
    {
        public string SensorName { get; set; }
    }
    public partial class RemoveDevicePage : ContentPage
    {
        private readonly ObservableCollection<RemovePickerData> _removePickerData = new ObservableCollection<RemovePickerData>();
        public RemoveDevicePage()
        {
            InitializeComponent();

            var db = new DbModule();
            try
            {
                const string sql = "SELECT * FROM sensor_status";

                var table = db.TableLoad(sql);

                while (table.Read())
                {
                    _removePickerData.Add(new RemovePickerData{ SensorName = table["sensor"].ToString() });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            RemoveDevicePicker.ItemsSource = _removePickerData;
        }

        private async void RemoveButton_OnClicked(object sender, EventArgs e)
        {
            try
            {
                var dataIdx = RemoveDevicePicker.SelectedIndex;
                Console.WriteLine("pickData : " + RemoveDevicePicker.Items[dataIdx]);
                const string sql = "DELETE FROM sensor_status WHERE sensor=@sensor";

                var db = new DbModule();
                try
                {
                    db.Execute(sql, new [] {"@sensor"}, new []{RemoveDevicePicker.Items[dataIdx]});
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    await Application.Current.MainPage.DisplayAlert("", ex.Message, "ok");
                    throw;
                }

                await Application.Current.MainPage.DisplayAlert("삭제 완료", "디바이스 삭제 완료", "확인");

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("기기 삭제 오류", "에러 메세지 : " + ex.Message, "확인");
                
                await Navigation.PopAsync();
            }
        }

        private async void CancelButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}

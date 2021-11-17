using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using G_One_Xamarin.module;
using Xamarin.CommunityToolkit.UI.Views;

namespace G_One_Xamarin.page
{
    public class AddPickerData
    {
        public string SensorName { get; set; }
    }

    public class TypePickerData
    {
        public string SensorType { get; set; }
    }
    public partial class AddDevicePage : ContentPage
    {
        private ObservableCollection<AddPickerData> _addPickerSource = new ObservableCollection<AddPickerData>();
        private ObservableCollection<TypePickerData> _typePickerSource = new ObservableCollection<TypePickerData>();

        public AddDevicePage()
        {
            InitializeComponent();
            try
            {
                DbModule db = new DbModule();
                string sql = "SELECT * FROM compatible_device";
                var table = db.TableLoad(sql);

                while (table.Read())
                {
                    _addPickerSource.Add(new AddPickerData{ SensorName = table["name"].ToString() });
                    _typePickerSource.Add(new TypePickerData{ SensorType = table["device_type"].ToString() });
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error Add_Device_Page : " + ex.Message);
            }

            DeviceAddPicker.ItemsSource = _addPickerSource;
            DeviceTypePicker.ItemsSource = _typePickerSource;
        }

        private async void AddButton_OnClicked(object sender, EventArgs e)
        {
            var addIdx = DeviceAddPicker.SelectedIndex;
            var typeIdx = DeviceTypePicker.SelectedIndex;

            var db = new DbModule();
            
            const string sql = "INSERT INTO sensor_status(sensor, status, device_type, led_value, last_use) VALUES(@sensor, '0', @device_type, '0', now())";

            try
            {
                db.Execute(sql, new[] {"@sensor", "@device_type"},
                    new[] {DeviceAddPicker.Items[addIdx], DeviceTypePicker.Items[typeIdx]});
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error Add Device", "Exception : " + ex.Message, "OK");

                await Navigation.PopAsync();
            }
            
            await Application.Current.MainPage.DisplayAlert("추가 완료", "디바이스 추가 완료", "확인");

            await Navigation.PopAsync();
        }

        private async void CancelButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_One.Module
{
    internal class DeviceControl
    {
        DB_Module db = new DB_Module();
        public void DeviceOn(string name)
        {
            string sql = "UPDATE sensor_status SET STATUS = 1, last_use = now() WHERE SENSOR = @sensorName";
            
            db.Execute(sql, new[] { "@sensorName" }, new[] { name });
        }

        public void DeviceOff(string name)
        {
            string sql = "UPDATE sensor_status SET STATUS = 0, last_use = now() WHERE SENSOR = @sensorName";

            db.Execute(sql, new[] { "@sensorName" }, new[] { name });
        }
    }
}

import time

from . import dbModule

class Update():
    def sql_update(name, status, led_value):
        sql = "UPDATE sensor_status set STATUS = %s, led_value = %s, Last_Use = now() WHERE sensor = %s"
        log_sql = "INSERT INTO log(use_program, sensor, def_location, sql_success, error_log, sql_run_time) VALUES(%s, %s, %s, %s, %s, %s)"
        sw_start = time.time()

        if status == 1: 
            status_str = '_ON'
        if status == 0:
            status_str = '_OFF'
    
        db = dbModule.Database()
        try:
            db.execute(sql, (status, led_value, str(name)))
            sql_end = time.time()
            sql_code_runtime = round(sql_end - sw_start, 2)
            db.execute(log_sql, ('flask_debug', str(name), str(name) + status_str, 'success', 'none', str(sql_code_runtime)))
        except Exception as e:
            sql_end = time.time()
            sql_code_runtime = round(sql_end - sw_start, 2)
            db.execute(log_sql, ('flask_debug', str(name), str(name) + status_str, 'fail', str(e), str(sql_code_runtime)))
        finally:
            db.commit()
            #db_class.close()

class iotAdd():
    def sql_insert(name, type, led_value):
        sql = "INSERT INTO sensor_status(sensor, device_type, status, led_value, last_use) VALUES(%s, %s, 0, %s, now())"
        log_sql = "INSERT INTO log(use_program, sensor, def_location, sql_success, error_log, sql_run_time) VALUES(%s, %s, %s, %s, %s, %s)"
        sw_start = time.time()
        db = dbModule.Database()


        try:
            db.execute(sql, (name, type, led_value))
            sql_end = time.time()
            sql_code_runtime = round(sql_end - sw_start, 2)
            db.execute(log_sql, ('Flask_Add_IoT', str(name), str(name) + '_Add', 'success', 'none', str(sql_code_runtime)))
        except Exception as e:
            print("iot_device_INSERT_Error")
            error_sql = "INSERT INTO log(use_program, sensor, def_location, sql_success, error_log, sql_run_time) VALUES(%s, %s, %s, %s, %s, %s)"
            db.execute(error_sql, ('Flask_Add_IoT', str(name), 'iotAdd', 'fail', str(e), '0.00'))
        finally:
            db.commit()

class iotDel():
    def sql_insert(name):
        sql = "DELETE FROM sensor_status WHERE sensor='" + str(name) + "'"
        log_sql = "INSERT INTO log(use_program, sensor, def_location, sql_success, error_log, sql_run_time) VALUES(%s, %s, %s, %s, %s, %s)"
        sw_start = time.time()
        db = dbModule.Database()

        try:
            db.execute(sql)
            sql_end = time.time()
            sql_code_runtime = round(sql_end - sw_start, 2)
            db.execute(log_sql, ('Flask_Delete_IoT', str(name), str(name) + '_Delete', 'success', 'none', str(sql_code_runtime)))
        except Exception as e:
            print("iot_device_Delete_Error")
            print(e)
            error_sql = "INSERT INTO log(use_program, sensor, def_location, sql_success, error_log, sql_run_time) VALUES(%s, %s, %s, %s, %s, %s)"
            db.execute(error_sql, ('Flask_Delete_IoT', str(name), 'iotDel', 'fail', str(e), '0.00'))
        finally:
            db.commit()
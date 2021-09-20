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
    
        db_class = dbModule.Database()
        try:
            db_class.execute(sql, (status, led_value, str(name)))
            sql_end = time.time()
            sql_code_runtime = round(sql_end - sw_start, 2)
            db_class.execute(log_sql, ('flask_debug', str(name), str(name) + status_str, 'success', 'none', str(sql_code_runtime)))
        except Exception as e:
            sql_end = time.time()
            sql_code_runtime = round(sql_end - sw_start, 2)
            db_class.execute(log_sql, ('flask_debug', str(name), str(name) + status_str, 'fail', str(e), str(sql_code_runtime)))
        finally:
            db_class.commit()
            #db_class.close()

        
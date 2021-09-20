# -*- coding: utf-8 -*-
import re
from flask import Flask, render_template, json, request, redirect, url_for
from flask_mqtt import Mqtt

from module import dbModule, mqtt_IDPW, dbEdit

import time

app = Flask(__name__)
app.config['TEMPLATES_AUTO_RELOAD'] = True
app.config['MQTT_CLIENT_ID'] = mqtt_IDPW.Client()
app.config['MQTT_BROKER_URL'] = mqtt_IDPW.Host()
app.config['MQTT_BROKER_PORT'] = mqtt_IDPW.Port()
app.config['MQTT_USERNAME'] = mqtt_IDPW.ID()
app.config['MQTT_PASSWORD'] = mqtt_IDPW.PW()
app.config['MQTT_TLS_ENABLED'] = False
app.config['MQTT_REFRESH_TIME'] = 1.0  # refresh time in seconds

mqtt = Mqtt(app)

@mqtt.on_connect()
def handle_connect(client, userdata, flags, rc):
    print("MQTT 서버 연결 성공")
    mqtt.subscribe('iot/#')

@mqtt.on_message()
def handle_mqtt_message(client, userdata, message):
    data = dict(
        topic=message.topic,
        payload=message.payload.decode()
    )
    print(data)

def sensor_info():
    db_class = dbModule.Database()
    sql = "SELECT * FROM sensor_status"
    result = db_class.executeAll(sql)

    return tuple(result)

@app.route('/')
def index():
    result = sensor_info()
    
    # data 의 값이 result 의 값이 될 때 까지 반복실행
    result_id = []
    result_sensor = []
    result_status = []
    result_led_value = []

    for data in result:
        result_id.append(data['id'])
        result_sensor.append(data['sensor'])
        result_status.append(data['status'])
        if data['sensor'] == 'LED':
            result_led_value.append(data['led_value'])

    return render_template('test.html', id = len(result_id), name = result_sensor, status = result_status, req_led_value = result_led_value)

@app.route('/sensorTrigger', methods=['POST'])
def sensorTrigger():
    result = sensor_info()

    result_id = []
    result_sensor = []
    result_status = []

    for data in result:
        result_id.append(data['id'])
        result_sensor.append(data['sensor'])
        result_status.append(data['status'])

    name = request.form['name']
    trigger = request.form['trigger']
    led_value = request.form['led_value']

    dbEdit.Update.sql_update(name, int(trigger), led_value)
    if trigger == "0":
        dbEdit.Update.sql_update(name, int(trigger), 0)

    try:
        for i in range(0, len(result_id), 1):
            if name == result_sensor[i]:
                if trigger == "0":
                    mqtt.publish('iot/' + str(result_sensor[i]), "0")
                elif trigger == "1":
                    mqtt.publish('iot/' + str(result_sensor[i]), "1")
    except Exception as e:
        print("sensor_trigger Error : " + str(e))
        return(redirect(url_for('error_db')))
    
    return redirect(url_for('index'))

@app.route('/ledAdjust', methods=['POST'])
def ledAdjust():
    value = request.args.get('led_value')
    mqtt.publish("LEDAdjust", value)

    sql = "UPDATE sensor_status set led_value = %s, Last_Use = now() WHERE sensor = %s"
    db_class = dbModule.Database()
    try:
        db_class.execute(sql, (value, "LED"))
    except Exception as e:
        return redirect(url_for('error_db'))
    finally:
        db_class.commit()
        #db_class.close()
    return redirect(url_for('index'))

@app.route('/add', methods=['POST'])
def iot_add():
    return redirect(url_for('index'))

@app.route('/error_db')
def error_db():
    return "<a href='/'><h1>DB 에러! 관리자에게 문의하세요! 클릭 시 메인 화면으로 이동합니다!</h1></a>"

if __name__ == '__main__':
    app.run(host='0.0.0.0', debug=True, port=8080)
    #, ssl_context=('/etc/letsencrypt/live/gvsolgryn.ddns.net/cert.pem', '/etc/letsencrypt/live/gvsolgryn.ddns.net/privkey.pem'))
# -*- coding: utf-8 -*-
from flask import Flask, flash, render_template, json, request, redirect, url_for
from flask_mqtt import Mqtt
from flask_socketio import SocketIO

from module import dbModule, mqtt_IDPW, dbEdit

import time

app = Flask(__name__)
app.config['TEMPLATES_AUTO_RELOAD'] = True
app.config['SECRET_KEY'] = "tkdeh3554!@#$%"
app.config['MQTT_CLIENT_ID'] = mqtt_IDPW.Client()
app.config['MQTT_BROKER_URL'] = mqtt_IDPW.Host()
app.config['MQTT_BROKER_PORT'] = mqtt_IDPW.Port()
app.config['MQTT_USERNAME'] = mqtt_IDPW.ID()
app.config['MQTT_PASSWORD'] = mqtt_IDPW.PW()
app.config['MQTT_KEEPALIVE'] = 5
app.config['MQTT_TLS_ENABLED'] = False

socketio = SocketIO(app, cors_allowed_origins="*")

mqtt = Mqtt(app)

#----------------#
####전역 변수 설정####
#----------------#

result_id = []
result_sensor = []
result_status = []
result_led_value = []
result_device_type = []

def sensor_info():
    db_class = dbModule.Database()
    sql = "SELECT * FROM sensor_status"
    result = db_class.executeAll(sql)

    return tuple(result)

def sensor_list():
    db_class = dbModule.Database()
    sql = "SELECT * FROM compatible_device"
    result = db_class.executeAll(sql)

    return tuple(result)

########################################

@app.route('/')
def index():
    result = sensor_info()

    global result_id
    global result_sensor
    global result_status
    global result_led_value
    global result_device_type

    result_id = []
    result_sensor = []
    result_status = []
    result_device_type = []
    result_led_value = []

    # data 의 값이 result 의 값이 될 때 까지 반복실행
    for data in result:
        result_id.append(data['id'])
        result_sensor.append(data['sensor'])
        result_status.append(data['status'])
        result_led_value.append(data['led_value'])
        result_device_type.append(data['device_type'])

    device_list = sensor_list()

    device_list_sensor = []
    device_list_device_type = []

    for data in device_list:
        device_list_sensor.append(data['name'])
        device_list_device_type.append(data['device_type'])


    return render_template('test.html',
                            id = len(result_id),
                            name = result_sensor,
                            status = result_status,
                            req_led_value = result_led_value,
                            device_type = result_device_type,
                            list_device = device_list_sensor,
                            list_device_type = device_list_device_type,
                            list_device_id = len(device_list_sensor))

@app.route('/sensorTrigger', methods=['POST'])
def sensorTrigger():
    result = sensor_info()

    global result_id
    global result_sensor
    global result_status

    device_list = sensor_list()

    name = request.form['name']
    trigger = request.form['trigger']
    led_value = request.form['led_value']
    
    print("led_value = " + led_value)

    dbEdit.Update.sql_update(name, int(trigger), led_value)
    try:
        if trigger == "0":
            dbEdit.Update.sql_update(name, int(trigger), 0)
    
        for i in range(0, len(result_id), 1):
            if name == result_sensor[i]:
                if trigger == "0":
                    mqtt.publish('iot/' + str(result_sensor[i]), "0")
                    print(str(i) + "번 처리 완료")
                elif trigger == "1":
                    mqtt.publish('iot/' + str(result_sensor[i]), "1")
                    print(str(i) + "번 처리 완료")

            if name == result_sensor[i] and result_sensor[i].lower().startswith('led'):
                mqtt.publish('iot/' + str(result_sensor[i]) + 'Adjust', str(led_value))
                
        if name == "Brightness_control_LED":
                mqtt.publish('iot/LEDAdjust', str(led_value))

            
    except Exception as e:
        print("sensor_trigger Error : " + str(e))
        return(redirect(url_for('error_db')))
    
    return redirect(url_for('index'))

@app.route('/add', methods=['POST'])
def iot_add():
    name = request.form['sensor_name']
    type = request.form['sensor_type']
    led_value = '0'

    if (name in result_sensor):
        flash("이미 추가 된 센서입니다.")

        return redirect(url_for('index'))

    dbEdit.iotAdd.sql_insert(name, type, led_value)

    return redirect(url_for('index'))

@app.route('/deleteIoT', methods=['POST'])
def iot_delete():
    name = request.form['del_iot_name']
    db = dbModule.Database()
    print(name)

    dbEdit.iotDel.sql_insert(name)
    return redirect(url_for('index'))

@app.route('/error_db')
def error_db():
    return "<a href='/'><h1>DB 에러! 관리자에게 문의하세요! 클릭 시 메인 화면으로 이동합니다!</h1></a>"


#######################################################

@socketio.on('message')
def handle_message(data):
    print('받은 데이터 : ' + data)
    
@socketio.on('socketioConnect')
def handle_connect_message(json):
    print(str(json))
    mqtt.subscribe('iot/#')

#######################################################


@mqtt.on_connect()
def handle_connect(client, userdata, flags, rc):
    print("MQTT 서버 연결 성공")
    

@mqtt.on_message()
def handle_mqtt_message(client, userdata, message):
    data = dict(
        topic=message.topic,
        payload=message.payload.decode()
    )

    
    socketio.emit('mqtt_message', data)
    socketio.emit('testio')

    print(data)


if __name__ == '__main__':
    socketio.run(app, host='0.0.0.0', port=8080, debug=True)
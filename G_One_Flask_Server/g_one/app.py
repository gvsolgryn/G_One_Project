# -*- coding: utf-8 -*-
from flask import Flask, render_template, json, request, redirect, url_for
from flask_mqtt import Mqtt

from module import dbModule, mqtt_IDPW, sensorUpdate

import time

app = Flask(__name__)
app.config['TEMPLATES_AUTO_RELOAD'] = True
app.config['MQTT_CLIENT_ID'] = mqtt_IDPW.Client
app.config['MQTT_BROKER_URL'] = mqtt_IDPW.Host
app.config['MQTT_BROKER_PORT'] = mqtt_IDPW.Port
app.config['MQTT_USERNAME'] = mqtt_IDPW.ID
app.config['MQTT_PASSWORD'] = mqtt_IDPW.PW
app.config['MQTT_TLS_ENABLED'] = False
app.config['MQTT_REFRESH_TIME'] = 1.0  # refresh time in seconds

mqtt = Mqtt(app)

@app.route('/')
def index():
    db_class = dbModule.Database()
    sql = "SELECT * FROM sensor_status"
    result = db_class.executeAll(sql)

    # data 의 값이 result 의 값이 될 때 까지 반복실행
    for data in result:
        if data['sensor'] == "LED":
            LED_Status = data['status']
            LED_Value = data['led_value']
        
        if data['sensor'] == "MULTI":
            MULTI_Status = data['status']

    print("----------센서 상태 출력----------")
    print("LED 센서 상태 : " + str(LED_Status))
    print("멀티탭 센서 상태 : " + str(MULTI_Status))
    print("----------상태 출력 종료----------")

    return render_template('index.html', LED = LED_Status, MULTI = MULTI_Status, VALUE = LED_Value)

@app.route('/sensorTrigger', methods=['POST'])
def sensorTrigger():
    name = request.form['name']
    trigger = request.form['trigger']
    sensorUpdate.Update.sql_update(name, int(trigger))
    if name == "LED":
        if trigger == "1":
            mqtt.publish("LEDTopic", "1")
        elif trigger == "0":
            mqtt.publish("LEDTopic", "0")
    elif name == "MULTI":
        if trigger == "1":
            mqtt.publish("MULTITopic", "1")
        elif trigger == "0":
            mqtt.publish("MULTITopic", "0")
    else:
        error = Exception
        return print(error)
    
    return redirect(url_for('index'))

@app.route('/ledAdjust', methods=['GET'])
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
        db_class.close()
    return redirect(url_for('index'))

@app.route('/error_db')
def error_db():
    return "<a href='/'><h1>DB 에러! 관리자에게 문의하세요! 클릭 시 메인 화면으로 이동합니다!</h1></a>"

if __name__ == '__main__':
    app.run(host='0.0.0.0', debug=True, port=8080, ssl_context=('/etc/letsencrypt/live/gvsolgryn.ddns.net/cert.pem', '/etc/letsencrypt/live/gvsolgryn.ddns.net/privkey.pem'))
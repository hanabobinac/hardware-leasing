from flask import Flask
from flask import request
import json
import io
import datetime
import sqlite3 as sqlite
from hardware import Hardware

app = Flask(__name__)

@app.route('/list', methods=['GET'])
def list_hardware():
    rows = execute_select_query("SELECT * FROM hardware")
    hw_list = tuple_to_hw_object(rows)
    return json.dumps(hw_list)

@app.route('/listleased', methods=['GET'])
def list_leased_hardware():
    rows = execute_select_query("SELECT * FROM hardware")
    hw_list = tuple_to_hw_object(rows)
    leased_hw = list(filter(lambda x: x.is_leased == True, hw_list))
    return json.dumps(leased_hw)

@app.route('/listbyplatform/<platform>', methods=['GET'])
def list_hardware_by_platform(platform):
    rows = execute_select_query("SELECT * FROM hardware WHERE platform = ?", [platform])
    hw_list = tuple_to_hw_object(rows)
    return json.dumps(hw_list)

@app.route('/add', methods=['PUT'])
def add_hardware():
    item = request.json
    item = (item["name"], item["ip"], item["platform"], 0, None)
    rows = execute_select_query("SELECT name FROM hardware WHERE name = '{}'".format(item[0]))
    if len(rows):
        return ("Hardware not added because it already exists", 400)
    execute_insert_query("INSERT INTO hardware (name, ip, platform, lease_duration, lease_date) VALUES (?, ?, ?, ?, ?)", item)
    return ("Hardware added", 201)

@app.route('/lease', methods=['POST'])
def lease_hardware():
    item = request.json
    rows = execute_select_query("SELECT * FROM hardware WHERE platform = ?", (item["platform"],))
    hw_list = tuple_to_hw_object(rows)
    leased_hw_list = list(filter(lambda x: x.is_leased == False, hw_list))
    if len(leased_hw_list):
        leased_hw = (item["lease_duration"], datetime.datetime.now(), leased_hw_list[0].name)
        execute_insert_query("UPDATE hardware SET lease_duration = ?, lease_date = ? WHERE name = ?", leased_hw)
        return ("Hardware leased", 202)
    return ("No available hw platform to lease", 400)

def execute_select_query(query, item = None):
    conn = sqlite.connect("./hardware.db", detect_types=sqlite.PARSE_DECLTYPES|sqlite.PARSE_COLNAMES)
    cur = conn.cursor()
    if item is None:
        cur.execute(query)
    else:
        cur.execute(query, item)
    rows = cur.fetchall()
    cur.close()
    conn.close()
    return rows

def execute_insert_query(query, item):
    conn = sqlite.connect("./hardware.db", detect_types=sqlite.PARSE_DECLTYPES|sqlite.PARSE_COLNAMES)
    cur = conn.cursor()
    cur.execute(query, item)
    conn.commit()
    cur.close()
    conn.close()
    return "OK"

def tuple_to_hw_object(rows):
    hw_list = list()
    for row in rows:
        hw_list.append(Hardware(row))
    return hw_list

if __name__ == '__main__':
    app.run(debug = True, host = '0.0.0.0') 



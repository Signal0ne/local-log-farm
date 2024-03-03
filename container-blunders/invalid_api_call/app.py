from flask import Flask, jsonify
import sqlite3
import traceback

app = Flask(__name__)

# Create a connection to the SQLite database
conn = sqlite3.connect('example.db')

# Endpoint to create a table in the database
@app.route('/create-table', methods=['GET'])
def create_table():
    try:
        # Create a new table in the database
        cursor = conn.cursor()
        cursor.execute('CREATE TABLE IF NOT EXISTS users (id INTEGER PRIMARY KEY, name TEXT)')
        conn.commit()
        return jsonify({'message': 'Table created successfully'}), 200
    except Exception as e:
        error_message = str(e)
        traceback.print_exc()
        return jsonify({'error': error_message}), 500

# Endpoint to insert data into the database
@app.route('/insert-data', methods=['GET'])
def insert_data():
    try:
        # Insert data into the table
        cursor = conn.cursor()
        cursor.execute('INSERT INTO users (name) VALUES (?)', ('John Doe',))
        conn.commit()
        return jsonify({'message': 'Data inserted successfully'}), 200
    except Exception as e:
        error_message = str(e)
        traceback.print_exc()
        return jsonify({'error': error_message}), 500

# Endpoint to retrieve data from the database
@app.route('/get-data', methods=['GET'])
def get_data():
    try:
        # Retrieve data from the table
        cursor = conn.cursor()
        cursor.execute('SELECT * FROM users')
        rows = cursor.fetchall()
        return jsonify({'data': rows}), 200
    except Exception as e:
        error_message = str(e)
        traceback.print_exc()
        return jsonify({'error': error_message}), 500

if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0')

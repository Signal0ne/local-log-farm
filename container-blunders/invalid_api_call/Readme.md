## invalid api call

1. create the docker image

```
docker build -t flask-app.
```

2. Run the container

```
docker run -p 5000:5000 flask-app
```

3. Go to 127.0.0.1:5000 and send a get request in `create-table` route. So click on this to send request : http://127.0.0.1:5000/create-table

4. Now check your docker logs using docker logs $container-id . You will find some errors like this in your running container : 

```
 * Serving Flask app 'app'
 * Debug mode: on
WARNING: This is a development server. Do not use it in a production deployment. Use a production WSGI server instead.
 * Running on all addresses (0.0.0.0)
 * Running on http://127.0.0.1:5000
 * Running on http://172.17.0.2:5000
Press CTRL+C to quit
 * Restarting with stat
 * Debugger is active!
 * Debugger PIN: 466-333-740
172.17.0.1 - - [24/Feb/2024 16:21:59] "GET / HTTP/1.1" 404 -
Traceback (most recent call last):
  File "/app/app.py", line 15, in create_table
    cursor = conn.cursor()
sqlite3.ProgrammingError: SQLite objects created in a thread can only be used in that same thread. The object was created in thread id 140135376844608 and this is thread id 140135347230464.
172.17.0.1 - - [24/Feb/2024 16:22:05] "GET /create-table HTTP/1.1" 500 -
```
import psycopg2

def init_db_connection(db_synthetic_connection_string):
    return psycopg2.connect(db_synthetic_connection_string)
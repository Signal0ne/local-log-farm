import os
from dotenv import load_dotenv


class Config():

    def __init__(self):
        load_dotenv()
        self.db_host = os.getenv('POSTGRES_HOST')
        self.db_name = os.getenv('POSTGRES_DB')
        self.db_username = os.getenv('POSTGRES_USER')
        self.db_password = os.getenv('POSTGRES_PASSWORD')
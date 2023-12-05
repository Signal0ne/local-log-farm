import grpc
import asyncio
import logging
import sys
from profileservice_py import profileservice_pb2_grpc
from profileservice_server.server import ProfileServiceServicer
from opentelemetry import trace
from config import server_config
from config import database

async def main() -> None:
    logger = logging.getLogger(__name__)
    logger.setLevel(logging.DEBUG)
    ch = logging.StreamHandler(sys.stdout)
    ch.setLevel(logging.DEBUG)
    formatter = logging.Formatter('%(asctime)s - %(name)s - %(levelname)s - %(message)s')
    ch.setFormatter(formatter)
    logger.addHandler(ch)
    server_cfg = server_config.Config()
    server = grpc.aio.server()
    db_conn_healthy = False
    conn_str = "dbname={0} user={1} password={2} host={3}".format(
                    server_cfg.db_name, 
                    server_cfg.db_username, 
                    server_cfg.db_password,
                    server_cfg.db_host
                )
    while not db_conn_healthy:
        try:
            conn = database.init_db_connection(conn_str)
            db_conn_healthy = True
        except Exception as e:
            print("DB connection failed err: {0}, retrying...".format(e))
            await asyncio.sleep(1)
    profileservice_pb2_grpc.add_ProfileServiceServicer_to_server(
        ProfileServiceServicer(
                db_conn=conn,
                logger=logger
            ), server
    )
    server.add_insecure_port('[::]:8089')
    await server.start()
    await server.wait_for_termination()
    conn.close()

if __name__ == '__main__':
    asyncio.new_event_loop().run_until_complete(main())
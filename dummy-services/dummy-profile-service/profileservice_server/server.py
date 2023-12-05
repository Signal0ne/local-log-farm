import grpc

from profileservice_py import profileservice_pb2
from profileservice_py import profileservice_pb2_grpc
from opentelemetry import trace
from datetime import datetime

class ProfileServiceServicer(profileservice_pb2_grpc.ProfileServiceServicer):

    def __init__(self, db_conn, logger) -> None:
        self.db_conn = db_conn
        self.logger = logger
        self.tracer = trace.get_tracer(__name__)

    async def GetProfile(
            self, request: profileservice_pb2.GetProfileRequest, 
            context: grpc.ServicerContext
        ) -> profileservice_pb2.GetProfileResponse:
        q_prefix=""
        id = request.id
        if request.id == "1":
            q_prefix="SELECT pg_sleep(3);"
        q = (
                q_prefix + "SELECT * FROM profiles WHERE id = '{0}' ;"
            ).format(id) 
        curs = self.db_conn.cursor()
        start = datetime.now()
        curs.execute(
            query=q
        )
        end = datetime.now()
        self.logger.info("executed query: {0}, time taken: {1}".format(q, end-start))
        records = curs.fetchone()
        curs.close()
        if records is None:
            context.set_code(grpc.StatusCode.NOT_FOUND)
            context.set_details("profile not found")
            self.logger.error("profile not found")
            return profileservice_pb2.GetProfileResponse()
        return profileservice_pb2.GetProfileResponse(
            profile=profileservice_pb2.Profile(
                id=str(records[0]),
                name=records[1],
                email=records[2]
        ))

    async def SetProfile(
            self, request, context
        ) -> profileservice_pb2.SetProfileResponse:
        name = request.name
        email = request.email
        if name == "" or email == "":
            context.set_code(grpc.StatusCode.INVALID_ARGUMENT)
            context.set_details("name and email are required")
            self.logger.error("name and email are required")
            return profileservice_pb2.SetProfileResponse()
        trace.get_current_span().set_attribute("api-version","v1")
        try:
            curs = self.db_conn.cursor()
            curs.execute(
                query=(
                        "INSERT INTO profiles (name, email) VALUES ('{0}', '{1}') RETURNING id"
                    ).format(name, email)
            )
            id = curs.fetchone()[0]
            curs.close()
        except Exception as e:
            self.logger.error("failed to insert profile err: {0}".format(e))
            return
        return profileservice_pb2.SetProfileResponse(
            profile=profileservice_pb2.Profile(
                id=str(id),
                name=name,
                email=email
        ))
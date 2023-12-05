package grpc_server

import (
	"context"
	"fmt"
	"log"
	"time"

	scoreservicepb "github.com/SzymonSt/autoinstrumentation-playground/dummy-proto/scoreservice-go/v1"
	"go.mongodb.org/mongo-driver/mongo"
	"go.opentelemetry.io/otel/attribute"
	"go.opentelemetry.io/otel/trace"
)

type ScoreServiceServer struct {
	dbClient *mongo.Client
	dbName   string
	logger   *log.Logger

	scoreservicepb.UnimplementedScoreServiceServer
}

func NewScoreServiceServer(dbClient *mongo.Client, logger *log.Logger) *ScoreServiceServer {
	return &ScoreServiceServer{
		dbClient: dbClient,
		dbName:   "dummy-scores",
		logger:   logger,
	}
}

func (ss *ScoreServiceServer) SubmitScore(ctx context.Context, req *scoreservicepb.ScoreRequest) (*scoreservicepb.ScoreResponse, error) {
	var err error
	span := trace.SpanFromContext(ctx)
	record := &scoreservicepb.ScoreRecord{
		UserProfileName: req.UserProfile,
		Score:           req.Score,
		Time:            time.Now().Unix(),
	}
	fmt.Printf("trace id: %s", span.SpanContext().TraceID())
	span.SetAttributes(
		attribute.String("user_profile", record.UserProfileName),
	)
	ss.dbClient.Database(ss.dbName).Collection("scores").InsertOne(ctx, record)
	log.Printf("Submitted score for user %s\n", record.UserProfileName)
	return &scoreservicepb.ScoreResponse{
		ScoreRecord: record,
	}, err
}

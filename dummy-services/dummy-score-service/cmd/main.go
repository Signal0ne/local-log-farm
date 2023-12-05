package main

import (
	"context"
	"fmt"
	"log"
	"net"
	"os"
	"scoreservice/pkg/config"
	"scoreservice/pkg/db"
	"scoreservice/pkg/grpc_server"
	"time"

	scoreservicepb "github.com/SzymonSt/autoinstrumentation-playground/dummy-proto/scoreservice-go/v1"
	"google.golang.org/grpc"
)

func main() {
	var (
		configName = "local"
	)
	logger := log.New(os.Stdout, "scoreservice", log.LstdFlags|log.Lshortfile)
	cfSrv, err := config.NewConfigServer(configName)
	if err != nil {
		panic(err)
	}
	dbClient, err := db.ConnectDB(cfSrv.DBUri, cfSrv.DDConnRetries)
	if err != nil {
		panic(err)
	}
	listener, err := net.Listen("tcp", fmt.Sprintf("0.0.0.0:%s", cfSrv.ServerPort))
	if err != nil {
		panic(err)
	}
	grpcServer := grpc.NewServer(
	// grpc.UnaryInterceptor(injectedLatencyInterceptor),
	)
	scoreservicepb.RegisterScoreServiceServer(grpcServer,
		grpc_server.NewScoreServiceServer(dbClient, logger),
	)
	fmt.Printf("Starting gRPC server on port %s\n", cfSrv.ServerPort)
	err = grpcServer.Serve(listener)
	if err != nil {
		panic(err)
	}
}

func injectedLatencyInterceptor(ctx context.Context, req interface{}, info *grpc.UnaryServerInfo, handler grpc.UnaryHandler) (interface{}, error) {
	fmt.Printf("About to intercept %s\n", info.FullMethod)
	time.Sleep(200 * time.Millisecond)
	fmt.Printf("Intercepted request to %s\n", info.FullMethod)
	resp, err := handler(ctx, req)
	return resp, err
}

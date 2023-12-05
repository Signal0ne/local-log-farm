package db

import (
	"context"
	"fmt"

	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/mongo/options"
)

func ConnectDB(dbUrl string, dbConnectionRetries int) (dbClient *mongo.Client, err error) {

	dbClient, err = mongo.Connect(context.Background(), options.Client().ApplyURI(dbUrl))
	if err != nil {
		return
	}
	for err = dbClient.Ping(context.Background(), nil); err != nil; {
		if dbConnectionRetries == 0 {
			return
		}
		fmt.Println("Retrying connection to DB, error: ", err)
		dbConnectionRetries--
	}
	return
}

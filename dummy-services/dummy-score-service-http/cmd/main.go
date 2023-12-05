package main

import (
	"encoding/json"
	"scoreservicehttp/pkg/config"
	"scoreservicehttp/pkg/db"

	"github.com/gin-contrib/cors"
	"github.com/gin-gonic/gin"
)

type Score struct {
	UserProfile string `json:"UserProfile"`
	Score       int    `json:"Score"`
}

func main() {
	var (
		configName = "local"
		server     = gin.Default()
	)
	cfSrv, err := config.NewConfigServer(configName)
	if err != nil {
		panic(err)
	}
	dbClient, err := db.ConnectDB(cfSrv.DBUri, cfSrv.DDConnRetries)
	if err != nil {
		panic(err)
	}
	corsConfig := cors.DefaultConfig()
	corsConfig.AllowOrigins = []string{"*"}
	corsConfig.AllowCredentials = true

	server.Use(cors.New(corsConfig))

	router := server.Group("/api")
	router.PUT("/submit_score", func(ctx *gin.Context) {
		var payload []byte
		var score Score
		body := ctx.Request.Body
		defer body.Close()
		if body == nil {
			ctx.JSON(400, gin.H{"message": "no payload"})
			return
		}
		_, err := body.Read(payload)
		if err != nil {
			ctx.JSON(500, gin.H{"message": "error reading payload"})
			return
		}
		json.Unmarshal(payload, &score)
		dbClient.Database("dummy-scores").Collection("scores").InsertOne(ctx, score)
		ctx.JSON(200, gin.H{"message": "ok"})
	})

	server.Run(":8084")
}

# build local-log-farm
## dummy-services
### dummy-frontend
`docker build -t 322456/dummy-frontend:latest -f ./dummy-frontend/Dockerfile ./dummy-frontend/`
### dummy-gateway
`docker build -t 322456/dummy-gateway:latest -f ./dummy-gateway/Dockerfile .`
### dummy-profile-service
`docker build -t 322456/dummy-profile-service:latest -f ./dummy-profile-service/Dockerfile ./dummy-profile-service/`
### dummy-score-service
`docker build -t 322456/dummy-score-service:latest -f ./dummy-score-service/Dockerfile ./dummy-score-service/`
# run local-log-farm
workdir: /dummy-services <br>
`docker compose up -d`
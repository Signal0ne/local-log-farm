build-container-blunders:
	docker build --tag=signaloneai/broken_bulb:dev ./broken_bulb
	docker build --tag=signaloneai/faulty_roger:dev ./faulty_roger
	docker build --tag=signaloneai/smoked_server:dev ./smoked_server

push-container-blunders:
	docker push signaloneai/broken_bulb:dev
	docker push signaloneai/faulty_roger:dev
	docker push signaloneai/smoked_server:dev
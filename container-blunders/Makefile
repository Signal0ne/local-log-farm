build-container-blunders:
	docker build --tag=signaloneai/broken_bulb:dev ./broken_bulb
	docker build --tag=signaloneai/faulty_roger:dev ./faulty_roger
	docker build --tag=signaloneai/smoked_server:dev ./smoked_server
	docker build --tag=signaloneai/invalid_api_call:dev ./invalid_api_call

push-container-blunders:
	docker push signaloneai/broken_bulb:dev
	docker push signaloneai/faulty_roger:dev
	docker push signaloneai/smoked_server:dev
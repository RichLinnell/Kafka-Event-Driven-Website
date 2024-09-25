# Kafka-Event-Driven-Website
A Blazor website that uses microservices and Kafka events to communicate with a microservice

* Site requires a kafka broker to be running on your local machine
* Settings for the broker and the topic are in the appsettings.json file
* At present - no consumer is built at all, you can just see the messages by running the console consumer `kafka-console-consumer --topic test-topic --from-beginning --bootstrap-server localhost:9092`

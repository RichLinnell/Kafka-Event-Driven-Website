using Confluent.Kafka;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new ConsumerConfig
        {
            GroupId = "trades-microservice-consumer-group",
            BootstrapServers = "localhost:9092",
            AutoOffsetReset = AutoOffsetReset.Earliest,  // Start from the earliest message if no offset is found
            EnableAutoCommit = false,  // We will commit manually after processing
        };

        using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
        {
            // Subscribe to the topic produced by the Blazor app
            consumer.Subscribe("test-topic"); 
            Console.WriteLine("Consuming Kafka messages...");

            try
            {
                while (true)
                {
                    // Consume a message from Kafka
                    var consumeResult = consumer.Consume();

                    // Process the message (add your business logic here)
                    Console.WriteLine($"Received message: {consumeResult.Message.Value}");

                    // Here, you can call any internal logic or microservice business code to process the event
                    await ProcessEvent(consumeResult.Message.Value);

                    // Manually commit the offset after processing
                    consumer.Commit(consumeResult);
                }
            }
            catch (ConsumeException e)
            {
                Console.WriteLine($"Error occurred: {e.Error.Reason}");
            }
            finally
            {
                consumer.Close();
            }
        }
    }

    // Example business logic to process the event from Blazor app
    private static async Task ProcessEvent(string message)
    {
        Console.WriteLine($"Processing event: {message}");

        // Simulate a task with delay
        await Task.Delay(1000);

        var producerService = new KafkaProducerService(); //Modify to use DI
        await producerService.ProduceMessageAsync($"Processed: {message}");
    }
}
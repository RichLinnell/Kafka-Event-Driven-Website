using Confluent.Kafka;
using System;
using System.Threading.Tasks;

public class KafkaProducerService
{
    private readonly string _bootstrapServers = "localhost:9092";  // Change to use config
    private readonly string _responseTopic = "ui-response-events";  // Replace with your topic

    public async Task ProduceMessageAsync(string message)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = _bootstrapServers
        };

        using var producer = new ProducerBuilder<Null, string>(config).Build();
        try
        {
            var result = await producer.ProduceAsync(_responseTopic, new Message<Null, string> { Value = message });
            Console.WriteLine($"Message '{message}' sent to topic '{_responseTopic}' at offset '{result.TopicPartitionOffset}'");
        }
        catch (ProduceException<Null, string> ex)
        {
            Console.WriteLine($"Message delivery failed: {ex.Error.Reason}");
        }
    }
}
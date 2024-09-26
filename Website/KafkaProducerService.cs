using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

public class KafkaProducerService
{
    private readonly string _bootstrapServers;
    private readonly string _topic;

    public KafkaProducerService(IConfiguration configuration)
    {
        _bootstrapServers = configuration["Kafka:BootstrapServers"];
        _topic = configuration["Kafka:Topic"];
        System.Console.WriteLine($"Set up with bootstrapservers : {_bootstrapServers} and topic {_topic}");
    }

    public async Task ProduceMessageAsync(string message)
    {
        Console.WriteLine($"About to sent message : {message}");
        var config = new ProducerConfig
        {
            BootstrapServers = _bootstrapServers,
        };

        using var producer = new ProducerBuilder<Null, string>(config).Build();
        try
        {
            var result = await producer.ProduceAsync(_topic, new Message<Null, string> { Value = message });
            Console.WriteLine($"Delivered '{result.Value}' to '{result.TopicPartitionOffset}'");
        }
        catch (ProduceException<Null, string> ex)
        {
            System.Console.WriteLine($"Delivery Failed {ex.Error.Reason}");
        }
    }
}
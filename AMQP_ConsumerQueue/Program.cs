using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace AMQP_ConsumerQueue
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(   queue: "queueTest", //queue name
                                            durable: false,     //queue persistence during broker restarts
                                            exclusive: false,   //if true makes a queue usable only by its declaring connection
                                            autoDelete: false,  //if true deletes the queu when nobody is subsribed
                                            arguments: null);   //plugins and queue features (ex. message-TTL, queue max-length)

                    var consumer = new EventingBasicConsumer(channel); //creates a new cosnumer instance
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body); // from original bytes sent by the sender to a human-readable string
                        Console.WriteLine(" [x] received {0}", message);
                    };

                    channel.BasicConsume(queue: "queueTest",        //declares where the consumer has to consume
                                            autoAck: true,          //sneds automatic-akcnowledgment
                                            consumer: consumer);    //assignes the consumer

                    Console.WriteLine("press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }
    }
}

using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Interfaces;
using Boilerplate.Infrastructure.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Core.Connections;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Boilerplate.Infrastructure.RabbitMq;
public class RabbitMQService : IRabbitMQService
{


    private readonly RabbitMQConnection _connection;

    public RabbitMQService(RabbitMQConnection connection)
    {
        _connection = connection;
    }

    
    public void PushToHeroQueue(Hero response) {

        var factory = _connection.GetConnection();
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "NewHeroes",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));

            channel.BasicPublish(exchange: "",
                                 routingKey: "NewHeroes",
                                 basicProperties: null,
                                 body: body);
        }
    }


    public void PushToBookQueue(Book response)
    {

        var factory = _connection.GetConnection();
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "NewBooks",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));

            channel.BasicPublish(exchange: "",
                                 routingKey: "NewBooks",
                                 basicProperties: null,
                                 body: body);
        }
    }


}


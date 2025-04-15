using MongoDB.Driver;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Infrastructure.MongoDB;
public class RabbitMQConnection
{
    private readonly string _hostName;
    private readonly string _userName;
    private readonly string _password;
    private readonly int _port;

    public RabbitMQConnection(string hostName, string userName,string password,int port)
    {
      _hostName = hostName;
         _userName = userName;  
        _password = password;
        _port = port;
    }

    public ConnectionFactory GetConnection()
    {
      return new ConnectionFactory() { HostName = _hostName, UserName = _userName, Password = _password , Port= _port };
    }
}

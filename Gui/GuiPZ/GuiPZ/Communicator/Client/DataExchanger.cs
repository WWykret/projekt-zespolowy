using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GuiPZ.Container;
using GuiPZ.MVVM.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using System.Windows.Navigation;

namespace GuiPZ.Communicator.Client;

public class DataExchanger
{
    
    
    private readonly DataContainer _dataContainer;

    private readonly IConnection _connection;
    private IBasicProperties _messageProperties;
    private IModel _channel;
    private ReplyHandler _replyHandler;

    public event Action DataLoaded;
    

    public void GetProfiles()
    {
        SendMessage("GET:PRS");
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _messageProperties.CorrelationId = Guid.NewGuid().ToString();
        _channel.BasicPublish("", "request-queue", _messageProperties, body);
    }
    
    public void InitializeData()
    {
        // _dataContainer.Profiles = new()
        // {
        //     new Profile()
        //     {
        //         Name = "Joe",
        //         Img = new Uri("pack://application:,,,/GuiPz;component/Data/Images/Avatars/Nfd.png")
        //     },
        //     new Profile()
        //     {
        //         Name = "Joel",
        //         Img = new Uri("pack://application:,,,/GuiPZ;component/Data/Images/Avatars/Bakalar.png")
        //     }
        // };
        
        GetProfiles();

        _dataContainer.Avatars = Directory.GetFiles("./Data/Images/Avatars")
            .Select(x => new Avatar() { Img = new Uri(string.Concat("pack://application:,,,/GuiPZ;component", x.AsSpan(1))) })
            .ToList();
    }

    public void SaveData()
    {
        var body = Encoding.UTF8.GetBytes("message");
        _messageProperties.CorrelationId = Guid.NewGuid().ToString();
        _channel.BasicPublish("", "request-queue", _messageProperties, body);
        
        
        _connection.Close();
    }
    
    public DataExchanger(DataContainer dataContainer)
    {
        
        
        _dataContainer = dataContainer;
        _replyHandler = new(_dataContainer);
        
        var factory = new ConnectionFactory {HostName = "localhost"};
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        var replyQueue = _channel.QueueDeclare("", exclusive:true);
        _channel.QueueDeclare("request-queue", exclusive:false);
        var consumer = new EventingBasicConsumer(_channel);
        
        consumer.Received += (model, ea) =>
        {
            _replyHandler.Handle(Encoding.UTF8.GetString(ea.Body.ToArray()));
            DataLoaded?.Invoke();
        };
        _channel.BasicConsume(queue: replyQueue.QueueName, autoAck: true, consumer: consumer);

        _messageProperties = _channel.CreateBasicProperties();
        _messageProperties.ReplyTo = replyQueue.QueueName;
    }
}
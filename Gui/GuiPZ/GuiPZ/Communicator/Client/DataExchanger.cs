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
using System.Threading;
using System.Windows.Automation;
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


    public void SetupData()
    {
        GetComapnies();
        GetProfiles();
    }

    public void GetComapnies()
    {
        SendMessage("GET:CMP");
    }

    public void GetProfiles()
    {
        SendMessage("GET:PRS");
    }

    public void GetImage(Company company)
    {
        SendMessage("GET:IMG:" + company.Name);
    }


    public void CreateNewProfile(Profile profile)
    {
        _dataContainer.Profiles.Add(profile);
        SendNewProfile(profile);
    }

    public void DeleteProfile(Profile profile)
    {
        if (_dataContainer.Profiles.Remove(profile))
            SendDeletedProfile(profile);
    }

    public void AddNewTrackedCompany(Profile profile, string companyName)
    {
        if (
            _dataContainer.Profiles.Any(x => x.Name.Equals(profile.Name)) &&
            _dataContainer.Companies.Any(x => x.Name.Equals(companyName)) &&
            !profile.TrackedCompanies.Any(x => x.Equals(companyName)))
        {
            _dataContainer.Profiles[_dataContainer.Profiles.IndexOf(profile)].TrackedCompanies.Add(companyName);

            SendNewTrackedCompany(profile, companyName);
        }
    }

    public void RemoveTrackedCompany(Profile profile, string companyName)
    {
        if (
            _dataContainer.Profiles.Any(x => x.Name.Equals(profile.Name)) &&
            _dataContainer.Companies.Any(x => x.Name.Equals(companyName)))
        {
            var companies = _dataContainer.Profiles[_dataContainer.Profiles.IndexOf(profile)].TrackedCompanies;
            if (companies.Remove(companyName))
            {
                SendRemovedTrackedCompany(profile, companyName);
            }
        }
    }

    public void SendNewProfile(Profile profile)
    {
        string jsonString = JsonSerializer.Serialize<Profile>(profile);
        SendMessage("SND:NUS:" + jsonString);
    }

    public void SendDeletedProfile(Profile profile)
    {
        SendMessage("SND:DUS:" + profile.Name);
    }

    public void SendNewTrackedCompany(Profile profile, string companyName)
    {
        string profileName = profile.Name;
        SendMessage("SND:NCM:" + profileName + ":" + companyName);
    }

    public void SendRemovedTrackedCompany(Profile profile, string companyName)
    {
        string profileName = profile.Name;
        SendMessage("SND:DCM:" + profileName + ":" + companyName);
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _messageProperties.CorrelationId = Guid.NewGuid().ToString();
        _channel.BasicPublish("", "request-queue", _messageProperties, body);
    }

    public void InitializeData()
    {
        SetupData();
        //
        // Thread.Sleep(1000);
        //
        // GetImage(_dataContainer.Companies[0]);
        //
        // Thread.Sleep(1000);
        // var p3 = new Profile("Adam3", "https://www.google.com/", new List<string>() {"Amazon1"});
        // CreateNewProfile(p3);
        // Thread.Sleep(1000);
        // AddNewTrackedCompany(p3, "Amazon2");
        // Thread.Sleep(1000);
        // AddNewTrackedCompany(p3, "Amazon1");
        // Thread.Sleep(1000);
        // RemoveTrackedCompany(p3, "Amazon1");
        // Thread.Sleep(1000);
        // RemoveTrackedCompany(_dataContainer.Profiles[1], "Amazon1");
        // Thread.Sleep(1000);
        // DeleteProfile(_dataContainer.Profiles[1]);

        _dataContainer.Avatars = Directory.GetFiles("./Data/Images/Avatars")
            .Select(x => new Avatar()
                {Img = new Uri(string.Concat("pack://application:,,,/GuiPZ;component", x.AsSpan(1)))})
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
        var replyQueue = _channel.QueueDeclare("", exclusive: true);
        _channel.QueueDeclare("request-queue", exclusive: false);
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
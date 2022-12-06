// See https://aka.ms/new-console-template for more information
using AMQ.Feeder;
using AMQ.Generator;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", false).Build();

var FILE_COUNT = int.Parse(config["FILE_COUNT"]);
var TOPIC_NAME = config["TOPIC_NAME"];
var BROKER = config["BROKER"];
var SOURCE = config["SOURCE"];
var PER_SECOND_MESSAGES = int.Parse(config["PER_SECOND_MESSAGES"]); ;

var generator = new FilesGenerator();
generator.GenerateFrom(SOURCE, FILE_COUNT);

Console.WriteLine("Hello, World!");

using var publisher = new SimpleTopicPublisher(TOPIC_NAME, BROKER);

var index = 0;
Timer timer;
timer = new Timer(x =>
{
    
    if(index > FILE_COUNT / PER_SECOND_MESSAGES)
    {
        Console.WriteLine("Done sending messages {0} ", DateTime.Now.ToString("T"));
    }
    else
    {
        var count = 0;
        Console.WriteLine("Started sending {0} message {1} ", index + 1, DateTime.Now.ToString("T"));
        while (count < PER_SECOND_MESSAGES)
        {
            count++;
            var destFile = $"templates/replaced_{index * PER_SECOND_MESSAGES + count}.txt";
            
            if (File.Exists(destFile))
            {
                Console.WriteLine("Handling file {0}", destFile);
                var fileContent = File.ReadAllText(destFile);
                publisher.SendMessage(fileContent);
            }
        }
    }
    index++;
}, null, Timeout.Infinite, Timeout.Infinite);

timer.Change(0, 1000);
Console.ReadLine();
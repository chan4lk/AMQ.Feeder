// See https://aka.ms/new-console-template for more information
using AMQ.Feeder;
using AMQ.Generator;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", false).Build();

var fileCount = int.Parse(config["FILE_COUNT"]);
var generator = new FilesGenerator();
generator.GenerateFrom(config["SOURCE"], fileCount);

Console.WriteLine("Hello, World!");
string TOPIC_NAME = config["TOPIC_NAME"];
string BROKER = config["BROKER"];

using var publisher = new SimpleTopicPublisher(TOPIC_NAME, BROKER);

var perSecondMessages = int.Parse(config["PER_SECOND_MESSAGES"]); ;
var index = 0;
Timer timer;
timer = new Timer(x =>
{
    
    if(index > fileCount / perSecondMessages)
    {
        Console.WriteLine("Done sending messages {0} ", DateTime.Now.ToString("T"));
    }
    else
    {
        var count = 0;
        Console.WriteLine("Started sending {0} messages {1} ", fileCount / perSecondMessages, DateTime.Now.ToString("T"));
        while (count < perSecondMessages)
        {
            count++;
            var destFile = $"templates/replaced_{index * perSecondMessages + count}.txt";
            
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
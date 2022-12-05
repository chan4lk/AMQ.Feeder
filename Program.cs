// See https://aka.ms/new-console-template for more information
using AMQ.Feeder;
using AMQ.Generator;

var fileCount = 100;
var generator = new FilesGenerator();
generator.GenerateFrom("templates/message.txt", fileCount);

Console.WriteLine("Hello, World!");
const string TOPIC_NAME = "srk.topic.test.journey";
const string BROKER = "tcp://localhost:61616";

using var publisher = new SimpleTopicPublisher(TOPIC_NAME, BROKER);

var perSecondMessages = 27;
var index = 0;
Timer timer;
timer = new Timer(x =>
{
    
    if(index >= fileCount / perSecondMessages)
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
            Console.WriteLine("Handling file {0}", destFile);

            var fileContent = File.ReadAllText(destFile);
            publisher.SendMessage(fileContent);
        }
    }
    index++;
}, null, Timeout.Infinite, Timeout.Infinite);

timer.Change(0, 1000);
Console.ReadLine();
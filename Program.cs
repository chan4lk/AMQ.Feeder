// See https://aka.ms/new-console-template for more information
using AMQ.Feeder;

Console.WriteLine("Hello, World!");
const string TOPIC_NAME = "srk.topic.journey";
const string BROKER = "tcp://localhost:61616";

var publisher = new SimpleTopicPublisher(TOPIC_NAME, BROKER);

var perSecondMessages = 27;
var delay = 1000/perSecondMessages;
var count = 0;
Console.WriteLine("Started sending {0} messages {1} ", 1000 / perSecondMessages, DateTime.Now.ToString("T"));
while (true)
{
    count++;
    publisher.SendMessage($"<test>Message {count}</test>");
    Thread.Sleep(delay);

    if (count >= perSecondMessages ) break;
}
Console.WriteLine("Done sending messages {0} ", DateTime.Now.ToString("T"));
Console.ReadLine();
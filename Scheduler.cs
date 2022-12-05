namespace AMQ.Feeder
{
    internal class Scheduler
    {
        private List<Timer> timers = new List<Timer>();

        public void Schedule(Action task)
        { 
            var timer = new Timer(x =>
            {                
                task.Invoke();
            }, null, 10, 1);

            timers.Add(timer);
        }
    }
}

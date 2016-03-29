using System.Timers;
namespace BuildingBlocks.Timers
{
    public static class GeneralTimer
    {
        public static void Wait(int milliseconds)
        {
            Timer timer = new Timer(milliseconds);
            timer.Elapsed += new ElapsedEventHandler(TimeElapsed);
            timer.Start();
            while (timer.Enabled)
            {
                //This is valid as it is used a processor saver to give the
                //timer the necessary "time" to actually elapse else we'll kill the processing while waiting
                System.Threading.Thread.Sleep(milliseconds);
            }
        }

        private static int tries;

        public static void WaitFor<T>(int intervalsInMilliseconds, int noTries, T valueToWaitFor, System.Predicate<T> predicate)
        {
            tries = noTries;
            for (int tryCount = 0; tryCount < noTries; tryCount++)
            {
                if (predicate.Invoke(valueToWaitFor) != true)
                {
                    //This is valid as it is used a processor saver to give the
                    //timer the necessary "time" to actually elapse else we'll kill the processing while waiting
                    System.Threading.Thread.Sleep(intervalsInMilliseconds);
                }
                else
                {
                    break;
                }
            }
        }

        public static void BlockWaitFor(int intervalsInMilliseconds, int noTries, System.Func<bool> predicate, System.Action timeOutAction = null)
        {
            tries = noTries;
            var isTimeOut = true;
            for (int tryCount = 0; tryCount < noTries; tryCount++)
            {
                //This is valid as it is used a processor saver to give the
                //timer the necessary "time" to actually elapse else we'll kill the processing while waiting
                System.Threading.Thread.Sleep(intervalsInMilliseconds);
                if (predicate.Invoke())
                {
                    isTimeOut = false;
                    break;
                }
            }
            if (isTimeOut && timeOutAction != null)
                timeOutAction.Invoke();
        }

        #region Events

        private static void TimeElapsed(object sender, ElapsedEventArgs e)
        {
            Timer timer = sender as Timer;
            timer.Stop();
        }

        #endregion Events
    }
}
namespace TimerService
{
    public abstract class Timer : ITimer
    {
        private readonly System.Threading.Timer t;
        private double setTime = 10;

        public Timer()
        {
            t = new(new(Action));
            t.Change(GetMsUntilFour(setTime), Timeout.Infinite);
        }
        
        private void Action(object? state)
        {
            MyAction();
            t.Change(GetMsUntilFour(setTime), Timeout.Infinite);
        }

        /// <summary>
        /// 計時器結束時要執行的動作
        /// </summary>
        public abstract void MyAction();

        /// <summary>
        /// 以秒為單位設定計時器週期，預設10秒
        /// </summary>
        /// <param name="time"></param>
        public void time(double time) => setTime = time;
        private int GetMsUntilFour(double time)
        {
            DateTime now = DateTime.Now;
            DateTime oneOClock = DateTime.Now.AddSeconds(time);
            if (now > oneOClock)
                oneOClock = oneOClock.AddSeconds(time);
            return (int)(oneOClock - now).TotalMilliseconds;
        }
    }
}
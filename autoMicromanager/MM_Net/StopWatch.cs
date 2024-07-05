// DESCRIPTION:   
//                 
//                
// AUTHOR:        Brian Ashcroft, Ashcroft@physics.leidenuniv.nl, 05/27/2009
//
// COPYRIGHT:     Brian Ashcroft
// LICENSE:       This file is distributed under the  MIT license.
//                License text is included with the source distribution.
//
//                This file is distributed in the hope that it will be useful,
//                but WITHOUT ANY WARRANTY; without even the implied warranty
//                of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
//
//                IN NO EVENT SHALL THE COPYRIGHT OWNER OR
//                CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
//                INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace CoreDevices
{
    public delegate void StopWatchTickEvent(object sender,long millisec, int TickIndex);

    [Serializable]
    public class StopWatch
    {
        System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
        public event StopWatchTickEvent StopWatchTick;
        private bool PauseStopWatchTicking = true;


        private System.Windows.Forms.Timer MainIntervalTimer;
        private System.Windows.Forms.Timer SecondIntervalTimer;

        /// <summary>
        /// This will produce a tick every interval of time and start the stopwatch.
        /// 
        /// </summary>
        /// <param name="IntervalMS">This is the main interval that will be ticked</param>
        /// <param name="SubTickInterval">This is a sub interval and should be set as an offset from the main
        /// interval.  I.e.  If interval clicks at every 1 second, but at 1.2 and 2.2 and 3.2 you also want a click then set this parameter to 200</param>
        public void StartTimeTicks(EasyCore eCore, long IntervalMS, long SubTickInterval)
        {
            if (MainIntervalTimer != null) MainIntervalTimer.Stop();
            if (SecondIntervalTimer != null) SecondIntervalTimer.Stop();

            MainIntervalTimer = new System.Windows.Forms.Timer();
            SecondIntervalTimer = new System.Windows.Forms.Timer();
            MainIntervalTimer.Tick += new EventHandler(MainIntervalTimer_Tick);
            SecondIntervalTimer.Tick += new EventHandler(SecondIntervalTimer_Tick);

            MainIntervalTimer.Interval=(int)IntervalMS;
            SecondIntervalTimer.Interval =(int) SubTickInterval;
            stopWatch.Start();
            MainIntervalTimer.Start();
            SecondIntervalTimer.Start();
        }

        void SecondIntervalTimer_Tick(object sender, EventArgs e)
        {
            long Curtime = GetStopWatchInterval();
            //Thread DoTick2 = new Thread(delegate()
            //{
                if (StopWatchTick != null) StopWatchTick(this, Curtime, 1);
            //});
            //DoTick2.Start();
            SecondIntervalTimer.Stop();
        }
        void MainIntervalTimer_Tick(object sender, EventArgs e)
        {
            
            long  Curtime = GetStopWatchInterval();
            SecondIntervalTimer.Start();
            //Thread DoTick = new Thread(delegate()
            //{
                if (StopWatchTick != null) StopWatchTick(this, Curtime, 0);
            //});
            //DoTick.Start();
           
        }

      
        #region HiResTimers
        /// <summary>
        /// This will produce a tick every interval of time and start the stopwatch.
        /// 
        /// </summary>
        /// <param name="IntervalMS">This is the main interval that will be ticked</param>
        /// <param name="SubTickInterval">This is a sub interval and should be set as an offset from the main
        /// interval.  I.e.  If interval clicks at every 1 second, but at 1.2 and 2.2 and 3.2 you also want a click then set this parameter to 200</param>
        public void StartTimeTicksHIRes(EasyCore eCore, long IntervalMS, long SubTickInterval)
        {
            PauseStopWatchTicking = false;
            Thread Monitor = new Thread(delegate()
            {

                long NextClick = IntervalMS;
                while (!PauseStopWatchTicking)
                {
                    long Curtime = GetStopWatchInterval();
                    int SleepTimeMS = (int)(NextClick - Curtime - 100);
                    if (SleepTimeMS < 0) SleepTimeMS = 1;
                    Thread.Sleep(SleepTimeMS );
                    Curtime = GetStopWatchInterval();
                    while (Curtime <= NextClick)
                    {
                        Thread.Sleep(2);
                        Curtime = GetStopWatchInterval();
                    }
                    Curtime = GetStopWatchInterval();
                    Thread DoTick = new Thread(delegate()
                        {
                            if (StopWatchTick != null) StopWatchTick(this, Curtime, 0);
                        });
                    DoTick.Start();
                    SleepTimeMS =(int)(NextClick + SubTickInterval - Curtime - 100);
                    if (SleepTimeMS <0) SleepTimeMS =1;
                    if (SleepTimeMS <100) SleepTimeMS =1;
                    Thread.Sleep(SleepTimeMS );
                    Curtime = GetStopWatchInterval();
                    while (Curtime <= NextClick+SubTickInterval )
                    {
                        Thread.Sleep(2);
                        Curtime = GetStopWatchInterval();
                    }
                    Curtime = GetStopWatchInterval();
                    Thread DoTick2 = new Thread(delegate()
                        {
                            if (StopWatchTick != null) StopWatchTick(this, Curtime, 1);
                        });
                    DoTick2.Start();
                    NextClick += IntervalMS;
                }

            });
            eCore.AddThreadToPool(Monitor);
            stopWatch.Start();
            Monitor.Start();
        }

        
        /// <summary>
        /// This will produce a tick every interval of time and start the stopwatch.
        /// 
        /// </summary>
        /// <param name="IntervalMS">This is the main interval that will be ticked</param>
        /// 
        public void StartTimeTicks(EasyCore ecore, long IntervalMS )
        {
            PauseStopWatchTicking = false;
            Thread Monitor = new Thread(delegate()
            {

                long NextClick = IntervalMS ;
                while (!PauseStopWatchTicking)
                {
                    long Curtime = GetStopWatchInterval();
                    Thread.Sleep((int)(NextClick - Curtime  -100));
                    Curtime = GetStopWatchInterval();
                    while (Curtime < NextClick)
                    {
                        Thread.Sleep(2);
                        Curtime = GetStopWatchInterval();
                    }
                    Curtime = GetStopWatchInterval();
                    Thread DoTick = new Thread(delegate()
                    {
                        if (StopWatchTick != null) StopWatchTick(this, Curtime, 0);
                    });
                    DoTick.Start();
                    NextClick += IntervalMS; 
                }

            });
            ecore.AddThreadToPool(Monitor);
            stopWatch.Start();
            Monitor.Start();
        }
        #endregion
        public void StartStopWatch()
        {
            stopWatch.Start();
        }
        public void StopStopWatch()
        {
            PauseStopWatchTicking = true;
            if (MainIntervalTimer!=null)  MainIntervalTimer.Stop();
            if (SecondIntervalTimer!=null)  SecondIntervalTimer.Stop();
            stopWatch.Stop();
        }
        public double GetStopWatchValue()
        {
            return (double)stopWatch.ElapsedMilliseconds / 1000;
        }
        public long GetStopWatchInterval()
        {
            return stopWatch.ElapsedMilliseconds;
        }


    }
}

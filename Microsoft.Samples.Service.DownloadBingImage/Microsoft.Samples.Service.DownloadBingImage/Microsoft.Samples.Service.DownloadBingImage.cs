//-----------------------------------------------------------------------
// <copyright file="DownloadBingImage.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// <author>John Bailey (johnbai)</author>
//-----------------------------------------------------------------------
namespace Microsoft.Samples.Service.DownloadBingImage
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.ServiceProcess;
    using System.Threading;
    using HttpMethods;
    using Types;

    /// <summary>
    ///     This service obtains the Json Data from the Bing API endpoint for downloading the Bing Image[s] that are used on the Search homepage.
    ///     In cases of first run (e.g.: when the service first starts), the service downloads the last 15 images.
    /// </summary>
    public partial class DownloadBingImage : ServiceBase
    {
        /// <summary>
        ///     AutoResetEvent is used to control the timer's behavior; specifically, it signals if the timer should iterate again after signaling to the callback.
        ///     It is also used to close the timer.
        ///     For more information on the AutoResetEvent, see: https://msdn.microsoft.com/en-us/library/system.threading.autoresetevent(v=vs.110).aspx
        /// </summary>
        private static readonly AutoResetEvent ServiceJsonTimerAutoResetEvent = new AutoResetEvent(true);

        /// <summary>
        ///     The TimerCallback references the method to invoke, when the timer completes it's iteration.
        ///     For more information on the TimerCallback, see: https://msdn.microsoft.com/en-us/library/system.threading.timercallback(v=vs.110).aspx
        /// </summary>
        private static readonly TimerCallback ServiceJsonTimerCallback = QueueJsonWorkItem;

        /// <summary>
        ///     The actual Timer used to signal work to be done.
        ///     Note that the timer has an initial offset and an interval with which to run.
        ///     It also uses the <see cref="ServiceJsonTimerCallback"/> on signal.
        ///     For more information on the System.Threading.Timer, see: https://msdn.microsoft.com/en-us/library/2x96zfy7(v=vs.110).aspx
        /// </summary>
        private static readonly Timer ServiceJsonTimer = new Timer(ServiceJsonTimerCallback, ServiceJsonTimerAutoResetEvent, (Int64)TimeSpan.FromSeconds(Int64.Parse(ConfigurationManager.AppSettings["InitialTimerValue"])).TotalMilliseconds, (Int64)TimeSpan.FromHours(Int64.Parse(ConfigurationManager.AppSettings["IntervalTimerValue"])).TotalMilliseconds);

        /// <summary>
        ///     Initializes a SemaphoreSlim, which we use to control thread queues and prevent collision in GDI+.
        ///     Otherwise, we receive a generic, non-descript "A generic error occurred in GDI+." exception.
        ///     The integer value is controlled via the App.Config file.
        ///     For more information on SemaphoreSlime, see: https://msdn.microsoft.com/en-us/library/dd270887(v=vs.110).aspx
        /// </summary>
        public static readonly SemaphoreSlim ServiceSemaphoreSlim = new SemaphoreSlim(int.Parse(ConfigurationManager.AppSettings["SemaphoreLimit"]));

        /// <summary>
        ///     Entry point for the service.
        /// </summary>
        public DownloadBingImage()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Object instantiation on service start.
        /// </summary>
        /// <param name="args"></param>
        protected sealed override void OnStart(string[] args)
        {
            Objects.FirstRun = true;
            Objects.ReturnedJsonData = new JsonData();

            // For more information on the DateTime.Now property see: https://msdn.microsoft.com/en-us/library/system.datetime.now(v=vs.110).aspx
            // For more information on the DateTime.AddMinutes(Double) see: https://msdn.microsoft.com/en-us/library/system.datetime.addminutes(v=vs.110).aspx
            Objects.CheckDateTime = DateTime.Now.AddMinutes(30);

            // Set the Maximum ThreadPool limit.
            // For more information on the ThreadPool.SetMaxThreads(Int32, Int32) method see: https://msdn.microsoft.com/en-us/library/system.threading.threadpool.setmaxthreads(v=vs.110).aspx
            ThreadPool.SetMaxThreads(int.Parse(ConfigurationManager.AppSettings["MaxWorkerThreads"]), int.Parse(ConfigurationManager.AppSettings["MaxCompletionPortThreads"]));

            // Set the Minimum ThreadPool limit.
            // For more information on the ThreadPool.SetMinThreads(Int32, Int32) method see: https://msdn.microsoft.com/en-us/library/system.threading.threadpool.setminthreads(v=vs.110).aspx
            ThreadPool.SetMinThreads(int.Parse(ConfigurationManager.AppSettings["MinWorkerThreads"]), int.Parse(ConfigurationManager.AppSettings["MinCompletionPortThreads"]));
        }

        /// <summary>
        ///     Tasks and object destruction on service stop.
        /// </summary>
        protected override void OnStop()
        {
            // For more information on the EventWaitHandle.Reset() method see: https://msdn.microsoft.com/en-us/library/system.threading.eventwaithandle.reset(v=vs.110).aspx
            ServiceJsonTimerAutoResetEvent.Reset();

            // For more information on the Timer.Change(TimeSpan, TimeSpan) method see: https://msdn.microsoft.com/en-us/library/317hx6fa(v=vs.110).aspx
            // For more information on the Timeout.InfiniteTimeSpan field see: https://msdn.microsoft.com/en-us/library/system.threading.timeout.infinitetimespan(v=vs.110).aspx
            ServiceJsonTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);

            // For more information on the WaitHandle.WaitAll(WaitHandle[], TimeSpan) method see: https://msdn.microsoft.com/en-us/library/cc190864(v=vs.110).aspx
            WaitHandle.WaitAll(new WaitHandle[] {ServiceJsonTimerAutoResetEvent}, TimeSpan.FromSeconds(10));

            // For more information on the WaitHandle.Close() see:https://msdn.microsoft.com/en-us/library/system.threading.waithandle.close(v=vs.110).aspx
            ServiceJsonTimerAutoResetEvent.Close();

            // For more information on the WaitHandle.Dispose() method see: https://msdn.microsoft.com/en-us/library/dd384809(v=vs.110).aspx
            ServiceJsonTimerAutoResetEvent.Dispose();

            // For more information on the Timer.Dispose() method see: https://msdn.microsoft.com/en-us/library/zb0225y6(v=vs.110).aspx
            ServiceJsonTimer.Dispose(/* Cannot point to the WaitHandle because it's been disposed of at this point */);
        }

        /// <summary>
        ///     Queues the item to the thread pool to be dispatched via thread.
        /// </summary>
        /// <param name="stateInfo"></param>
        private static void QueueJsonWorkItem(Object stateInfo)
        {
            // We evaluate if this is the first time the service has run in this instance.
            if (Objects.FirstRun)
            {
                // The first time that the service is called, so we run.
                GetBingJsonData();
            }
            // We validate if we've hit the new time to process.
            if (DateTime.Now >= Objects.CheckDateTime)
            {
                GetBingJsonData();
            }

            // Do nothing.
        }

        /// <summary>
        ///     Obtains the JSON data from the Bing API endpoint.
        ///     Creates a request to download the photo[s] per item retured.
        /// </summary>
        private static async void GetBingJsonData()
        {
            // We must await completion before we can continue.
            // For more information on the await operator see: https://msdn.microsoft.com/en-us/library/hh156528.aspx
            await Methods.GetReturnedJsonData(ServiceSemaphoreSlim);

            // So we don't reference 'null', we start the download here - after awaiting the above.
            // For more information on the Enumerable.ToList<TSource>(IEnumerable<TSource>) method see: https://msdn.microsoft.com/en-us/library/bb342261(v=vs.110).aspx
            // For more information on the List<T>.ForEach(Action<T>) method see: https://msdn.microsoft.com/en-us/library/bwabdf9z(v=vs.110).aspx
            // For more information on the ThreadPool.QueueUserWorkItem(WaitCallback) method see: https://msdn.microsoft.com/en-us/library/kbf0f1ct(v=vs.110).aspx
            Objects.ReturnedJsonData.Image.ToList().ForEach(x => ThreadPool.QueueUserWorkItem(o => DownloadImageData(x.Url, ServiceSemaphoreSlim)));
        }

        /// <summary>
        ///     Downloads the picture data.
        /// </summary>
        /// <param name="targetUri">URL to target for the download.</param>
        /// <param name="passedSemaphoreSlim">Semaphore used to prevent contention/thrashing.</param>
        private static void DownloadImageData(string targetUri, SemaphoreSlim passedSemaphoreSlim)
        {
            Methods.DownloadAndWriteImageStream(targetUri, passedSemaphoreSlim);
        }
    }
}
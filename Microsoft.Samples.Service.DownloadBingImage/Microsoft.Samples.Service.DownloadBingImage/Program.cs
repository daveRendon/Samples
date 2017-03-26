//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// <author>John Bailey (johnbai)</author>
//-----------------------------------------------------------------------
namespace Microsoft.Samples.Service.DownloadBingImage
{
    using System.ServiceProcess;

    /// <summary>
    ///     Defines the program.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] servicesToRun;
            servicesToRun = new ServiceBase[]
            {
                new DownloadBingImage
                {
                    ServiceName = "Microsoft.EnterpriseCloud.Service.DownloadBingImage",
                    AutoLog = true,
                    EventLog = {Source = "Microsoft.EnterpriseCloud.Service.DownloadBingImage", Log = "Application"}
                },
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
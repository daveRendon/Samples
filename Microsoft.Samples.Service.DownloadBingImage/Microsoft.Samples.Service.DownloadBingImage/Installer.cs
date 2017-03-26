//-----------------------------------------------------------------------
// <copyright file="Installer.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// <author>John Bailey (johnbai)</author>
//-----------------------------------------------------------------------
namespace Microsoft.Samples.Service.DownloadBingImage
{
    using System.ComponentModel;
    using System.ServiceProcess;

    /// <summary>
    ///     Installer class is used by InstallUtil to install the service.
    ///     From Elevated Prompt (Command or PowerShell) run 'InstallUtil' from C:\Windows\Microsoft.NET\Framework64\v4.0.30319
    ///     and include the path to the built exe to install this service.
    /// </summary>
    [RunInstaller(true)]
    public class Installer : System.Configuration.Install.Installer
    {
        /// <summary>
        ///     Entry point for the <see cref="Installer"/>.
        /// </summary>
        public Installer()
        {
            // Instantiate installers for process and services.
            ServiceProcessInstaller processInstaller = new ServiceProcessInstaller();
            ServiceInstaller serviceInstaller = new ServiceInstaller();

            // The services run under the local system account.
            processInstaller.Account = ServiceAccount.LocalSystem;

            // The services are started manually.
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            // ServiceName must equal those on ServiceBase derived classes.
            serviceInstaller.ServiceName = "Microsoft.Samples.Service.DownloadBingImage";
            serviceInstaller.DisplayName = "Microsoft.Samples.Service.DownloadBingImage";
            serviceInstaller.Description = "Service used to Download the Daily Bing Image.";

            // Add installers to collection. Order is not important.
            Installers.Add(serviceInstaller);
            Installers.Add(processInstaller);
        }
    }
}
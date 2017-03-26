//-----------------------------------------------------------------------
// <copyright file="Strings.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// <author>John Bailey (johnbai)</author>
//-----------------------------------------------------------------------
namespace Microsoft.Samples.Service.DownloadBingImage
{
    using System;
    using Types;

    /// <summary>
    ///     Contains strings used during the service's lifetime.
    /// </summary>
    public class Objects
    {
        /// <summary>
        ///     Gets or sets the <see cref="FirstRun"/> boolean, 
        ///     which determines if this is the first run of the service.
        /// </summary>
        public static bool FirstRun { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="BingUrl"/> URI.
        /// </summary>
        public static Uri BingUrl { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="CheckDateTime"/> DateTime,
        ///     which is used to determine when we should check again.
        /// </summary>
        public static DateTime CheckDateTime { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="ReturnedJsonData"/> JsonData,
        ///     which is returned from the Bing API endpoint.
        /// </summary>
        public static JsonData ReturnedJsonData { get; set; }
    }
}
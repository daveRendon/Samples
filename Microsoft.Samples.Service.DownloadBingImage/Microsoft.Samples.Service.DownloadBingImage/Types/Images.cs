//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// <author>John Bailey (johnbai)</author>
//-----------------------------------------------------------------------
namespace Microsoft.Samples.Service.DownloadBingImage.Types
{
    using Newtonsoft.Json;

    /// <summary>
    ///     Class representing the Json Data returned from the Bing API.
    /// </summary>
    public class Images
    {
        /// <summary>
        ///     Gets or sets the <see cref="StartDate"/> property.
        /// </summary>
        [JsonProperty("startdate")]
        public  string StartDate { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="FullStartDate"/> property.
        /// </summary>
        [JsonProperty("fullstartdate")]
        public  string FullStartDate { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="EndDate"/> property.
        /// </summary>
        [JsonProperty("enddate")]
        public string EndDate { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="Url"/> property.
        /// </summary>
        [JsonProperty("url")]
        public  string Url { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="UrlBase"/> property.
        /// </summary>
        [JsonProperty("urlbase")]
        public  string UrlBase { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="CopyRight"/> property.
        /// </summary>
        [JsonProperty("copyright")]
        public  string CopyRight { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="CopyRightLink"/> property.
        /// </summary>
        [JsonProperty("copyrightlink")]
        public  string CopyRightLink { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="Quiz"/> property.
        /// </summary>
        [JsonProperty("quiz")]
        public  string Quiz { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="CopyRightSource"/> property.
        /// </summary>
        [JsonProperty("copyrightsource")]
        public  string CopyRightSource { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="Wp"/> property.
        /// </summary>
        [JsonProperty("wp")]
        public  string Wp { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="Hsh"/> property.
        /// </summary>
        [JsonProperty("hsh")]
        public  string Hsh { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="Drk"/> property.
        /// </summary>
        [JsonProperty("drk")]
        public  string Drk { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="Top"/> property.
        /// </summary>
        [JsonProperty("top")]
        public  string Top { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="Bot"/> property.
        /// </summary>
        [JsonProperty("bot")]
        public  string Bot { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="Hs"/> property.
        /// </summary>
        [JsonProperty("hs")]
        public  string[] Hs { get; set; }
    }
}
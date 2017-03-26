//-----------------------------------------------------------------------
// <copyright file="Tooltip.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// <author>John Bailey (johnbai)</author>
//-----------------------------------------------------------------------
namespace Microsoft.Samples.Service.DownloadBingImage.Types
{
    using Newtonsoft.Json;

    /// <summary>
    ///     Class containing the properties for the <see cref="ToolTip"/>
    /// </summary>
    public class ToolTip
    {
        /// <summary>
        ///     Gets or sets the <see cref="Loading"/> property.
        /// </summary>
        [JsonProperty("loading")]
        public static string Loading { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="Previous"/> property.
        /// </summary>
        [JsonProperty("previous")]
        public static string Previous { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="Next"/> property.
        /// </summary>
        [JsonProperty("next")]
        public static string Next { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="WallE"/> property.
        /// </summary>
        [JsonProperty("walle")]
        public static string WallE { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="WallS"/> property.
        /// </summary>
        [JsonProperty("walls")]
        public static string WallS { get; set; }
    }
}
//-----------------------------------------------------------------------
// <copyright file="JsonData.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// <author>John Bailey (johnbai)</author>
//-----------------------------------------------------------------------
namespace Microsoft.Samples.Service.DownloadBingImage.Types
{
    using Newtonsoft.Json;

    /// <summary>
    ///     A class containing <see cref="Images"/> and <see cref="ToolTip"/> objects.
    /// </summary>
    public class JsonData
    {
        /// <summary>
        ///     Gets or sets the <see cref="Image"/> property.
        /// </summary>
        [JsonProperty("images")]
        public Images[] Image { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="ToolTips"/> property.
        /// </summary>
        [JsonProperty("tooltips")]
        public ToolTip ToolTips { get; set; }
    }
}
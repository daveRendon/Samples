//-----------------------------------------------------------------------
// <copyright file="Designer.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// <author>John Bailey (johnbai)</author>
//-----------------------------------------------------------------------
namespace Microsoft.Samples.Service.DownloadBingImage
{
    /// <summary>
    ///     Designer Class for the <see cref="DownloadBingImage"/> Service.
    /// </summary>
    partial class DownloadBingImage
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.ServiceName = "Service1";
        }

        #endregion
    }
}

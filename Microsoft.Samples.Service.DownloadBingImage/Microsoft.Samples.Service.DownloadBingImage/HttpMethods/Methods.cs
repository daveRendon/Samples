//-----------------------------------------------------------------------
// <copyright file="Methods.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// <author>John Bailey (johnbai)</author>
//-----------------------------------------------------------------------
namespace Microsoft.Samples.Service.DownloadBingImage.HttpMethods
{
    using System;
    using System.Configuration;
    using System.Drawing;
    using System.IO;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Types;

    /// <summary>
    ///     Contains the methods to perform the HTTP work.
    /// </summary>
    internal class Methods
    {
        /// <summary>
        ///     Uses an HttpClient to download the JSON data from Bing.
        /// </summary>
        public static async Task GetReturnedJsonData(SemaphoreSlim passedSemaphoreSlim)
        {
            // For more information on the SemaphoreSlim.Wait() method see: https://msdn.microsoft.com/en-us/library/dd270787(v=vs.110).aspx
            passedSemaphoreSlim.Wait();

            // Since HttpClient is IDisposable, we wrap in a 'using' statement.
            // For more information on the HttpClient class see: https://msdn.microsoft.com/en-us/library/system.net.http.httpclient(v=vs.118).aspx
            // Fore more information on the using statement see: https://msdn.microsoft.com/en-us/library/yh598w02.aspx
            using (HttpClient newHttpClient = new HttpClient())
            {
                // Add the header to distinguish our requests.
                // For more information on the DefaultRequestHeaders property see: https://msdn.microsoft.com/en-us/library/system.net.http.httpclient.defaultrequestheaders(v=vs.118).aspx
                // For more information on the HttpRequestHeaders.UserAgent property see: https://msdn.microsoft.com/en-us/library/system.net.http.headers.httprequestheaders.useragent(v=vs.118).aspx
                // For more information on the HttpHeaderValueCollection<T>.ParseAdd(string) method see: https://msdn.microsoft.com/en-us/library/hh158968(v=vs.118).aspx
                // For more information on the ConfigurationManager.AppSettings property see: https://msdn.microsoft.com/en-us/library/system.configuration.configurationmanager.appsettings%28v=vs.110%29.aspx
                newHttpClient.DefaultRequestHeaders.UserAgent.ParseAdd(ConfigurationManager.AppSettings["UserAgent"]);

                // Wrap in a try so we can log the exception.
                try
                {
                    // We set the URI, depending on if this is the first run of the service or not.
                    // For more information on the URI class see: https://msdn.microsoft.com/en-us/library/system.uri(v=vs.110).aspx
                    // For more information on the ConfigurationManager.AppSettings property see: https://msdn.microsoft.com/en-us/library/system.configuration.configurationmanager.appsettings%28v=vs.110%29.aspx
                    Objects.BingUrl = Objects.FirstRun ? new Uri(string.Format(ConfigurationManager.AppSettings["BingApiUrl"], "15", "15", ConfigurationManager.AppSettings["BingApiMarket"])) : new Uri(string.Format(ConfigurationManager.AppSettings["BingApiUrl"], "0", "1", ConfigurationManager.AppSettings["BingApiMarket"]));

                    // Since HttpResponseMessage is IDisposable, we wrap in a 'using' statement.
                    // For more information on the HttpResponseMessage class see: https://msdn.microsoft.com/en-us/library/system.net.http.httpresponsemessage(v=vs.118).aspx
                    // For more information on the HttpClient.GetAsync(Uri) method see:https://msdn.microsoft.com/en-us/library/hh158912(v=vs.118).aspx
                    using (HttpResponseMessage newHttpResponseMessage = await newHttpClient.GetAsync(Objects.BingUrl))
                    {
                        // For more information on the HttpResponseMessage.Content property see: https://msdn.microsoft.com/en-us/library/system.net.http.httpresponsemessage.content(v=vs.118).aspx
                        // For more information on the HttpContent.ReadAsStringAsync() method see: https://msdn.microsoft.com/en-us/library/system.net.http.httpcontent.readasstringasync(v=vs.118).aspx
                        // For more information on the await operator see: https://msdn.microsoft.com/en-us/library/hh156528.aspx
                        string responseMessageStream = await newHttpResponseMessage.Content.ReadAsStringAsync();

                        // For more information on the JsonConvert.DeserializeObject<T>(string) method see: http://www.newtonsoft.com/json/help/html/M_Newtonsoft_Json_JsonConvert_DeserializeObject__1.htm
                        Objects.ReturnedJsonData = JsonConvert.DeserializeObject<JsonData>(responseMessageStream);
                    }

                    // We tell the service to retry later.
                    // For more information on the DateTime.AddDays(Double) method see: https://msdn.microsoft.com/en-us/library/system.datetime.adddays%28v=vs.110%29.aspx
                    Objects.CheckDateTime = DateTime.Now.AddDays(1);
                }
                catch (HttpRequestException ex)
                {
                    // Probably a client-side or server-side connection issue that will be transient.
                    // Since the exception is coming from the thread that is in the thread pool, it will be aggregated (read: wrapped), 
                    // so we need the inner exception to see what the issue is.
                    if (ex.InnerException != null)
                    {
                        string exception = ex.InnerException.Message;

                        // For more information on the TextWriter class see: https://msdn.microsoft.com/en-us/library/system.io.textwriter(v=vs.110).aspx
                        // For more information on the StreamWriter class see: https://msdn.microsoft.com/en-us/library/system.io.streamwriter(v=vs.110).aspx
                        // For more information on the ConfigurationManager.AppSettings property see: https://msdn.microsoft.com/en-us/library/system.configuration.configurationmanager.appsettings%28v=vs.110%29.aspx
                        using (TextWriter newTextWriter = new StreamWriter(ConfigurationManager.AppSettings["ServiceLogPath"], true /* Append to the file */))
                        {
                            // For more information on the TextWriter.Synchronized(TextWriter) method see: https://msdn.microsoft.com/en-us/library/system.io.textwriter.synchronized(v=vs.110).aspx
                            TextWriter.Synchronized(newTextWriter);

                            // For more information on the TextWriter.Write(String) method see: https://msdn.microsoft.com/en-us/library/cay30k2f(v=vs.110).aspx
                            newTextWriter.Write(exception);

                            // If we don't close, a handle will be kept on the file, so the next iteration will throw an exception.
                            // For more information on the TextWriter.Close() method see: https://msdn.microsoft.com/en-us/library/system.io.textwriter.close(v=vs.110).aspx
                            newTextWriter.Close();
                        }
                    }

                    // We tell the service to retry later.
                    // For more information on the DateTime.AddDays(Double) method see: https://msdn.microsoft.com/en-us/library/system.datetime.adddays%28v=vs.110%29.aspx
                    Objects.CheckDateTime = DateTime.Now.AddDays(1);
                }
            }

            // For more information on the SemaphoreSlim.Release(Int32) method see: https://msdn.microsoft.com/en-us/library/dd289587(v=vs.110).aspx
            passedSemaphoreSlim.Release(1);
        }

        /// <summary>
        ///     Uses an HttpClient to download the picture data and saves it to the Temp folder.
        /// </summary>
        /// <param name="targetUri">URI part that is returned in the JSON response.</param>
        /// <param name="passedSemaphoreSlim"></param>
        public static async void DownloadAndWriteImageStream(string targetUri, SemaphoreSlim passedSemaphoreSlim)
        {
            // For more information on the SemaphoreSlim.Wait() method see: https://msdn.microsoft.com/en-us/library/dd270787(v=vs.110).aspx
            passedSemaphoreSlim.Wait();

            // Since HttpClient is IDisposable, we wrap in a 'using' statement.
            // For more information on the HttpClient class see: https://msdn.microsoft.com/en-us/library/system.net.http.httpclient(v=vs.118).aspx
            // Fore more information on the using statement see: https://msdn.microsoft.com/en-us/library/yh598w02.aspx
            using (HttpClient imageHttpClient = new HttpClient())
            {
                // For more information on the String.Format method see: https://msdn.microsoft.com/en-us/library/system.string.format(v=vs.110).aspx
                // For more information on the ConfigurationManager.AppSettings property see: https://msdn.microsoft.com/en-us/library/system.configuration.configurationmanager.appsettings%28v=vs.110%29.aspx
                string downloadUrl = string.Format(ConfigurationManager.AppSettings["BingDefaultDownloadRoot"] + targetUri);

                // Since Stream is IDisposable, we wrap in a 'using' statement.
                // For more information on the Stream class see: https://msdn.microsoft.com/en-us/library/system.io.stream(v=vs.118).aspx
                // For more information on the HttpClient.GetStreamAsync(string) method see: https://msdn.microsoft.com/en-us/library/hh551738(v=vs.118).aspx
                using (Stream newImageStream = await imageHttpClient.GetStreamAsync(downloadUrl))
                {
                    // Since Image is IDisposable, we wrap in a 'using' statement.
                    // For more information on the Image class see: https://msdn.microsoft.com/en-us/library/system.drawing.image(v=vs.110).aspx
                    // For more information on the Image.FromStream(Stream) method see: https://msdn.microsoft.com/en-us/library/93z9ee4x(v=vs.110).aspx
                    using (Image convertedStreamImage = Image.FromStream(newImageStream))
                    {
                        // For more information on the String.LastIndexOf Method (String, StringComparison) method see: https://msdn.microsoft.com/en-us/library/ms224422(v=vs.110).aspx
                        // For more information on the StringComparison enumeration see: https://msdn.microsoft.com/en-us/library/system.stringcomparison(v=vs.110).aspx
                        int startIndex = downloadUrl.LastIndexOf("/", StringComparison.InvariantCulture);
                        int length = (downloadUrl.Length - startIndex);

                        // For more information on the String.Substring(Int32, Int32) method see: https://msdn.microsoft.com/en-us/library/aka44szs(v=vs.110).aspx
                        string imageName = downloadUrl.Substring(startIndex, length);

                        // For more information on the Regex.Replace(String, String, String) method see: https://msdn.microsoft.com/en-us/library/e7f5w83z(v=vs.110).aspx
                        string fixedImageNameString = Regex.Replace(imageName, "/", "");

                        // For more information on the Image.Save(String) method see: https://msdn.microsoft.com/en-us/library/ktx83wah(v=vs.110).aspx
                        // For more information on the ConfigurationManager.AppSettings property see: https://msdn.microsoft.com/en-us/library/system.configuration.configurationmanager.appsettings%28v=vs.110%29.aspx
                        convertedStreamImage.Save(ConfigurationManager.AppSettings["TargetFolder"] + fixedImageNameString);
                    }
                }
            }

            Objects.FirstRun = false;

            // For more information on the SemaphoreSlim.Release(Int32) method see: https://msdn.microsoft.com/en-us/library/dd289587(v=vs.110).aspx
            passedSemaphoreSlim.Release(1);
        }
    }
}
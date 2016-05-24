using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Calls;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Streaming.Adaptive;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace testUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private AdaptiveMediaSource ams;
        private System.Uri manifestUri;
        private HttpClient httpClient;

        public MainPage()
        {
            this.InitializeComponent();
            httpClient = new Windows.Web.Http.HttpClient();
            InitializeAdaptiveMediaSource(new Uri("http://amssamples.streaming.mediaservices.windows.net/49b57c87-f5f3-48b3-ba22-c55cfdffa9cb/Sintel.ism/manifest(format=m3u8-aapl)"));


        }

        private void Row3Knapp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Knapp1_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void Knapp2_Click(object sender, RoutedEventArgs e)
        {

            var sucess = await Launcher.LaunchUriAsync(new Uri(@"http://united.no"));
            if (sucess)
            {
                await Launcher.LaunchUriAsync(new Uri(@"http://united.no"));
            }
            else
            {
                throw new Exception("feilet");
            }
        }

        async private void InitializeAdaptiveMediaSource(System.Uri uri)
        {
            manifestUri = uri;
           httpClient.DefaultRequestHeaders.TryAppendWithoutValidation("X-CustomHeader", "This is a custom header");
            AdaptiveMediaSourceCreationResult result = await AdaptiveMediaSource.CreateFromUriAsync(manifestUri, httpClient);
            

            if (result.Status == AdaptiveMediaSourceCreationStatus.Success)
            {
                ams = result.MediaSource;
                mediaElement.SetMediaStreamSource(ams);


                ams.InitialBitrate = ams.AvailableBitrates.Max<uint>();

                //Register for download requests
           //     ams.DownloadRequested += DownloadRequested;

                //Register for bitrate change events
    //            ams.DownloadBitrateChanged += DownloadBitrateChanged;
  //              ams.PlaybackBitrateChanged += PlaybackBitrateChanged;
            }
            else
            {
                // Handle failure to create the adaptive media source
            }
        }

      
        
       private async void DownloadRequested(AdaptiveMediaSource sender, AdaptiveMediaSourceDownloadRequestedEventArgs args)
       {

           // rewrite key URIs to replace http:// with https://
           if (args.ResourceType == AdaptiveMediaSourceResourceType.Key)
           {
               string originalUri = args.ResourceUri.ToString();
               string secureUri = originalUri.Replace("http:", "https:");

               // override the URI by setting property on the result sub object
               args.Result.ResourceUri = new Uri(secureUri);
           }

           if (args.ResourceType == AdaptiveMediaSourceResourceType.Manifest)
           {
               AdaptiveMediaSourceDownloadRequestedDeferral deferral = args.GetDeferral();
              // args.Result.Buffer = await CreateMyCustomManifest(args.ResourceUri);
               deferral.Complete();
           }
       }
 /*
       private async void DownloadBitrateChanged(AdaptiveMediaSource sender, AdaptiveMediaSourceDownloadBitrateChangedEventArgs args)
       {
           await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
           {
               txtDownloadBitrate.Text = args.NewValue.ToString();
           }));
       }

       private async void PlaybackBitrateChanged(AdaptiveMediaSource sender, AdaptiveMediaSourcePlaybackBitrateChangedEventArgs args)
       {
           await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
           {
               txtPlaybackBitrate.Text = args.NewValue.ToString();
              
           }));
       }
       */
    }
}

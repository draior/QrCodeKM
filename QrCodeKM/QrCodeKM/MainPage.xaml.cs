using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Mobile;

namespace QrCodeKM
{
  public partial class MainPage : ContentPage
  {
    internal string ipAddress;
    internal string port;
    internal string endpoint = "/repaircard";
    internal string bg;
    internal string lastCode = string.Empty;

    public MainPage()
    {
      InitializeComponent();

      ipAddress = Preferences.Get("IPAddress", string.Empty);
      port = Preferences.Get("Port", string.Empty);
      bg = Preferences.Get("Bg", string.Empty);
    }

    private void ScanButton_Clicked(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(ipAddress) || string.IsNullOrEmpty(port) || string.IsNullOrEmpty(bg))
      {
        DisplayAlert("Error", $"Please first fill the settings", "OK");
        return;
      }

      staticImage.IsVisible = !staticImage.IsVisible;
      scannerView.IsVisible = !scannerView.IsVisible;

      scannerView.IsScanning = scannerView.IsVisible;

      if (scannerView.IsScanning)
      {
        // Stop scanning if it's already started.
        decodedLabel.Text = "Scanning...";
      }
      else
      {
        // Start scanning.
        decodedLabel.Text = "Scanning stopped";
      }
    }

    private async void OnScanResult(ZXing.Result result)
    {
      if (result != null && !string.IsNullOrEmpty(result.Text))
      {
        // Display the decoded content in the label.
        Device.BeginInvokeOnMainThread(() =>
        {
          DependencyService.Get<IVibrationService>().Vibrate();
          DependencyService.Get<IPlayBeepSoundService>().Beep();

          scannerView.IsVisible = false;
          scannerView.IsScanning = scannerView.IsVisible;
          staticImage.IsVisible = true;

          decodedLabel.Text = "Decoded Content: " + result.Text;
          if (lastCode.Equals(result.Text))
          {
            return;
          }

          using (HttpClient client = new HttpClient())
          {
            try
            {
              // Define the URL of your server endpoint
              string apiUrl = $"http://{ipAddress}:{port}{endpoint}";

              // Prepare the data to send (you can customize this based on your server's API)
              var postData = new Dictionary<string, string>
              {
                { "repaircardno", result.Text },
                { "bg", bg }
              };
             

              // Convert the data to JSON or other suitable format
              var jsonContent = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");

              // Send a POST request to the server asynchronously and without awaiting it
              Task<HttpResponseMessage> response = client.PostAsync(apiUrl, jsonContent);

              // Handle the response here
              if (response.Result.IsSuccessStatusCode)
              {
                // Handle a successful response here
                AddLine($@"QR code {result.Text} sent successfully");
                //DisplayAlert("Success", "QR code data sent successfully.", "OK");
              }
              else
              {
                // Handle an unsuccessful response here
                AddLine($@"Failed to send QR code {result.Text}");
                //DisplayAlert("Error", "Failed to send QR code data.", "OK");
              }
            }
            catch (Exception ex)
            {
              // Handle any exceptions that occur during the request
              AddLine($@"An error occurred: {ex.Message}");
              //DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
          }
        });
      }
    }

    // Event handler for the "Send POST Request" button
    private async void Settings_Clicked(object sender, EventArgs e)
    {
      await Navigation.PushAsync(new SettingsPage(this));
    }

    private void AddLine(string msg)
    {
      var currentFormattedString = lblMsg.FormattedText ?? new FormattedString(); 

      // Add a new span with a line break
      currentFormattedString.Spans.Insert(0, new Span { Text = "\n" });
      currentFormattedString.Spans.Insert(0, new Span { Text = msg });

      // Update the label with the modified formatted string
      lblMsg.FormattedText = currentFormattedString;
    }

    private void Clear_Clicked(object sender, EventArgs e)
    {
      // Clear the text of the multiline label
      lblMsg.Text = string.Empty;
    }
  }
}

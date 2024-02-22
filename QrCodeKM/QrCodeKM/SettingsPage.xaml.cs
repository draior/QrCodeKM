using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QrCodeKM
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class SettingsPage : ContentPage
  {
    internal MainPage _mainPage;

    public SettingsPage(MainPage mainPage)
    {
      InitializeComponent();

      _mainPage = mainPage;
      ipEntry.Text = mainPage.ipAddress;
      portEntry.Text = mainPage.port;
      endpointEntry.Text = mainPage.endpoint;
      bgEntry.Text = mainPage.bg;
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
      // Retrieve the entered IP, port, and endpoint values
      _mainPage.ipAddress = ipEntry.Text;
      _mainPage.port = portEntry.Text;
      _mainPage.endpoint = endpointEntry.Text;
      _mainPage.bg = bgEntry.Text;

      // Save the settings using Xamarin.Essentials Preferences or another method
      // For example:
      Preferences.Set("IPAddress", _mainPage.ipAddress);
      Preferences.Set("Port", _mainPage.port);
      Preferences.Set("Bg", _mainPage.bg);

      // Optionally, navigate back to the previous page after saving
      Navigation.PopAsync();
    }

    private void TestButton_Clicked(object sender, EventArgs e)
    {
      using (HttpClient client = new HttpClient())
      {
        try
        {
          string apiUrl = $"http://{ipEntry.Text}:{portEntry.Text}/checkstatus";

          Task<HttpResponseMessage> response = client.GetAsync(apiUrl);

          // Handle the response here
          if (response.Result.IsSuccessStatusCode)
          {
            // Handle a successful response here
            DisplayAlert("Success", "Get status successfully.", "OK");
          }
          else
          {
            // Handle an unsuccessful response here
            DisplayAlert("Error", "Failed to send get status.", "OK");
          }
        }
        catch (Exception ex)
        {
          // Handle any exceptions that occur during the request
          DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
      }
    }    
  }
}
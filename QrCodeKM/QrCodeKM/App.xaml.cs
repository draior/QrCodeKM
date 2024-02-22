using System;
using Xamarin.Forms;
using QrCodeKM;
using Xamarin.Forms.Xaml;

namespace QrCodeKM
{
  public partial class App : Application
  {
    public App()
    {
      InitializeComponent();

      MainPage = new NavigationPage(new MainPage());
    }

    protected override void OnStart()
    {
    }

    protected override void OnSleep()
    {
    }

    protected override void OnResume()
    {
    }
  }
}

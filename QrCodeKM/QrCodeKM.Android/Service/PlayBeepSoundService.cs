using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using QrCodeKM.Droid.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(PlayBeepSoundService))]
namespace QrCodeKM.Droid.Service
{  
  public class PlayBeepSoundService : IPlayBeepSoundService
  {
    public void Beep()
    {
      int resourceId = Resource.Raw.scanner_beep;

      // Construct the Uri from the resource ID
      Android.Net.Uri uri = Android.Net.Uri.Parse("android.resource://" + Application.Context.PackageName + "/" + resourceId);

      // Create MediaPlayer using the Uri
      MediaPlayer player = MediaPlayer.Create(Application.Context, uri);
      player.Start(); // 
    }
  }
}
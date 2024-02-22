using Android.OS;
using QrCodeKM;

[assembly: Xamarin.Forms.Dependency(typeof(VibrationService))]
namespace QrCodeKM
{
  public class VibrationService : IVibrationService
  {
    public void Vibrate()
    {
      if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
      {
        // Create a VibrationEffect
        var effect = VibrationEffect.CreateOneShot(1500, VibrationEffect.DefaultAmplitude);

        // Get the Vibration Service
        var vibrator = (Vibrator)Android.App.Application.Context.GetSystemService(Android.Content.Context.VibratorService);

        // Vibrate with the VibrationEffect
        vibrator.Vibrate(effect);
      }
      else
      {
        // For devices below Android Oreo, fall back to the old vibrate method
        var vibrator = (Vibrator)Android.App.Application.Context.GetSystemService(Android.Content.Context.VibratorService);
        vibrator.Vibrate(1500); // Vibrate for 500 milliseconds
      }

      // next lines are working
      /*var vibrator = (Vibrator)Android.App.Application.Context.GetSystemService(Android.Content.Context.VibratorService);
      if (vibrator.HasVibrator)
      {
        var effect = VibrationEffect.CreateOneShot(1500, VibrationEffect.EffectHeavyClick);

        vibrator.Vibrate(effect);
      }

      vibrator.Vibrate(1500); */
    }
  }
}
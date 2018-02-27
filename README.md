# ModernAlerts







Android:
Download https://www.nuget.org/packages/ModernAlerts.Droid/ Nuget Pacakge in Droid Project

Android Initialization in Your Main Activity.
ModernAlerts.ModernAlertsHelper.GetInstance().Init(this);

Like below
public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
{
protected override void OnCreate(Bundle bundle)
{
base.OnCreate(bundle);
global::Xamarin.Forms.Forms.Init(this, bundle);
ModernAlerts.ModernAlertsHelper.GetInstance().Init(this);
LoadApplication(new App());
}
}

iOS:
Download https://www.nuget.org/packages/ModernAlerts.iOS/ get Pacakge in iOS Project
No Need to initialize for IOS Apps

Simple Display Alert:
ModernAlertsHelper.GetInstance().DisplayAlert(Color.Red, Color.White, "Modern Alerts Simple Alert", "Modern Alerts Simple Alert Body", "OK", null, (obj) =>
{
    //Logic
});

Confirmation Alert:
ModernAlertsHelper.GetInstance().DisplayAlert(Color.Red,Color.White, "Modern Alert Confirmation Header","Modern Alert Confirmation Body", "Yes", "No", (obj) =>
{
if (obj == null) return;
//Logic
});

ActionSheet:
string[] numbersarray = new string[] { "1","2","3","4" };
ModernAlertsHelper.GetInstance().ActionSheet(Color.Red,Color.White,"Modern Alerts ActionSheet", numbersarray, "Cancel", (obj) =>
{
if (obj == null || (obj != null && obj == "Cancel")) return;
//Logic
});

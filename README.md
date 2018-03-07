# ModernAlerts for Xamrin Forms


Android:<br />
Download https://www.nuget.org/packages/ModernAlerts.Droid/ Nuget Pacakge in Droid Project

Android Initialization in Main Activity.<br />
```c#
ModernAlerts.ModernAlertsHelper.GetInstance().Init(this);
```
Like below<br />
```c#
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
```


iOS:<br />
Download https://www.nuget.org/packages/ModernAlerts.iOS/ get Pacakge in iOS Project<br />
No Need to initialize for IOS Apps<br />

Simple Display Alert:
```c#
ModernAlertsHelper.GetInstance().DisplayAlert(Color.Red, Color.White, "Modern Alerts Simple Alert", "Modern Alerts Simple Alert Body", "OK", null, (obj) =>
{
    //Logic
});
```

Simple Get Input Alert: <br />
```c# 
GetInputConfig getInputConfig = new GetInputConfig();
getInputConfig.BackgroundColor = Color.White;
getInputConfig.keyboard = Keyboard.Telephone;
getInputConfig.MaxLength = 10;
getInputConfig.FontColor = Color.Black;
ModernAlertsHelper.GetInstance().DisplayAlert(Color.Black, Color.White, "Modern Alerts Simple Alert", "Modern Alerts Simple Alert Body", "OK", null, (obj) =>
{

//Logic

}, true, getInputConfig);
```

Confirmation Alert:
```c#
ModernAlertsHelper.GetInstance().DisplayAlert(Color.Red,Color.White, "Modern Alert Confirmation Header","Modern Alert Confirmation Body", "Yes", "No", (obj) =>
{
if (obj == null) return;
//Logic
});
```
ActionSheet:
```c#
string[] numbersarray = new string[] { "1","2","3","4" };
ModernAlertsHelper.GetInstance().ActionSheet(Color.Red,Color.White,"Modern Alerts ActionSheet", numbersarray, "Cancel", (obj) =>
{
if (obj == null || (obj != null && obj == "Cancel")) return;
//Logic
});
```

using System;
using System.Threading.Tasks;
using Android;
using Android.Content;
using Android.Content.PM;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Telephony;
using Android.Text;
using Android.Views;
using Android.Widget;
using ModernAlerts;
using ModernAlerts.Droid;
using ModernAlerts.Droid.ProgressBar;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(ModerAlertHelperService))]
namespace ModernAlerts.Droid
{
    public class ModerAlertHelperService : IShowDialogService
    {

        public ModerAlertHelperService()
        {
        }

        public async Task<bool> ActionSheet(Color backgroundcolor, Color fontColor, string title, string[] list, string cancelButton, Action<string> callback)
        {
            return await Task.Run(() => ShowActionSheet(backgroundcolor, fontColor, title, list, cancelButton, callback));
        }

        public async Task<bool> DisplayAlert(Color backgroundcolor, Color fontColor, string title, string content, string positiveButton, string negativeButton, Action<string> callback, bool isGetInput, GetInputConfig getinputConfig)
        {
            return await Task.Run(() => Alert(backgroundcolor, fontColor, title, content, isGetInput, positiveButton, negativeButton, callback, getinputConfig));
        }

        private bool ShowActionSheet(Color backgroundcolor, Color fontColor, string title, string[] content, string negativeButton, Action<string> callback)
        {
            bool alercon = false;
            if (ModernAlertsHelper.currentActivity == null)
            {
                throw new Exception("Call ModernAlertHelper.GetInstance().Init(this) in your MainActivity");
            }
            try
            {
                AlertDialog alertdialog = null;
                ModernAlertsHelper.currentActivity.RunOnUiThread(() =>
                {

                    AlertDialog.Builder dialogBuilder = new AlertDialog.Builder(ModernAlertsHelper.currentActivity);
                    LayoutInflater inflater = ModernAlertsHelper.currentActivity.LayoutInflater;
                    var convertView = inflater.Inflate(Resource.Layout.AlertDialogAlertLayout, null);
                    dialogBuilder.SetView(convertView);
                    LinearLayout bv = (LinearLayout)convertView.FindViewById(Resource.Id.body_view);
                    LinearLayout listview_root = (LinearLayout)convertView.FindViewById(Resource.Id.listview_root);
                    listview_root.Visibility = ViewStates.Visible;
                    bv.Visibility = ViewStates.Gone;
                    Android.Widget.ListView lv = (Android.Widget.ListView)convertView.FindViewById(Resource.Id.listiview);
                    AlertDialogListViewAdapter adapter = new AlertDialogListViewAdapter(ModernAlertsHelper.currentActivity, content, backgroundcolor.ToAndroid(), fontColor.ToAndroid());
                    lv.Adapter = adapter;
                    lv.SetDrawSelectorOnTop(true);
                    lv.Divider = null;
                    var header = convertView.FindViewById<TextView>(Resource.Id.header);
                    var root = convertView.FindViewById<LinearLayout>(Resource.Id.root);
                    var lineheader = convertView.FindViewById(Resource.Id.lineheader);
                    lineheader.SetBackgroundColor(fontColor.ToAndroid());
                    var linebody = convertView.FindViewById(Resource.Id.linebody);
                    linebody.SetBackgroundColor(fontColor.ToAndroid());
                    //var leftSpacer = convertView.FindViewById<LinearLayout>(Resource.Id.leftSpacer);
                    //leftSpacer.Visibility = ViewStates.Visible;
                    var buttons = convertView.FindViewById<LinearLayout>(Resource.Id.buttons);
                    var button1 = convertView.FindViewById<Android.Widget.Button>(Resource.Id.positinvebutton);
                    header.SetTextColor(fontColor.ToAndroid());
                    header.Text = title;
                    buttons.SetBackgroundColor(backgroundcolor.ToAndroid());
                    root.SetBackgroundColor(backgroundcolor.ToAndroid());
                    button1.Visibility = ViewStates.Visible;
                    button1.Text = negativeButton;
                    button1.SetTextColor(fontColor.ToAndroid());
                    button1.Click += ((si, e) =>
                    {
                        callback(negativeButton);
                        alertdialog.Dismiss();
                    });
                    lv.ItemClick += ((sd, w) =>
                      {
                          callback(content[w.Position]);
                          alertdialog.Dismiss();
                      });
                    lv.ItemSelected += ((sd, w) =>
                      {
                          callback(content[w.Position]);
                          alertdialog.Dismiss();
                      });
                    alertdialog = dialogBuilder.Create();
                    alertdialog.Show();
                });
            }
            catch (Exception e)
            {
                throw e;
            }
            return alercon;
        }


        private bool Alert(Color backgroundcolor, Color fontColor, string title, string content, bool isGetInput, string positiveButton, string negativeButton, Action<string> callback, GetInputConfig config)
        {
            bool alercon = false;
            if (ModernAlertsHelper.currentActivity == null)
            {
                throw new Exception("Call ModernAlertHelper.GetInstance().Init(this) in your MainActivity");
            }
            try
            {
                AlertDialog alertdialog = null;
                ModernAlertsHelper.currentActivity.RunOnUiThread(() =>
                {
                    AlertDialog.Builder dialogBuilder = new AlertDialog.Builder(ModernAlertsHelper.currentActivity);

                    LayoutInflater inflater = ModernAlertsHelper.currentActivity.LayoutInflater;
                    var alertLayout = inflater.Inflate(Resource.Layout.AlertDialogAlertLayout, null);
                    dialogBuilder.SetView(alertLayout);
                    var header = alertLayout.FindViewById<TextView>(Resource.Id.header);
                    var lineheader = alertLayout.FindViewById(Resource.Id.lineheader);
                    lineheader.SetBackgroundColor(fontColor.ToAndroid());
                    var linebody = alertLayout.FindViewById(Resource.Id.linebody);
                    linebody.SetBackgroundColor(fontColor.ToAndroid());
                    var root = alertLayout.FindViewById<LinearLayout>(Resource.Id.root);
                    var body = alertLayout.FindViewById<TextView>(Resource.Id.body);
                    var buttons = alertLayout.FindViewById<LinearLayout>(Resource.Id.buttons);
                    var button1 = alertLayout.FindViewById<Android.Widget.Button>(Resource.Id.positinvebutton);
                    var getinput_et = alertLayout.FindViewById<Android.Widget.EditText>(Resource.Id.getinput_et);
                    header.Text = title;
                    body.Text = content;
                    header.SetTextColor(fontColor.ToAndroid());
                    body.SetTextColor(fontColor.ToAndroid());
                    buttons.SetBackgroundColor(backgroundcolor.ToAndroid());
                    root.SetBackgroundColor(backgroundcolor.ToAndroid());
                    button1.Visibility = ViewStates.Visible;
                    button1.Text = positiveButton;
                    button1.SetTextColor(fontColor.ToAndroid());
                    button1.Click += ((si, e) =>
                    {
                        if (isGetInput)
                        {
                            if (!string.IsNullOrEmpty(getinput_et.Text))
                            {
                                callback(getinput_et.Text);
                                alertdialog.Dismiss();
                            }
                        }
                        else
                        {
                            callback(positiveButton);
                            alertdialog.Dismiss();
                        }
                    });
                    if (isGetInput)
                    {
                        var getinputll = alertLayout.FindViewById<LinearLayout>(Resource.Id.get_input_view);
                        if (config != null)
                        {
                            getinput_et.SetBackgroundColor(config.BackgroundColor.ToAndroid());
                            getinput_et.SetTextColor(config.FontColor.ToAndroid());
                            if (config.keyboard == Keyboard.Telephone)
                            {
                                getinput_et.InputType = InputTypes.ClassPhone;
                            }
                            else if (config.keyboard == Keyboard.Numeric)
                            {
                                getinput_et.InputType = InputTypes.ClassNumber;
                            }
                            //getinput_et.SetFilters(new global::Android.Text.IInputFilter[] { Android.Text.InputFilterLengthFilter=config.MaxLenght });
                        }
                        getinputll.Visibility = ViewStates.Visible;
                    }
                    if (negativeButton != null)
                    {
                        var button2 = alertLayout.FindViewById<Android.Widget.Button>(Resource.Id.negativebutton);
                        button2.Visibility = ViewStates.Visible;
                        button2.Text = negativeButton;
                        button2.SetTextColor(fontColor.ToAndroid());
                        button2.Click += ((si, e) =>
                        {
                            callback(negativeButton);
                            alertdialog.Dismiss();
                        });
                    }
                    alertdialog = dialogBuilder.Create();
                    alertdialog.Show();

                });
            }
            catch (Exception e)
            {
                throw e;
            }
            return alercon;
        }

        public void ShowLoading(string title, bool show, string cancelText, MaskType? maskType, Action onCancel)
        {
            try
            {
                var config = new ProgressDialogConfig
                {
                    Title = title ?? ProgressDialogConfig.DefaultTitle,
                    AutoShow = show,
                    CancelText = cancelText ?? ProgressDialogConfig.DefaultCancelText,
                    MaskType = maskType ?? ProgressDialogConfig.DefaultMaskType,
                    IsDeterministic = true,
                    OnCancel = onCancel
                };

                ProgressDialog pr = new ProgressDialog(config, ModernAlertsHelper.currentActivity);
                pr.Show();
            }
            catch (Exception e)
            {

            }
        }

    }
}
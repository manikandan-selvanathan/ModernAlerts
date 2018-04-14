using ModernAlerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sample.Forms
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void GetInput_Clicked(object sender, EventArgs e)
        {
            try
            {

                var noteInput = new InputConfig()
                {
                    BackgroundColor = Color.White,
                    keyboard = Keyboard.Numeric,
                    FontColor = Color.Black,
                    isMultipleLine = true
                };
                ModernAlertsHelper.GetInstance().DisplayAlert(Color.Black, Color.White, "Add Note", "", "save", "Cancel", null, (obj) =>
                {
                    if (string.IsNullOrEmpty(obj))
                        return;
                    if (obj != "Cancel")
                    {
                    }
                }, true, noteInput);



                //InputConfig getInputConfig = new InputConfig();
                //getInputConfig.BackgroundColor = Color.Black;
                //getInputConfig.keyboard = Keyboard.Telephone;
                //getInputConfig.MaxLength = 30;
                //getInputConfig.MinLength = 10;
                //getInputConfig.placeholder = "Enter asdf";
                //getInputConfig.FontColor = Color.White;
                //getInputConfig.DefaultValue = "Default Value";
                //ModernAlertsHelper.GetInstance().DisplayAlert(Color.Black, Color.White, "Modern Alerts Simple Alert", "Modern Alerts Simple Alert Body", "OK", "CANCEL",null, (obj) =>
                //{
                //    //Logic
                //},true, getInputConfig);
            }
            catch (Exception ex)
            {

            }
        }
        private void DisplayAlert_Clicked(object sender, EventArgs e)
        {
            try
            {
                ModernAlertsHelper.GetInstance().DisplayAlert(Color.Red, Color.White, "Modern", "Modern Alerts Simple </br> Alert </br>Body ", "OK", null, null, (obj) =>
                 {
                    //Logic
                }, iscontentleftalign:true);
            }
            catch (Exception ex)
            {

            }
        }
        private void DisplayConfirmation_Clicked(object sender, EventArgs e)
        {
            try
            {
                ModernAlertsHelper.GetInstance().DisplayAlert(Color.Red, Color.White, "Modern Alert Confirmation Header", "Modern Alert Confirmation Body", "Yes", "No", "MayBe", (obj) =>
                 {
                     if (obj == null) return;
                    //Logic
                });
            }
            catch (Exception ex)
            {

            }
        }

        private void DisplayActionSheet_Clicked(object sender, EventArgs e)
        {
            try
            {
                string[] numbersarray = new string[] { "1", "2", "3", "4" };
                ModernAlertsHelper.GetInstance().ActionSheet(Color.Red, Color.White, "Modern Alerts ActionSheet", numbersarray, "Cancel", (obj) =>
                {
                    if (obj == null || (obj != null && obj == "Cancel")) return;
                    //Logic
                });
            }
            catch (Exception ex)
            {

            }
        }

        private void DisplayLoader_Clicked(object sender, EventArgs e)
        {
            try
            {
                string[] numbersarray = new string[] { "1", "2", "3", "4" };
                //ModernAlertsHelper.GetInstance().ShowLoading();
            }
            catch (Exception ex)
            {

            }
        }
    }
}

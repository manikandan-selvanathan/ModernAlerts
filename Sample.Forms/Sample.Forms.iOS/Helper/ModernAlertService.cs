﻿using System;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Xamarin.Forms;
using ModernAlerts.IOS;
using Xamarin.Forms.Platform.iOS;
[assembly: Dependency(typeof(ModernAlertService))]
namespace ModernAlerts.IOS
{
    //https://iosdevcenters.blogspot.com/2016/05/hacking-uialertcontroller-in-swift.html
    public class ModernAlertService : IShowDialogService
    {

        public async Task<bool> ActionSheet(Color backgroundcolor, Color fontColor, string title, string[] list, string cancelButton, Action<string> callback)
        {
            return await Task.Run(() => ShowActionSheet(backgroundcolor, fontColor, title, list, cancelButton, callback));
        }

        public async Task<bool> DisplayAlert(Color backgroundcolor, Color fontColor, string title, string content, string positiveButton, string negativeButton, Action<string> callback, bool isGetInput, InputConfig getinputConfig = null)
        {
            return await Task.Run(() => Alert(backgroundcolor, fontColor, title, content, positiveButton, negativeButton, callback, getinputConfig, isGetInput));
        }

        private bool ShowActionSheet(Color backgroundcolor, Color fontColor, string title, string[] content, string negativeButton, Action<string> callback)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var pushView = UIAlertController.Create(title, "", UIAlertControllerStyle.ActionSheet);


                foreach (var item in content)
                {
                    pushView.AddAction(UIAlertAction.Create(item, UIAlertActionStyle.Default, (action) => callback(item)));
                }
                pushView.AddAction(UIAlertAction.Create(negativeButton, UIAlertActionStyle.Destructive, (action) => callback(negativeButton)));

                //Customization/Hack
                UIView[] array = pushView.View.Subviews;
                array[0].Subviews[0].TintColor = fontColor.ToUIColor();

                foreach (var item in array[0].Subviews[0].Subviews[0].Subviews)
                {
                    item.BackgroundColor = backgroundcolor.ToUIColor();
                }
                UIStringAttributes titlecolor = new UIStringAttributes
                {
                    ForegroundColor = fontColor.ToUIColor(),
                };
                pushView.SetValueForKey(new NSAttributedString(title, titlecolor), new NSString("attributedTitle"));
                pushView.SetValueForKey(new NSAttributedString("", titlecolor), new NSString("attributedMessage"));
                UIPopoverPresentationController presentationPopover = pushView.PopoverPresentationController;
                var window = UIApplication.SharedApplication.KeyWindow;
                if (presentationPopover != null)
                {
                    presentationPopover.SourceView = window;
                    presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up;
                }
                window.MakeKeyAndVisible();
                window.RootViewController.PresentViewController(pushView, true, null);

            });

            return true;
        }



        private bool Alert(Color backgroundcolor, Color fontColor, string title, string content, string positiveButton, string negativeButton, Action<string> callback, InputConfig config, bool isGetInput)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var pushView = UIAlertController.Create(title, content, UIAlertControllerStyle.Alert);
                 
                UITextField field = null;
                var posbut = UIAlertAction.Create(positiveButton, UIAlertActionStyle.Default, (obj) =>
                {
                    if (field != null && !string.IsNullOrEmpty(field.Text))
                        callback(field.Text);
                    else
                        callback(positiveButton);
                });
            
                if (isGetInput)
                { 
                    pushView.AddTextField((textField) =>
                    {
                        field = textField;
                        if (isGetInput)
                        {
                            UIView[] array2 = pushView.View.Subviews;
                            if (config != null && config.MinLength != 0)
                            {
                                array2[0].Subviews[0].TintColor = UIColor.Gray; 
                                posbut.Enabled = false;
                                field.AddTarget((w, me) =>
                                { 
                                    if (field.Text.Trim().Length >= config.MinLength)
                                    {
                                        array2[0].Subviews[0].TintColor = fontColor.ToUIColor();
                                        posbut.Enabled = true; 
                                    }
                                    else
                                    { 
                                        array2[0].Subviews[0].TintColor = UIColor.Gray; 
                                        posbut.Enabled = false;
                                    }
                                }, UIControlEvent.EditingChanged);
                            }
                        } 
                        if (config != null)
                        {
                            if (config.keyboard == Keyboard.Telephone)
                                field.KeyboardType = UIKeyboardType.PhonePad;
                            else if (config.keyboard == Keyboard.Text)
                                field.KeyboardType = UIKeyboardType.Default;
                            else if (config.keyboard == Keyboard.Email)
                                field.KeyboardType = UIKeyboardType.EmailAddress;

                            field.Placeholder = config.placeholder;
                            field.TextColor = config.FontColor.ToUIColor();
                            field.BackgroundColor = config.BackgroundColor.ToUIColor();
                            field.ShouldChangeCharacters = (UITextField sd, NSRange range, string replacementString) =>
                            {
                                var length = textField.Text.Length - range.Length + replacementString.Length;
                                return length <= config.MaxLength;
                            };
                        }
                        field.AutocorrectionType = UITextAutocorrectionType.No;
                        field.ClearButtonMode = UITextFieldViewMode.WhileEditing;
                        field.ReturnKeyType = UIReturnKeyType.Done;
                        field.BorderStyle = UITextBorderStyle.RoundedRect;
                    });

                }
                pushView.AddAction(posbut);
                

                //Customization/Hack
                UIView[] array = pushView.View.Subviews;

                if (config != null && config.MinLength != 0)
                {
                    array[0].Subviews[0].TintColor = UIColor.Gray; 
                }
                else
                {
                    array[0].Subviews[0].TintColor = fontColor.ToUIColor();
                } 
                foreach (var item in array[0].Subviews[0].Subviews[0].Subviews)
                {
                    item.BackgroundColor = backgroundcolor.ToUIColor();
                }
                UIStringAttributes titlecolor = new UIStringAttributes
                {
                    ForegroundColor = fontColor.ToUIColor(),
                };
                pushView.SetValueForKey(new NSAttributedString(title, titlecolor), new NSString("attributedTitle"));
                pushView.SetValueForKey(new NSAttributedString(content, titlecolor), new NSString("attributedMessage"));
                if (negativeButton != null)
                {
                    pushView.AddAction(UIAlertAction.Create(negativeButton, UIAlertActionStyle.Cancel, (obj) =>
                    {
                        callback(negativeButton);
                    }));
                }

                var window = UIApplication.SharedApplication.KeyWindow;
                window.MakeKeyAndVisible();
                window.RootViewController.PresentViewController(pushView, true, null); 
            });

            return true;
        }

    }
}

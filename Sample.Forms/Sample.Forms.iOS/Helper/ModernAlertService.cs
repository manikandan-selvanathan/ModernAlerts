using System;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Xamarin.Forms;
using ModernAlerts.IOS;
using Xamarin.Forms.Platform.iOS;
using CoreGraphics;

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

        public async Task<bool> DisplayAlert(Color backgroundcolor, Color fontColor, string title, string content, string positiveButton, string negativeButton,string neutralButton, Action<string> callback, bool isGetInput, InputConfig getinputConfig = null, bool iscontentleftalign=false)
        {
            return await Task.Run(() => Alert(backgroundcolor, fontColor, title, content, positiveButton, negativeButton, neutralButton, callback, getinputConfig, isGetInput, iscontentleftalign));
        }

        private bool ShowActionSheet(Color backgroundcolor, Color fontColor, string title, string[] content, string negativeButton, Action<string> callback)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var window = UIApplication.SharedApplication.KeyWindow;
                var visualEffectView = GetTransparentView(); 
                var pushView = UIAlertController.Create(title,null, UIAlertControllerStyle.ActionSheet);
                foreach (var item in content)
                { 
                    pushView.AddAction(UIAlertAction.Create(item, UIAlertActionStyle.Default, (action) =>
                    {
                        visualEffectView.RemoveFromSuperview();
                        callback(item);
                    }));
                }
                pushView.AddAction(UIAlertAction.Create(negativeButton, UIAlertActionStyle.Destructive, (action) =>
                {
                    visualEffectView.RemoveFromSuperview();
                    callback(negativeButton);
                })); 
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
                    Font = UIFont.FromName("Helvetica-Bold", 18f),
                };
                pushView.SetValueForKey(new NSAttributedString(title, titlecolor), new NSString("attributedTitle"));
                //pushView.SetValueForKey(new NSAttributedString("", titlecolor), new NSString("attributedMessage"));
                UIPopoverPresentationController presentationPopover = pushView.PopoverPresentationController; 
                if (presentationPopover != null)
                {
                    presentationPopover.SourceView = window;
                    presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up;
                }
                pushView.View.UserInteractionEnabled = true;
                pushView.View.AddGestureRecognizer(new UITapGestureRecognizer(() => {
                    Console.WriteLine("Clicked Outside");
                }));
                window.MakeKeyAndVisible();
                window.RootViewController.PresentViewController(pushView, true, null); 
                window.RootViewController.View.AddSubview(visualEffectView); 
            }); 
            return true;
        }

        UIView GetTransparentView()
        {
            UIView visualEffectView = new UIView();
            var window = UIApplication.SharedApplication.KeyWindow;
            visualEffectView.BackgroundColor = UIColor.Black.ColorWithAlpha((nfloat)0.6);
            visualEffectView.Frame = window.RootViewController.View.Bounds;
            return visualEffectView;
        }


        NSMutableParagraphStyle GetLeftAlignStyle()
        {
            NSMutableParagraphStyle style = new NSMutableParagraphStyle();
            style.Alignment = UITextAlignment.Left;
            return style;
        }
        UIVisualEffectView GetBlurView()
        {
            UIVisualEffect blurEffect = UIBlurEffect.FromStyle(UIBlurEffectStyle.Prominent);
            var window = UIApplication.SharedApplication.KeyWindow;
            UIVisualEffectView visualEffectView = new UIVisualEffectView(blurEffect);
            visualEffectView.Frame = window.RootViewController.View.Bounds;
            return visualEffectView;
        }


        private bool Alert(Color backgroundcolor, Color fontColor, string title, string content, string positiveButton, string negativeButton,string neutralButton, Action<string> callback, InputConfig config, bool isGetInput,bool iscontentleftalign)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var pushView = UIAlertController.Create(title, content, UIAlertControllerStyle.Alert);
                var window = UIApplication.SharedApplication.KeyWindow; 
                var visualEffectView = GetTransparentView();
                UITextField field = null; 
                var posbut = UIAlertAction.Create(positiveButton, UIAlertActionStyle.Default, (obj) =>
                {
                    visualEffectView.RemoveFromSuperview();
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
                            if (config != null )
                            {
                                array2[0].Subviews[0].TintColor = UIColor.Gray;
                                if(config.MinLength != 0)
                                {
                                    posbut.Enabled = false;
                                    field.AddTarget((w, me) =>
                                    {
                                        if (field.Text.Trim().Length >= config.MinLength)
                                        {
                                            pushView.View.TintColor = fontColor.ToUIColor();
                                            //array2[0].Subviews[0].Subviews[1].TintColor = fontColor.ToUIColor();
                                            posbut.Enabled = true;
                                        }
                                        else
                                        {
                                            //array2[0].Subviews[0].TintColor = UIColor.Gray;
                                            posbut.Enabled = false;
                                        }
                                    }, UIControlEvent.EditingChanged);
                                }
                                else
                                {
                                     posbut.Enabled = true;
                                    pushView.View.TintColor = fontColor.ToUIColor();
                                }
                            }
                        }
                        if (config != null)
                        {
                            if (config.keyboard == Keyboard.Telephone)
                                field.KeyboardType = UIKeyboardType.PhonePad;
                            else if (config.keyboard == Keyboard.Text)
                                field.KeyboardType = UIKeyboardType.Default;
                            else if (config.keyboard == Keyboard.Numeric)
                                field.KeyboardType = UIKeyboardType.NumberPad;
                            else if (config.keyboard == Keyboard.Email)
                                field.KeyboardType = UIKeyboardType.EmailAddress;

                            field.Placeholder = config.placeholder;
                            field.TextColor = config.FontColor.ToUIColor();
                            field.BackgroundColor = config.BackgroundColor.ToUIColor();
                            if (config.MinLength != 0)
                            {
                                field.ShouldChangeCharacters = (UITextField sd, NSRange range, string replacementString) =>
                                { 
                                    var length = textField.Text.Length - range.Length + replacementString.Length;
                                    return length <= config.MaxLength;
                                };
                            }
                            if (!string.IsNullOrEmpty(config.DefaultValue))
                                field.Text = config.DefaultValue;
                        }
                        field.AutocorrectionType = UITextAutocorrectionType.No;
                        field.ClearButtonMode = UITextFieldViewMode.WhileEditing;
                        field.ReturnKeyType = UIReturnKeyType.Done;
                        field.BorderStyle = UITextBorderStyle.RoundedRect;
                    }); 

                    var frameRect = field.Frame;
                    frameRect.Size = new CGSize(frameRect.Width, 53);
                    field.Frame = frameRect;
                }
                pushView.AddAction(posbut);


                //Customization/Hack
                UIView[] array = pushView.View.Subviews;

                if (config != null && config.MinLength != 0)
                {
                    //TODO:Need to Fix this
                    //array[0].Subviews[0].TintColor = UIColor.Gray;
                    array[0].Subviews[0].Subviews[1].TintColor = fontColor.ToUIColor();
                }
                else
                {
                    array[0].Subviews[0].TintColor = fontColor.ToUIColor();
                }
                foreach (var item in array[0].Subviews[0].Subviews[0].Subviews)
                {
                    item.BackgroundColor = backgroundcolor.ToUIColor();
                }
                 
                UIStringAttributes bodystyle = new UIStringAttributes
                {
                    ForegroundColor = fontColor.ToUIColor(), 
                };

                if (iscontentleftalign)
                {
                    NSMutableParagraphStyle style = new NSMutableParagraphStyle();
                    style.Alignment = UITextAlignment.Left;
                    bodystyle.ParagraphStyle = style;
                    
                } 

                UIStringAttributes titlecolor = new UIStringAttributes
                {
                    ForegroundColor = fontColor.ToUIColor(),
                    Font = UIFont.FromName("Helvetica-Bold", 18f),
                   
                };

                pushView.SetValueForKey(new NSAttributedString(title, titlecolor), new NSString("attributedTitle"));
                pushView.SetValueForKey(new NSAttributedString(content, bodystyle), new NSString("attributedMessage"));
                if (negativeButton != null)
                {
                    pushView.AddAction(UIAlertAction.Create(negativeButton, UIAlertActionStyle.Cancel, (obj) =>
                    {
                        //visualEffectView.Alpha=0
                        visualEffectView.RemoveFromSuperview();
                        callback(negativeButton);
                    }));
                }
                if (neutralButton != null)
                {
                    pushView.AddAction(UIAlertAction.Create(neutralButton, UIAlertActionStyle.Default, (obj) =>
                    {
                        visualEffectView.RemoveFromSuperview();
                        callback(neutralButton);
                    }));
                }
                window.MakeKeyAndVisible();
                window.RootViewController.PresentViewController(pushView, true, null);  
                window.RootViewController.View.AddSubview(visualEffectView);
            }); 
            return true;
        }

    }
}


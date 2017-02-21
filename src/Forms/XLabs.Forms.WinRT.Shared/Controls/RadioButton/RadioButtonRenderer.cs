// ***********************************************************************
// Assembly         : XLabs.Forms.WinRT.Forms
// Author           : XLabs Team
// Created          : 12-31-2014
// 
// Last Modified By : Eli Black
// Last Modified On : 02-17-2017
// ***********************************************************************
// <copyright file="RadioButtonRenderer.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System.ComponentModel;
using Xamarin.Forms;
using XLabs.Forms.Controls;

#if WINDOWS_PHONE
using Xamarin.Forms.Platform.WinPhone;

#elif WINDOWS_APP || WINDOWS_PHONE_APP
using Xamarin.Forms.Platform.WinRT;

#elif NETFX_CORE
using Xamarin.Forms.Platform.UWP;
#endif

using NativeRadioButton = Windows.UI.Xaml.Controls.RadioButton;

[assembly: ExportRenderer(typeof(CustomRadioButton), typeof(RadioButtonRenderer))]

namespace XLabs.Forms.Controls
{
    public class RadioButtonRenderer : ViewRenderer<CustomRadioButton, Windows.UI.Xaml.Controls.RadioButton>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<CustomRadioButton> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.CheckedChanged -= CheckedChanged;
            }

            if(e.NewElement == null)
            {
                return;
            }

            if (this.Control == null)
            {
                var radioButton = new NativeRadioButton();
                radioButton.Checked += (s, args) => this.Element.Checked = true;
                radioButton.Unchecked += (s, args) => this.Element.Checked = false;

                this.SetNativeControl(radioButton);
            }

            this.Control.Content = e.NewElement.Text;
            this.Control.IsChecked = e.NewElement.Checked;


            this.Element.CheckedChanged += CheckedChanged;
            this.Element.PropertyChanged += ElementOnPropertyChanged;
        }

        private void ElementOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            NativeRadioButton control = this.Control;

            if (control == null)
            {
                return;
            }

            switch (propertyChangedEventArgs.PropertyName)
            {
                case "Checked":
                    control.IsChecked = this.Element.Checked;
                    break;
                case "TextColor":
                    //control.Foreground = this.Element.TextColor ToBrush();
                    break;
                case "Text":
                    control.Content = Element.Text;
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("Property change for {0} has not been implemented.", propertyChangedEventArgs.PropertyName);
                    break;
            }
        }

        private void CheckedChanged(object sender, EventArgs<bool> eventArgs)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.Control.Content = this.Element.Text;
                this.Control.IsChecked = eventArgs.Value;
            });
        }


    }
}
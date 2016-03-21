// ***********************************************************************
// Assembly         : XLabs.Forms.WP8
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
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
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using XLabs.Forms.Controls;
using NativeCheckBox = System.Windows.Controls.RadioButton;

[assembly: ExportRenderer(typeof (CustomRadioButton), typeof (RadioButtonRenderer))]

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Class RadioButtonRenderer.
    /// </summary>
    public class RadioButtonRenderer : ViewRenderer<CustomRadioButton, NativeCheckBox>
    {
        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<CustomRadioButton> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.CheckedChanged -= CheckedChanged;
            }

            if (Control == null)
            {
                var checkBox = new NativeCheckBox();

                checkBox.Checked += (s, args) => Element.Checked = true;
                checkBox.Unchecked += (s, args) => Element.Checked = false;

                SetNativeControl(checkBox);
            }

            UpdateText();
            Control.IsChecked = e.NewElement.Checked;

            UpdateFont();
            Control.Foreground = Element.TextColor.ToBrush();

            Element.CheckedChanged += CheckedChanged;
            Element.PropertyChanged += ElementOnPropertyChanged;
        }

        private void ElementOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case "Checked":
                    Control.IsChecked = Element.Checked;
                    break;
                case "TextColor":
                    Control.Foreground = Element.TextColor.ToBrush();
                    break;
                case "FontName":
                case "FontSize":
                    UpdateFont();
                    break;
                case "Text":
                    UpdateText();
                    break;
                default:
                    Debug.WriteLine("Property change for {0} has not been implemented.",
                        propertyChangedEventArgs.PropertyName);
                    break;
            }
        }

        private void CheckedChanged(object sender, EventArgs<bool> eventArgs)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                UpdateText();
                Control.IsChecked = eventArgs.Value;
            });
        }
        /// <summary>
        /// Updates radio button text content.
        /// </summary>
        private void UpdateText() {
            Control.Content = new TextBlock()
            {
                Text = Element.Text,
                TextWrapping = TextWrapping.Wrap
            };
        }

        /// <summary>
        /// Updates the font.
        /// </summary>
        private void UpdateFont()
        {
            if (!string.IsNullOrEmpty(Element.FontName))
            {
                Control.FontFamily = new FontFamily(Element.FontName);
            }

            Control.FontSize = (Element.FontSize > 0) ? (float)Element.FontSize : 12.0f;
        }
    }
}
// ***********************************************************************
// Assembly         : XLabs.Forms.WP8
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="CircleImageRenderer.cs" company="XLabs Team">
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

using System;
using System.Windows.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(CircleImage), typeof(CircleImageRenderer))]
namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Class CircleImageRenderer.
    /// </summary>
    public class CircleImageRenderer : ImageRenderer
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="CircleImageRenderer"/> class.
        /// </summary>
        public CircleImageRenderer()
        {
        }

        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
        }

        /// <summary>
        /// Handles the <see cref="E:ElementPropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control != null && Control.Clip == null)
            {
                var min = Math.Min(Element.Width, Element.Height) / 2.0f;

                if (min <= 0)
                    return;

                Control.Clip = new EllipseGeometry
                {
                    Center = new System.Windows.Point(min, min),
                    RadiusX = min,
                    RadiusY = min
                };
            }
        }
    }
}


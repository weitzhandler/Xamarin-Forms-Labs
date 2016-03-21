// ***********************************************************************
// Assembly         : XLabs.Platform.WP8
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="Display.cs" company="XLabs Team">
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

using System.Windows;
using Microsoft.Phone.Info;

namespace XLabs.Platform.Device
{
    /// <summary>
    /// Windows Phone 8 Display.
    /// </summary>
    public class Display : IDisplay
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Display" /> class.
        /// </summary>
        /// <remarks>To get accurate display reading application should enable ID_CAP_IDENTITY_DEVICE on app manifest.</remarks>
        public Display()
        {
            object physicalScreenResolutionObject;

            if (DeviceExtendedProperties.TryGetValue("PhysicalScreenResolution", out physicalScreenResolutionObject))
            {
                var physicalScreenResolution = (Size)physicalScreenResolutionObject;
                Height = (int)physicalScreenResolution.Height;
                Width = (int)physicalScreenResolution.Width;
            }
            else
            {
                var scaleFactor = Application.Current.Host.Content.ScaleFactor;
                Height = (int)(Application.Current.Host.Content.ActualHeight * scaleFactor);
                Width = (int)(Application.Current.Host.Content.ActualWidth * scaleFactor);
            }

            object rawDpiX, rawDpiY;

            if (DeviceExtendedProperties.TryGetValue("RawDpiX", out rawDpiX))
            {
                Xdpi = (double)rawDpiX;
            }

            if (DeviceExtendedProperties.TryGetValue("RawDpiY", out rawDpiY))
            {
                Ydpi = (double)rawDpiY;
            }

            //FontManager = new FontManager(this);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents the current <see cref="Display" />.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents the current <see cref="Display" />.</returns>
        public override string ToString()
        {
            return string.Format("[Screen: Height={0}, Width={1}, Xdpi={2:0.0}, Ydpi={3:0.0}]", Height, Width, Xdpi, Ydpi);
        }

        #region IDisplay Members

        /// <summary>
        /// Gets the screen height in pixels
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; private set; }

        /// <summary>
        /// Gets the screen width in pixels
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the screens X pixel density per inch
        /// </summary>
        /// <value>The xdpi.</value>
        public double Xdpi { get; private set; }

        /// <summary>
        /// Gets the screens Y pixel density per inch
        /// </summary>
        /// <value>The ydpi.</value>
        public double Ydpi { get; private set; }

        /// <summary>
        /// Gets the scale value of the display.
        /// </summary>
        public double Scale
        {
            get
            {
                return Application.Current.Host.Content.ScaleFactor / 100d;
            }
        }
        /// <summary>
        /// Convert width in inches to runtime pixels
        /// </summary>
        /// <param name="inches">The inches.</param>
        /// <returns>System.Double.</returns>
        public double WidthRequestInInches(double inches)
        {
            return inches * Xdpi * 100 / Application.Current.Host.Content.ScaleFactor;
        }

        /// <summary>
        /// Convert height in inches to runtime pixels
        /// </summary>
        /// <param name="inches">The inches.</param>
        /// <returns>System.Double.</returns>
        public double HeightRequestInInches(double inches)
        {
            return inches * Ydpi * 100 / Application.Current.Host.Content.ScaleFactor;
        }

        #endregion
    }
}
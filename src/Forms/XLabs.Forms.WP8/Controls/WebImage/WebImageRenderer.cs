// ***********************************************************************
// Assembly         : XLabs.Forms.WP8
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="WebImageRenderer.cs" company="XLabs Team">
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
using System.Windows.Media.Imaging;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(WebImage), typeof(WebImageRenderer))]
namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class WebImageRenderer.
	/// </summary>
	public class WebImageRenderer : ImageRenderer
	{
		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);

			var webImage = (WebImage)Element;

			SetNativeControl(GetImageFromWeb(webImage.ImageUrl));
		}

		/// <summary>
		/// Gets the image from web.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <returns>System.Windows.Controls.Image.</returns>
		private System.Windows.Controls.Image GetImageFromWeb(string url)
		{
			var image = new System.Windows.Controls.Image();

			var uri = new Uri(url, UriKind.Absolute);

			image.Source = new BitmapImage(uri);

			return image;
		}
	}
}
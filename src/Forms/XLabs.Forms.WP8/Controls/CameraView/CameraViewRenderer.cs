// ***********************************************************************
// Assembly         : XLabs.Forms.WP8
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="CameraViewRenderer.cs" company="XLabs Team">
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

using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Devices;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using XLabs.Forms.Controls;
using XLabs.Ioc;
using XLabs.Platform.Mvvm;
using Orientation = XLabs.Enums.Orientation;

[assembly: ExportRenderer(typeof(CameraView), typeof(CameraViewRenderer))]

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class CameraViewRenderer.
	/// </summary>
	public class CameraViewRenderer : ViewRenderer<CameraView, Canvas>
	{
		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<CameraView> e)
		{
			base.OnElementChanged(e);

			System.Diagnostics.Debug.WriteLine(Parent);

			if (Control == null)
			{
				
				// TODO: determine how to dispose the camera...
				var camera = new PhotoCamera((CameraType)((int)e.NewElement.Camera));

				var app = Resolver.Resolve<IXFormsApp>();

				var rotation = camera.Orientation;
				switch (app.Orientation)
				{
					case Orientation.LandscapeLeft:
						rotation -= 90;
						break;
					case Orientation.LandscapeRight:
						rotation += 90;
						break;
				}

				var videoBrush = new VideoBrush {
					RelativeTransform = new CompositeTransform()
					{
						CenterX = 0.5,
						CenterY = 0.5,
						Rotation = rotation
					}
				};
				
				
				videoBrush.SetSource(camera);

				var canvas = new Canvas
				{
					Background = videoBrush
				};

				SetNativeControl(canvas);
			}
		}

		/// <summary>
		/// Handles the <see cref="E:ElementPropertyChanged" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			switch (e.PropertyName)
			{
				case "Camera":
					var brush = Control.Background as VideoBrush;
					var camera = new PhotoCamera((CameraType)((int)Element.Camera));
					brush.SetSource(camera);
					break;
				default:
					break;
			}
		}
	}
}

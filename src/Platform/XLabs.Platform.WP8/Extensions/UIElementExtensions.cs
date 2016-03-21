// ***********************************************************************
// Assembly         : XLabs.Platform.WP8
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="UIElementExtensions.cs" company="XLabs Team">
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

using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace XLabs.Platform
{
	/// <summary>
	/// Class UiElementExtensions.
	/// </summary>
	public static class UiElementExtensions
	{
		/// <summary>
		/// Streams to JPEG.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <param name="stream">The stream.</param>
		/// <returns>Task.</returns>
		public static Task StreamToJpeg(this UIElement view, Stream stream)
		{
			return
				Task.Run(() => view.ToBitmap().SaveJpeg(stream, (int)view.RenderSize.Width, (int)view.RenderSize.Height, 0, 100));
		}

		/// <summary>
		/// To the bitmap.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <returns>WriteableBitmap.</returns>
		public static WriteableBitmap ToBitmap(this UIElement view)
		{
			return new WriteableBitmap(view, null);
		}
	}
}
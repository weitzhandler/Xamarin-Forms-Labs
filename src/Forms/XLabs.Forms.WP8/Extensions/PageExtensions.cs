// ***********************************************************************
// Assembly         : XLabs.Forms.WP8
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="PageExtensions.cs" company="XLabs Team">
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

using Microsoft.Phone.Controls;
using XLabs.Ioc;
using XLabs.Platform.Mvvm;

namespace XLabs.Forms.Extensions
{
	/// <summary>
	/// Class PageExtensions.
	/// </summary>
	public static class PageExtensions
	{
		/// <summary>
		/// Sets the orientation.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <param name="orientation">The orientation.</param>
		public static void SetOrientation(this PhoneApplicationPage page, PageOrientation? orientation = null)
		{
			var app = Resolver.Resolve<IXFormsApp>() as XFormsAppWP;

			if (app != null)
			{
				app.SetOrientation(orientation ?? page.Orientation);
			}
		}
	}
}
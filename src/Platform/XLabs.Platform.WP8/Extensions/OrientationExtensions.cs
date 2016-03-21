// ***********************************************************************
// Assembly         : XLabs.Platform.WP8
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="OrientationExtensions.cs" company="XLabs Team">
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
using XLabs.Enums;

namespace XLabs.Platform
{
	/// <summary>
	/// Class OrientationExtensions.
	/// </summary>
	public static class OrientationExtensions
	{
		/// <summary>
		/// To the orientation.
		/// </summary>
		/// <param name="orientation">The orientation.</param>
		/// <returns>Orientation.</returns>
		public static Orientation ToOrientation(this PageOrientation orientation)
		{
			return (Orientation)((int)orientation);
		}

		/// <summary>
		/// To the page orientation.
		/// </summary>
		/// <param name="orientation">The orientation.</param>
		/// <returns>PageOrientation.</returns>
		public static PageOrientation ToPageOrientation(this Orientation orientation)
		{
			return (PageOrientation)((int)orientation);
		}
	}
}
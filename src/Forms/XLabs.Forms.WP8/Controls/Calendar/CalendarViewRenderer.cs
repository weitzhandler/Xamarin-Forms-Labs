// ***********************************************************************
// Assembly         : XLabs.Forms.WP8
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="CalendarViewRenderer.cs" company="XLabs Team">
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

using WPControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(CalendarView), typeof(CalendarViewRenderer))]
namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class CalendarViewRenderer.
	/// </summary>
	public class CalendarViewRenderer : ViewRenderer<CalendarView, WPControls.Calendar>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CalendarViewRenderer"/> class.
		/// </summary>
		public CalendarViewRenderer()
		{

		}

		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<CalendarView> e)
		{
			base.OnElementChanged(e);
			var calendar = new WPControls.Calendar();
			calendar.DateClicked +=
				(object sender, SelectionChangedEventArgs es) =>
				{
					Element.NotifyDateSelected(es.SelectedDate);
				};
			this.SetNativeControl(calendar);
		}
	}
}

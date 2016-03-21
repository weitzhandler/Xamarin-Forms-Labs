// ***********************************************************************
// Assembly         : XLabs.Forms.WP8
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ExtendedScrollViewRenderer.cs" company="XLabs Team">
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
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedScrollView), typeof(ExtendedScrollViewRenderer))]
namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class ExtendedScrollViewRenderer.
	/// </summary>
	public class ExtendedScrollViewRenderer : ScrollViewRenderer
	{
		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<ScrollView> e)
		{
			base.OnElementChanged(e);

			LayoutUpdated += (sender, ev) =>
			{
				var scrollView = (ExtendedScrollView)Element;
				var bounds = new Rectangle(Control.HorizontalOffset, Control.VerticalOffset, Control.ScrollableWidth, Control.ScrollableHeight);
				scrollView.UpdateBounds(bounds);
			};

			if (e.OldElement != null)
				e.OldElement.PropertyChanged -= OnElementPropertyChanged;

			e.NewElement.PropertyChanged += OnElementPropertyChanged;
		}

		/// <summary>
		/// The _epsilon
		/// </summary>
		double _epsilon = 0.1;

		/// <summary>
		/// Handles the <see cref="E:ElementPropertyChanged" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == ExtendedScrollView.PositionProperty.PropertyName)
			{
				var scrollView = (ExtendedScrollView)Element;
				var position = scrollView.Position;

				if (Math.Abs(Control.VerticalOffset - position.Y) < _epsilon
					&& Math.Abs(Control.HorizontalOffset - position.X) < _epsilon)
					return;

				Control.ScrollToVerticalOffset(position.Y);
				Control.UpdateLayout();
			}
		}

	}
}


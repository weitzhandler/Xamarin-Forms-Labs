// ***********************************************************************
// Assembly         : XLabs.Forms.WP8
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="SensorBarControl.xaml.cs" company="XLabs Team">
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
using System.Windows.Controls;
using System.Windows.Media;

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class SensorBarControl.
	/// </summary>
	public partial class SensorBarControl : UserControl
	{
		/// <summary>
		/// The positive color
		/// </summary>
		private System.Windows.Media.Color _positiveColor = System.Windows.Media.Color.FromArgb(255, 0, 255, 0);
		/// <summary>
		/// The negative color
		/// </summary>
		private System.Windows.Media.Color _negativeColor = System.Windows.Media.Color.FromArgb(255, 255, 0, 0);
		/// <summary>
		/// The limit
		/// </summary>
		private double _limit = 1;
		/// <summary>
		/// The current value
		/// </summary>
		private double _currentValue = 0;

		/// <summary>
		/// Initializes a new instance of the <see cref="SensorBarControl"/> class.
		/// </summary>
		public SensorBarControl()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Gets or sets the current value.
		/// </summary>
		/// <value>The current value.</value>
		[Browsable(true)]
		public double CurrentValue
		{
			get { return _currentValue; }
			set
			{
				if (Math.Abs(value) <= Limit)
				{
					_currentValue = value;

				}
			}
		}

		/// <summary>
		/// Gets or sets the limit.
		/// </summary>
		/// <value>The limit.</value>
		[Browsable(true)]
		public double Limit
		{
			get { return _limit; }
			set { _limit = value; }
		}

		/// <summary>
		/// Gets or sets the color of the positive.
		/// </summary>
		/// <value>The color of the positive.</value>
		[Browsable(true)]
		public System.Windows.Media.Color PositiveColor
		{
			get { return _negativeColor; }
			set 
			{
				_negativeColor = value;
				PositiveRectangle.Fill = new SolidColorBrush(value); 
			}
		}

		/// <summary>
		/// Gets or sets the color of the negative.
		/// </summary>
		/// <value>The color of the negative.</value>
		[ Browsable(true)]
		public System.Windows.Media.Color NegativeColor
		{
			get { return _negativeColor; }
			set 
			{ 
				_negativeColor = value;
				NegativeRectangle.Fill = new SolidColorBrush(value); 
			}
		}
	}
}

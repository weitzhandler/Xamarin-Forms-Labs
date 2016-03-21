// ***********************************************************************
// Assembly         : XLabs.Platform.WP8
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="Gyroscope.cs" company="XLabs Team">
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
using Microsoft.Devices.Sensors;

namespace XLabs.Platform.Device
{
	/// <summary>
	/// Class Gyroscope.
	/// </summary>
	public partial class Gyroscope
	{
		/// <summary>
		/// The _gyroscope
		/// </summary>
		private Microsoft.Devices.Sensors.Gyroscope _gyroscope;

		/// <summary>
		/// Gets or sets the interval.
		/// </summary>
		/// <value>The interval.</value>
		public AccelerometerInterval Interval { get; set; }

		/// <summary>
		/// Starts this instance.
		/// </summary>
		partial void Start()
		{
			_gyroscope = new Microsoft.Devices.Sensors.Gyroscope
				             {
					             TimeBetweenUpdates = TimeSpan.FromMilliseconds((long)Interval)
				             };

			_gyroscope.CurrentValueChanged += GyroscopeCurrentValueChanged;
			_gyroscope.Start();
		}

		/// <summary>
		/// Stops this instance.
		/// </summary>
		partial void Stop()
		{
			_gyroscope.CurrentValueChanged -= GyroscopeCurrentValueChanged;
			_gyroscope.Stop();
			_gyroscope = null;
		}

		/// <summary>
		/// Gyroscopes the current value changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The e.</param>
		private void GyroscopeCurrentValueChanged(object sender, SensorReadingEventArgs<GyroscopeReading> e)
		{
			if (_gyroscope.IsDataValid)
			{
				LatestReading = e.SensorReading.RotationRate.AsVector3();
				readingAvailable.Invoke(this, this.LatestReading);
			}
		}
	}
}
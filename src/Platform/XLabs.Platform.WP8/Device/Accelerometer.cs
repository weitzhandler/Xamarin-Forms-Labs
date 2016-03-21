// ***********************************************************************
// Assembly         : XLabs.Platform.WP8
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="Accelerometer.cs" company="XLabs Team">
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
	/// Class Accelerometer.
	/// </summary>
	public partial class Accelerometer
	{
		/// <summary>
		/// The accelerometer
		/// </summary>
		private Microsoft.Devices.Sensors.Accelerometer accelerometer;

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
			this.accelerometer = new Microsoft.Devices.Sensors.Accelerometer
				                 {
					                 TimeBetweenUpdates =
						                 TimeSpan.FromMilliseconds((long)Interval)
				                 };

			this.accelerometer.CurrentValueChanged += AccelerometerOnCurrentValueChanged;
			this.accelerometer.Start();
		}

		/// <summary>
		/// Stops this instance.
		/// </summary>
		partial void Stop()
		{
			if (this.accelerometer != null)
			{
				this.accelerometer.CurrentValueChanged -= AccelerometerOnCurrentValueChanged;
				this.accelerometer.Stop();
				this.accelerometer = null;
			}
		}

		/// <summary>
		/// Accelerometers the on current value changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="sensorReadingEventArgs">The sensor reading event arguments.</param>
		private void AccelerometerOnCurrentValueChanged(
			object sender,
			SensorReadingEventArgs<AccelerometerReading> sensorReadingEventArgs)
		{
			LatestReading = sensorReadingEventArgs.SensorReading.Acceleration.AsVector3();
			readingAvailable.Invoke(sender, LatestReading);
		}
	}
}
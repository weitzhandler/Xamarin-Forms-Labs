using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLabs.Platform.Device;

namespace XLabs.Platform.Device
{
	public partial class Accelerometer : IAccelerometer
	{
		public AccelerometerInterval Interval { get; set; }
	}
}

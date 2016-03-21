using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLabs.Platform.Device;

namespace XLabs.Platform.Device
{
	/// <summary>
	///     Battery portion for Windows devices.
	/// </summary>
	public partial class Battery
	{
		public int Level { get; }
		public bool Charging { get; }
	}
}

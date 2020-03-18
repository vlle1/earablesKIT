using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EarablesKIT.Models.AudioService
{
	public interface IAudioService
	{
		Task Speak(string message);
	}
}

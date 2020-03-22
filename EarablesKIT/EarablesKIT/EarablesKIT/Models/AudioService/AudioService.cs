using System.Threading.Tasks;
using Xamarin.Essentials;

namespace EarablesKIT.Models.AudioService
{
    class AudioService : IAudioService
    {
        public async Task Speak(string message)
        {
            await TextToSpeech.SpeakAsync(message);
        }
    }
}

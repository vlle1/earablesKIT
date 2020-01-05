using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace EarablesKIT.Models.SettingsService
{
    class SettingsService : ISettingsService
    {
        public CultureInfo ActiveLanguage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public SamplingRate SamplingRate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public User MyUser { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        public SettingsService()
        {
            throw new NotImplementedException();
        }

        private void loadSettings()
        {
            throw new NotImplementedException();
        }
    }
}

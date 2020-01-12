using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EarablesKIT.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EarablesKIT.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StepModePage : ContentPage
    {
        public StepModePage()
        {
            InitializeComponent();
			BindingContext = new StepModeViewModel();
        }
    }
}
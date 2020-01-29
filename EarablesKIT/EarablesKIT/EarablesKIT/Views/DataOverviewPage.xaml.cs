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
    public partial class DataOverviewPage : ContentPage
    {
        /// <summary>
        /// Default constructor for page DataOverview
        /// </summary>
        public DataOverviewPage()
        {
            InitializeComponent();
            BindingContext = new DataOverviewViewModel();
        }

    }
}
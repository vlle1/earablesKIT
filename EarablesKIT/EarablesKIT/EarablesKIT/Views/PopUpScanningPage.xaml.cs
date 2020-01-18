using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EarablesKIT.ViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EarablesKIT.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpScanningPage : PopupPage
    {

        private ScanningPopUpViewModel _viewModel;
        
        public PopUpScanningPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ScanningPopUpViewModel();
        }


        private void OnScanningButtonPressed(object sender, EventArgs e)
        {
            _viewModel.ScanDevicesCommand.Execute(this);
            if (_viewModel.DevicesList.Count != 0)
            {
                DevicesListView.SelectedItem = _viewModel.DevicesList[0];
            }
        }
    }
}
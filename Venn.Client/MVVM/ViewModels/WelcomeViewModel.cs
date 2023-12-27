using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venn.Client.Services;

namespace Venn.Client.MVVM.ViewModels
{
    internal class WelcomeViewModel : ViewModelBase
    {
        public SnackbarMessageQueue SnackbarMessageQueue { set; get; } = new(TimeSpan.FromSeconds(1));

        public INavigationService NavigationService { get; set; }

        public RelayCommand ToLoginViewCommand { get; set; }

        public RelayCommand ToCreateAccountViewCommand { get; set; }

        public WelcomeViewModel(INavigationService NavigationService)
        {
            this.NavigationService = NavigationService;
            ToLoginViewCommand = new RelayCommand(ToLoginView);
            ToCreateAccountViewCommand = new RelayCommand(ToCreateAccountView);
        }

        public void ToLoginView()
        {
            NavigationService.NavigateTo<LoginViewModel>();
        }

        public void ToCreateAccountView()
        {
            NavigationService.NavigateTo<CreateAccountViewModel>();
        }

        public void SnackbarEnqueue(string msg, string btnContent = "", Action btnAction = null, double duration = 1)
        {
            SnackbarMessageQueue.Enqueue(msg,
            btnContent,
            _ => btnAction?.Invoke(), actionArgument: null,
            promote: false, neverConsiderToBeDuplicate: false,
            durationOverride: TimeSpan.FromSeconds(duration));
        }
    }
}

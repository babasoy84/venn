using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
        public INavigationService NavigationService { get; set; }

        public RelayCommand ToLoginViewCommand { get; set; }

        public RelayCommand ToCreateTeamViewCommand { get; set; }

        public WelcomeViewModel(INavigationService NavigationService)
        {
            this.NavigationService = NavigationService;
            ToLoginViewCommand = new RelayCommand(ToLoginView);
            ToCreateTeamViewCommand = new RelayCommand(ToCreateTeamView);
        }

        public void ToLoginView()
        {
            NavigationService.NavigateTo<LoginViewModel>();
        }

        public void ToCreateTeamView()
        {
            NavigationService.NavigateTo<CreateTeamViewModel>();
        }
    }
}

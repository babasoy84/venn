using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venn.Client.Messages;
using Venn.Client.Services;
using Venn.Client.Net;
using Venn.Models.Models.Concretes;

namespace Venn.Client.MVVM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ServerHelper server;

        private INavigationService navigationService;

        private User user;

        private ViewModelBase currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get => currentViewModel;
            set => Set(ref currentViewModel, value);    
        }


        public MainViewModel(IMessenger messenger, INavigationService navigationService)
        {
            this.navigationService = navigationService;
            server = App.Container.GetInstance<ServerHelper>();
            server.ConnectToServer();
            messenger.Register<NavigationMessage>(this, message =>
            {
                var viewModel = App.Container.GetInstance(message.ViewModelType) as ViewModelBase;
                CurrentViewModel = viewModel;
            });
            this.navigationService.NavigateTo<WelcomeViewModel>();
        }
    }
}

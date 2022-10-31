using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using Venn.Client.MVVM.ViewModels;
using Venn.Client.MVVM.Views;
using Venn.Client.Net;
using Venn.Client.Services;
using Venn.Data;
using Venn.Data.Repos;
using Venn.Models.Models.Concretes;

namespace Venn.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Container Container;

        protected override void OnStartup(StartupEventArgs e)
        {
            Register();
            StartMain<MainViewModel>();
            base.OnStartup(e);
        }

        private void Register()
        {
            Container = new Container();


            Container.RegisterSingleton<INavigationService, NavigationService>();
            Container.RegisterSingleton<IMessenger, Messenger>();

            Container.RegisterSingleton<MainViewModel>();
            Container.RegisterSingleton<WelcomeViewModel>();
            Container.RegisterSingleton<LoginViewModel>();
            Container.RegisterSingleton<CreateTeamViewModel>();
            Container.RegisterSingleton<ChatViewModel>();
            Container.RegisterSingleton<ServerHelper>();
            Container.RegisterSingleton<User>();

            Container.Verify();
        }

        public void StartMain<T>() where T : ViewModelBase
        {
            Window window = new MainView();
            var viewModel = Container.GetInstance<T>();
            window.DataContext = viewModel;
            window.ShowDialog();
        }
    }
}

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Venn.Client.Net;
using Venn.Client.Services;

namespace Venn.Client.MVVM.ViewModels
{
    class EmailVerificationViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string vertificationCode = "";

        private string digit1;

        private string digit2;

        private string digit3;

        private string digit4;

        private string digit5;

        private string digit6;

        private string errorTextVisibility = "Collapsed";

        public string Digit1
        {
            get { return digit1; }
            set
            {
                digit1 = value;
                NotifyPropertyChanged("Digit1");
            }
        }

        public string Digit2
        {
            get { return digit2; }
            set
            {
                digit2 = value;
                NotifyPropertyChanged("Digit2");
            }
        }

        public string Digit3
        {
            get { return digit3; }
            set
            {
                digit3 = value;
                NotifyPropertyChanged("Digit3");
            }
        }

        public string Digit4
        {
            get { return digit4; }
            set
            {
                digit4 = value;
                NotifyPropertyChanged("Digit4");
            }
        }

        public string Digit5
        {
            get { return digit5; }
            set
            {
                digit5 = value;
                NotifyPropertyChanged("Digit5");
            }
        }

        public string Digit6
        {
            get { return digit6; }
            set
            {
                digit6 = value;
                NotifyPropertyChanged("Digit6");
            }
        }

        public string ErrorTextVisibility
        {
            get { return errorTextVisibility; }
            set
            {
                errorTextVisibility = value;
                NotifyPropertyChanged("ErrorTextVisibility");
            }
        }


        public string Type { get; set; } = "";

        public string Email { get; set; } = "";

        public INavigationService NavigationService { get; set; }

        public ServerHelper Server { get; set; }

        public RelayCommand ToBackViewCommand { get; set; }

        public RelayCommand VerifyCommand { get; set; }

        public RelayCommand ResendMailCommand { get; set; }

        public EmailVerificationViewModel(INavigationService NavigationService, ServerHelper Server)
        {
            this.NavigationService = NavigationService;
            this.Server = Server;

            ToBackViewCommand = new RelayCommand(ToBackView);
            VerifyCommand = new RelayCommand(Verify);
            ResendMailCommand = new RelayCommand(() => Task.Run(() => SendVertificationCode()));
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void ToBackView()
        {
            Digit1 = "";
            Digit2 = "";
            Digit3 = "";
            Digit4 = "";
            Digit5 = "";
            Digit6 = "";

            ErrorTextVisibility = "Collapsed";

            if (Type == "Reset Password")
                NavigationService.NavigateTo<ForgotPasswordViewModel>();
            else
                NavigationService.NavigateTo<CreateAccountViewModel>();
        }

        public void SendVertificationCode()
        {
            Digit1 = "";
            Digit2 = "";
            Digit3 = "";
            Digit4 = "";
            Digit5 = "";
            Digit6 = "";

            vertificationCode = GenerateVertificationCode();

            string body = $@"
            <html>
                <head>
                    <style>
                        body {{
                            font-family: 'Arial', sans-serif;
                            background-color: #f4f4f4;
                            text-align: center;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: 20px auto;
                            padding: 20px;
                            background-color: #ffffff;
                            border-radius: 8px;
                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                        }}
                        h2 {{
                            color: #333333;
                        }}
                        p {{
                            color: #555555;
                        }}
                        .verification-code {{
                            font-size: 24px;
                            font-weight: bold;
                            color: #007bff;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h2>Vertification Code:</h2>
                        <p class='verification-code'>{vertificationCode}</p>
                    </div>
                </body>
            </html>";

            EmailService.SendMail(Email, "Venn - Vertification Code", body);
        }

        public string GenerateVertificationCode()
        {
            Random random = new Random();
            int randomNumber = random.Next(100000, 1000000);

            return randomNumber.ToString();
        }

        private async void Verify()
        {
            int code = int.Parse(Digit1);
            code *= 10;
            code += int.Parse(Digit2);
            code *= 10;
            code += int.Parse(Digit3);
            code *= 10;
            code += int.Parse(Digit4);
            code *= 10;
            code += int.Parse(Digit5);
            code *= 10;
            code += int.Parse(Digit6);

            Digit1 = "";
            Digit2 = "";
            Digit3 = "";
            Digit4 = "";
            Digit5 = "";
            Digit6 = "";

            if (code.ToString() == vertificationCode)
            {
                ErrorTextVisibility = "Collapsed";
                if (Type == "Reset Password")
                {
                    NavigationService.NavigateTo<ResetPasswordViewModel>();
                    App.Container.GetInstance<ResetPasswordViewModel>().Email = Email;
                }
                else
                {
                    NavigationService.NavigateTo<WelcomeViewModel>();
                    App.Container.GetInstance<CreateAccountViewModel>().CreateAccount();
                    App.Container.GetInstance<WelcomeViewModel>().SnackbarEnqueue("Your Venn account has been successfully created.");
                }
            }
            else
            {
                ErrorTextVisibility = "Visible";
            }
        }
    }
}

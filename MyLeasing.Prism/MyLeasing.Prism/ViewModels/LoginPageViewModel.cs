using MyLeasing.Common.Models;
using MyLeasing.Common.Services;
using Prism.Commands;
using Prism.Navigation;

namespace MyLeasing.Prism.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _loginCommand;
        private string _password;

        public LoginPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Login";
            IsEnabled = true;

            Email = "jzuluaga55@hotmail.com";
            Password = "123456";
        }

        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(Login));

        public string Email { get; set; }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must enter an email.", "Accept");
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must enter a password.", "Accept");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var url = App.Current.Resources["UrlAPI"].ToString();
            var connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsEnabled = true;
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "Check the internet connection.", "Accept");
                return;
            }


            var request = new TokenRequest
            {
                Password = Password,
                Username = Email
            };

            var response = await _apiService.GetTokenAsync(url, "Account", "/CreateToken", request);

            if (!response.IsSuccess)
            {
                IsEnabled = true;
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "User or password incorrect.", "Accept");
                Password = string.Empty;
                return;
            }

            var token = response.Result;
            
            await _navigationService.NavigateAsync("PropertiesPage");

            IsEnabled = true;
            IsRunning = false;

        }
    }
}

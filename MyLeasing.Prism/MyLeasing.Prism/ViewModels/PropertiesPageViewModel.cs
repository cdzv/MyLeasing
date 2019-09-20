using MyLeasing.Common.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyLeasing.Prism.ViewModels
{
    public class PropertiesPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private OwnerResponse _owner;
        private TokenResponse _token;

        public PropertiesPageViewModel(INavigationService navigationService):base(navigationService)
        {
            _navigationService = navigationService;
            Title = $"Properties of: {_owner.FullName}";
        }


    }
}

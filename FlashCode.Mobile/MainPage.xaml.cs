using FlashCode.Mobile.Models;
using FlashCode.Mobile.Services;
using Microsoft.Maui.Storage;

namespace FlashCode.Mobile
{
    public partial class MainPage : ContentPage
    {
        private readonly RegistrationService _service;
        private const string TokenKey = "user_token";

        public MainPage()
        {
            InitializeComponent();
            _service = Application.Current!.Handler.MauiContext.Services.GetService<RegistrationService>()!; // Update this line
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var token = await SecureStorage.Default.GetAsync(TokenKey);
            if (!string.IsNullOrEmpty(token))
            {
                var profile = await _service.GetProfileAsync(token);
                if (profile != null)
                {
                    FirstNameEntry.Text = profile.FirstName;
                    CompanyEntry.Text = profile.Company;
                    EmailEntry.Text = profile.Email;
                    ContactCheckbox.IsChecked = profile.AcceptContact;
                }
            }
        }

        private async void OnSubmit(object sender, EventArgs e)
        {
            var token = await SecureStorage.Default.GetAsync(TokenKey);
            var dto = new MobileRegisterDto
            {
                Token = token,
                FirstName = FirstNameEntry.Text ?? string.Empty,
                Company = CompanyEntry.Text ?? string.Empty,
                Email = EmailEntry.Text ?? string.Empty,
                AcceptContact = ContactCheckbox.IsChecked
            };

            var result = await _service.RegisterAsync(dto);
            if (result.Success)
            {
                await SecureStorage.Default.SetAsync(TokenKey, result.Token);
                await DisplayAlert("Succès", "Inscription enregistrée", "OK");
            }
            else
            {
                await DisplayAlert("Erreur", result.Error ?? "Erreur inconnue", "OK");
            }
        }
    }
}

using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Graphics;

namespace FlashCode.Mobile;

public class LoginPage : ContentPage
{
    private readonly Entry _firstName = new();
    private readonly Entry _company = new();
    private readonly Entry _email = new();
    private readonly CheckBox _optIn = new();
    private readonly Button _submit = new() { Text = "Valider" };
    private readonly Label _error = new() { TextColor = Colors.Red };
    private readonly ApiClient _api = new();

    public LoginPage()
    {
        Title = "FlashCode";
        Content = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Padding = 20,
                Children =
                {
                    new Label { Text = "🎯 Participez au défi FlashCode", FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Prénom :" },
                    _firstName,
                    new Label { Text = "Entreprise :" },
                    _company,
                    new Label { Text = "Email professionnel :" },
                    _email,
                    new HorizontalStackLayout
                    {
                        Children =
                        {
                            _optIn,
                            new Label { Text = "J’accepte d’être contacté dans le cadre de ce challenge." }
                        }
                    },
                    new Label { Text = "📩 Vous recevrez un mail uniquement si vous faites partie des gagnants, ou pour recevoir votre bonus de participation." },
                    _submit,
                    _error
                }
            }
        };

        _submit.Clicked += OnSubmitClicked;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var token = await SecureStorage.Default.GetAsync("user_token");
        if (!string.IsNullOrEmpty(token))
        {
            try
            {
                var user = await _api.GetUserAsync(token);
                _firstName.Text = user.FirstName;
                _company.Text = user.Company;
                _email.Text = user.Email;
                _optIn.IsChecked = user.AcceptContact;
            }
            catch (Exception ex)
            {
                _error.Text = ex.Message;
            }
        }
    }

    private async void OnSubmitClicked(object? sender, EventArgs e)
    {
        _error.Text = string.Empty;
        _submit.IsEnabled = false;
        try
        {
            var token = await SecureStorage.Default.GetAsync("user_token");
            var response = await _api.RegisterAsync(new RegisterRequest
            {
                Token = token,
                FirstName = _firstName.Text ?? string.Empty,
                Company = _company.Text ?? string.Empty,
                Email = _email.Text ?? string.Empty,
                AcceptContact = _optIn.IsChecked
            });
            await SecureStorage.Default.SetAsync("user_token", response.Token);
            await DisplayAlert("Succès", "Inscription réussie", "OK");
        }
        catch (Exception ex)
        {
            _error.Text = ex.Message;
        }
        finally
        {
            _submit.IsEnabled = true;
        }
    }
}

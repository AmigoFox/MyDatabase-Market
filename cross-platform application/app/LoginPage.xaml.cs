namespace app.Services.Api;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        await _vm.Login(LoginEntry.Text, PasswordEntry.Text);
    }
}
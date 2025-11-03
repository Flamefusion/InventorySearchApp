
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InventorySearchApp.Services;
using InventorySearchApp.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace InventorySearchApp.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [RelayCommand]
        private async Task LoginAsync()
        {
            var supabaseUrl = DotNetEnv.Env.GetString("SUPABASE_URL");
            var supabaseApiKey = DotNetEnv.Env.GetString("SUPABASE_API_KEY");

            var supabase = new Supabase.Client(supabaseUrl, supabaseApiKey);
            var session = await supabase.Auth.SignIn(Email, Password);

            if (session != null)
            {
                AppDataService.SupabaseClient = supabase;
                var shell = (Application.Current as App)?.m_window as Shell;
                (shell?.Content as Frame)?.Navigate(typeof(MainPage));
            }
            else
            {
                ErrorMessage = "Invalid login credentials.";
            }
        }
    }
}


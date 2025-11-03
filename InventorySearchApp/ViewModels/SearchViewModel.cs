
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InventorySearchApp.Models;
using InventorySearchApp.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace InventorySearchApp.ViewModels
{
    public partial class SearchViewModel : ObservableObject
    {
        [ObservableProperty]
        private string serialNumbers = string.Empty;

        public ObservableCollection<SerialNumber> SearchResults { get; } = new();

        [RelayCommand]
        private async Task SearchAsync()
        {
            if (string.IsNullOrEmpty(SerialNumbers))
            {
                return;
            }

            var serials = SerialNumbers.Split('\n').ToList();
            var supabaseService = new SupabaseService();
            var results = await supabaseService.GetSerialNumbers(serials);

            SearchResults.Clear();
            foreach (var result in results)
            {
                SearchResults.Add(result);
            }
        }
    }
}

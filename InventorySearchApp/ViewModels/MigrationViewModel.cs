
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InventorySearchApp.Models;
using InventorySearchApp.Services;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

namespace InventorySearchApp.ViewModels
{
    public partial class MigrationViewModel : ObservableObject
    {
        [ObservableProperty]
        private string jsonCredentials = string.Empty;

        [ObservableProperty]
        private string selectedFileName = "No file selected";

        [RelayCommand]
        private async Task PickFileAsync()
        {
            var filePicker = new FileOpenPicker();
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle((Application.Current as App)?.m_window);
            WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);
            filePicker.FileTypeFilter.Add(".json");
            var file = await filePicker.PickSingleFileAsync();
            if (file != null)
            {
                SelectedFileName = file.Name;
                JsonCredentials = await File.ReadAllTextAsync(file.Path);
            }
        }

        [RelayCommand]
        private async Task StartMigrationAsync()
        {
            if (string.IsNullOrEmpty(JsonCredentials))
            {
                Debug.WriteLine("Please select a credentials file.");
                return;
            }

            var spreadsheetId = DotNetEnv.Env.GetString("SPREADSHEET_ID");
            var sheetName = DotNetEnv.Env.GetString("SHEET_NAME");

            var googleSheetsService = new GoogleSheetsService(JsonCredentials);
            var data = await googleSheetsService.ReadData(spreadsheetId, sheetName, "C:ZZ");

            if (data != null && data.Count > 0)
            {
                var serialNumbers = new List<SerialNumber>();
                var headerRow = data[0];
                for (int i = 1; i < data.Count; i++)
                {
                    for (int j = 0; j < data[i].Count; j++)
                    {
                        serialNumbers.Add(new SerialNumber
                        {
                            Serial = data[i][j].ToString(),
                            Box = headerRow[j].ToString()
                        });
                    }
                }

                var supabaseService = new SupabaseService();
                await supabaseService.InsertData(serialNumbers);

                Debug.WriteLine("Migration completed successfully.");
            }
            else
            {
                Debug.WriteLine("No data found.");
            }
        }
    }
}

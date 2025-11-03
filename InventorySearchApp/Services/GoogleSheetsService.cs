
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace InventorySearchApp.Services
{
    public class GoogleSheetsService
    {
        private const string ApplicationName = "Inventory Search App";
        private readonly SheetsService _sheetsService;

        public GoogleSheetsService(string credentialsJson)
        {
            GoogleCredential credential;
            using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(credentialsJson)))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(SheetsService.Scope.SpreadsheetsReadonly);
            }

            _sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }

        public async Task<IList<IList<object>>> ReadData(string spreadsheetId, string sheetName, string range)
        {
            var request = _sheetsService.Spreadsheets.Values.Get(spreadsheetId, $"{sheetName}!{range}");
            var response = await request.ExecuteAsync();
            return response.Values;
        }
    }
}

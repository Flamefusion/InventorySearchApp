
using InventorySearchApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventorySearchApp.Services
{
    public class SupabaseService
    {
        public async Task InsertData(List<SerialNumber> serialNumbers)
        {
            if (AppDataService.SupabaseClient == null) return;
            foreach (var serialNumber in serialNumbers)
            {
                await AppDataService.SupabaseClient.From<SerialNumber>().Insert(serialNumber);
            }
        }

        public async Task<List<SerialNumber>> GetSerialNumbers(List<string> serials)
        {
            if (AppDataService.SupabaseClient == null) return new List<SerialNumber>();
            var response = await AppDataService.SupabaseClient.From<SerialNumber>().Filter("serial", Postgrest.Constants.Operator.In, serials).Get();
            return response.Models;
        }
    }
}

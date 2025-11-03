
using Postgrest.Models;
using Postgrest.Attributes;

namespace InventorySearchApp.Models
{
    [Table("serial_numbers")]
    public class SerialNumber : BaseModel
    {
        [PrimaryKey("serial")]
        public string? Serial { get; set; }

        [Column("box")]
        public string? Box { get; set; }
    }
}

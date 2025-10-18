using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenNotes.Common.Models
{
    public class SettingModel
    {
        public int Id { get; set; }
        public string? ApiKey { get; set; }

        public string? BaseUrl { get; set; }
    }
}

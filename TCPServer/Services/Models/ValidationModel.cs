using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{

    public class ValidationViewModel
    {
        public bool Demo { get; set; }

        public required string TaxID { get; set; }

        public required string PrivateKey { get; set; }
    }
}

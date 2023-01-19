using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_Proyectos_BE
{
    public class dto_Error
    {
        public int ErrorNumber { get; set; }
        public string ErrorMsg { get; set; }
        public int RowKey { get; set; }
    }
}

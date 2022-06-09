using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_Proyectos_BE
{
    public class Grafico_BE
    {
       

    }

    public class Chart
    {
        public string[] labels { get; set; }
        public List<Datasets> datasets { get; set; }
    }

    public class Datasets
    {
        public string label { get; set; }
        public string[] backgroundColor { get; set; }
        public int[] data { get; set; }
        public int hoverOffset { get; set; }
    }

}

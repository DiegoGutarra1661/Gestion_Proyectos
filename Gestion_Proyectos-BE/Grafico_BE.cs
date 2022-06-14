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
        public List<DatasetsChart> datasets { get; set; }
    }

    public class DatasetsChart
    {
        public string label { get; set; }
        public string[] backgroundColor { get; set; }
        public int[] data { get; set; }
        public int hoverOffset { get; set; }
        public string borderColor { get; set; }
    }

    public class Bar
    {
        public string[] labels { get; set; }
        public List<DatasetsBar> datasets { get; set; }
    }

    public class DatasetsBar
    {
        public string label { get; set; }
        public string backgroundColor { get; set; }
        public int[] data { get; set; }
        public string borderColor { get; set; }
    }


}

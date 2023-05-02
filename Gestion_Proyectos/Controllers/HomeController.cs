using Microsoft.AspNetCore.Mvc;
using IronPdf;
using IronPdf.Rendering;
using BL;
using BE;
using Newtonsoft.Json.Linq;
using sun.swing;
using System.Drawing.Imaging;
using IronPdf.Rendering.Abstractions;
using System.Drawing;

namespace Power_Campus_Tools.Controllers
{
    public class ResolucionController : Controller
    {
        private readonly BL_RESOLUCION _BL_RESOLUCION;

        public ResolucionController()
        {
            _BL_RESOLUCION = new BL_RESOLUCION();
        }
        public IActionResult generarinforme()
        {
            // Obtener el HTML que se va a convertir a PDF
            var lista = _BL_RESOLUCION.SimulacionCambio("2023", "SEM01", "ANI", "P000284996").ToList();

            var cursosConvalidados = lista.Where(p => p.nota != "").ToList();
            var cursosPorConvalidar = lista.Where(p => p.nota == "").ToList();

            var obtenerCiclos = lista.Select(p => p.print_order).Distinct().ToList();

            //Con Notas
            List<(string, int)> rowCiclos = obtenerCiclos
             .Select(x => (x, lista.Count(p => p.print_order == x && p.nota != "")))
             .ToList();
            string generarContentido = "";
            int lastPrintOrder = -1;
            foreach (var x in cursosConvalidados)
            {
                x.ciclo = CiclosEnRomano().Where(t => t.Item1 == x.print_order).FirstOrDefault().Item2;
                if (x.nota != "")
                {
                    if (Convert.ToInt32(x.print_order) != lastPrintOrder)
                    {
                        var tuplaEncontrada = rowCiclos.Where(t => t.Item1 == x.print_order).FirstOrDefault();

                        generarContentido += $@"
<tr>
        <td  rowspan=""{tuplaEncontrada.Item2}"" class=""ciclo"">{x.ciclo}</td>
        <td>{x.desc_curso}</td>
        <td class=""nota"">{x.nota}</td>
        <td class=""ciclo"">{x.ciclo}</td>
        <td>{x.desc_curso_conv}</td>
        <td class=""nota"">{x.nota}</td>
        <td class=""periodo"">{x.ano + "-" + x.semestre.Substring(x.semestre.Length - 2)}</td>
      </tr>
";
                        lastPrintOrder = Convert.ToInt32(x.print_order);

                    }
                    else
                    {
                        generarContentido += $@"
<tr>

        <td>{x.desc_curso}</td>
        <td class=""nota"">{x.nota}</td>
        <td class=""ciclo"">{x.ciclo}</td>
        <td>{x.desc_curso_conv}</td>
        <td class=""nota"">{x.nota}</td>
        <td class=""periodo"">{x.ano + "-" + x.semestre.Substring(x.semestre.Length - 2)}</td>
      </tr>
";
                    }

                }

            }

            //Sin Notas
            List<(string, int)> rowCiclosSin = obtenerCiclos.Select(x => (x, lista.Count(p => p.print_order == x && p.nota == ""))).ToList();
            string generarContentidoSin = "";
            int lastPrintOrderSin = -1;

            foreach (var x in cursosPorConvalidar)
            {
                x.ciclo = CiclosEnRomano().Where(t => t.Item1 == x.print_order).FirstOrDefault().Item2;

                if (x.nota == "")
                {
                    if (Convert.ToInt32(x.print_order) != lastPrintOrderSin)
                    {
                        var tuplaEncontrada = rowCiclosSin.Where(t => t.Item1 == x.print_order).FirstOrDefault();

                        generarContentidoSin += $@"
<tr>
        <td  rowspan=""{tuplaEncontrada.Item2}"" class=""ciclo"">{x.ciclo}</td>
        <td>{x.desc_curso}</td>
      </tr>
";
                        lastPrintOrderSin = Convert.ToInt32(x.print_order);

                    }
                    else
                    {
                        generarContentidoSin += $@"
<tr>

        <td>{x.desc_curso}</td>

      </tr>
";
                    }

                }

            }
            var tabla = $@"
<div style=""text-align: left"">
    <table style=""margin: 0 auto;border-collapse: collapse;"">
        <thead>
            <tr>
                <th class=""ciclo"">Ciclo</th>
                <th class=""planestudio"">Plan de estudios 2023</th>
                <th class=""nota"">Nota</th>
                <th class=""ciclo"">Ciclo</th>
                <th class=""planestudionuev"">Plan de estudios 2021</th>
                <th class=""nota"">Nota</th>
                <th class=""periodo"">Periodo Academico</th>
              </tr>
        </thead>
        <tbody>
            {generarContentido}
        
          </tbody>
    </table>
  </div>
";
            var unidades = $@"

    <table style=""margin: 0 auto;border-collapse: collapse;"">
        <thead>
            <tr>
                <th class=""ciclo"">Ciclo</th>
                <th class=""planestudio"">Unidades Didácticas</th>
                
              </tr>
        </thead>
        <tbody>
            {generarContentidoSin}
        
          </tbody>
    </table>
 
";
            ResolucionView rv = new ResolucionView();
            var html = ObtenerHtml(rv, tabla, unidades);

            // Crear el objeto IronPdf.HtmlToPdf para convertir el HTML a PDF
            var htmlToPdf = new HtmlToPdf();
            // Configuración de IronPDF
            htmlToPdf.PrintOptions.MarginTop = 15;
            htmlToPdf.PrintOptions.MarginBottom = 15;
            htmlToPdf.PrintOptions.MarginLeft = 10;
            htmlToPdf.PrintOptions.MarginRight = 10;
            // Convertir el HTML a PDF
            var pdf = htmlToPdf.RenderHtmlAsPdf(html);


            // Guardar el archivo PDF en disco o devolverlo como resultado de la acción
            // Guardar en disco:
            //pdf.SaveAs(@"D:\Perfil_DiegoGutarra\Documents\Repositorio\PwCToolsVersions\Power_Campus_Tools vIronPdf\Power_Campus_Tools\Power Campus Tools\wwwroot\archivos\ejemplo.pdf");
            // Devolver como resultado de la acción:

            var pdfBytes = pdf.BinaryData;
            return File(pdfBytes, "application/pdf", "ejemplo.pdf");
        }
      
        private List<(string, string)> CiclosEnRomano()
        {
            // Creamos un List de tuplas (string, int)
            List<(string, string)> ciclos = new List<(string, string)>()
            {
                ("1", "I"),
                ("2", "II"),
                ("3", "III"),
                ("4", "IV"),
                ("5", "V"),
                ("6", "VI")
            };
            return ciclos;
        }
        private string FormatDatetimeDDdMMYYYY()
        {
            DateTime fecha = DateTime.Now;
            string fechaFormateada = fecha.ToString("dd 'de' MMMM 'de' yyyy");

            return fechaFormateada;
        }
        private string GetAnio()
        {
            DateTime fecha = DateTime.Now;
            string fechaFormateada = fecha.ToString("yyyy");

            return fechaFormateada;
        }
        private string ObtenerHtml(ResolucionView rv, string tabla, string unidades)
        {
            string html = @"
<!DOCTYPE html>
<html lang=""en"">
  <head>
    <meta charset=""UTF-8"" />
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"" />
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
    <title>Document</title>
  </head>

  <body>
    <div style=""position: relative;"">
        <div style=""position: absolute; top: 220px; left: 0; z-index: -1; transform: rotate(180deg);"">
            <div style=""writing-mode: vertical-rl; text-align: center;font-size: 10px;"">
                Instituto de Educación Superior Tecnológico Privado de Comercio Exterior - R.M 1066-84-ED / R.D. 043-2005-ED
              </div>
        </div>
        <style>
            body {
              font-family: sans-serif;
            }
            .contenedor {
              padding: 0px 25px;
            }
            .encabezadoTitulo {
              text-align: center;
              font-weight: 700;
              margin-block-end: 0.1em;
              margin-block-start: 0.1em;
            }
            .dialogo {
              color: red;
              text-align: center;
              font-style: italic;
              font-weight: bold;
            }
            .fecha {
              font-weight: 600;
              text-align: right;
            }
            .tituloResolucion {
              text-align: center;
              text-decoration: underline;
            }
            .solicitante {
              text-align: justify;
            }
            .registrese{
              text-align: center;
              font-weight: 700;
            }
            .convalidaciones{
            font-weight: 700;
            font-size: 12px;
        }
        .instituto{
            font-weight: 700;
        }
          </style>
          <div class=""contenedor"">
            <h4 class=""encabezadoTitulo"">
              INSTITUTO DE EDUCACION SUPERIOR TECNOLOGICO
            </h4>
            <h4 class=""encabezadoTitulo"">PRIVADO DE COMERCIO EXTERIOR</h4>
            <p class=""dialogo"">""Año del diálogo y la reconciliación nacional""</p>
            <p class=""fecha"">San Borja, " + rv.fecha + @"</p>
            <h2 class=""tituloResolucion"">RESOLUCION DIRECTORAL Nº 023-B-" + rv.anio + @"</h2>

           <p class=""solicitante"">
          Visto la solicitud del(la) alumno(a)
          <strong>" + rv.nombrecompleto + @"</strong>, con código Nº 000-" + rv.codigo + @", con
          referencia a su convalidación y adecuación de los cursos de su
          especialidad de acuerdo al plan de estudios " + rv.anioplanestudio + @", adecuado al Nuevo
          Diseño Curricular Básico Nacional.
        </p>
        <h3>CONSIDERANDO:</h3>
        <p class=""solicitante"">
          Que, el(la) alumno(a) <strong>" + rv.nombrecompleto + @"</strong>, se matriculó al Instituto de
          Educación Superior Tecnológico Privado de Comercio Exterior en el
          Semestre Académico " + rv.periodoplanestudio + @" en la carrera de " + rv.carrera + @", siendo el plan de estudios
          modular.
        </p>
        <p class=""solicitante"">
          Que, en el semestre " + rv.periodoplanestudionuevo + @" el(la) mencionado(a) alumno(a) se matriculó
          con la condicon académica de reingresante al " + rv.ciclo + @" ciclo y de acuerdo al
          Reglamento General del Alumno en su título PLAN DE ESTUDIOS, ítem 1.2
          Del currículo académico, en sus incisos:<strong>
            c) ""Los cambios curriculares pueden implicar incorporar, eliminar
            cursos y/o cambiarlos de semestre. Solamente se aplica a los
            estudiantes: ingresantes, repitentes, traslados o reingreso que
            deben de cursar un semestre en el que entra en vigencia un nuevo
            currículo académico."" d) ""En el caso de estudiantes repitentes o
            reingresantes, el cambio de currículo académico podría implicar un
            retroceso de semestre, ya que resulta obligatorio aprobar todos los
            cursos del nuevo currículo académico desde el primer
            semestre.""</strong
          >
        </p>
        <p class=""solicitante"">
          Que, mediante Resolución Directoral N° 0101-2014-MINEDU/VMGP-DIGESUTP,
          se oficializó para el periodo académico " + rv.periodoplanestudio + @" en adelante, la carrera " + rv.carrera + @",
          debe insertarse al Nuevo Diseño Curricular Básico de Educación
          Superior Tecnológica, la cual irá implementándose gradualmente.
        </p>
        <h3>SE DECRETA:</h3>
        <ol>
          <li>
            Autorizar la convalidación de las Unidades Didácticas y la
            adecuación al Nuevo Diseño Curricular Básico Nacional del(la)
            alumno(a) <strong>" + rv.nombrecompleto + @"</strong> con código N° 000-" + rv.codigo + @", de la carrera de " + rv.carreranuevo + @".
          </li>
          <li>
            Dar cumplimiento con lo emitido en la presente Resolución Directoral
            (se adjunta cuadro de convalidación).
          </li>
        </ol>
        <h3 class=""registrese"">REGÍSTRESE, COMUNÍQUESE Y ARCHÍVESE</h3>
<br>
       <div style=""display: flex; justify-content: center; align-items: center;"">
                <img src=""https://res.cloudinary.com/dfuuywyk9/image/upload/v1682539144/Sello_icnnu0.jpg"" alt=""Sello"" style=""margin-right: 20px;"">
                <img src=""https://res.cloudinary.com/dfuuywyk9/image/upload/v1682539143/Firma_nj9edk.jpg"" alt=""Firma"" style=""margin-left: 20px;"">
              </div>
 <br>
              <br>
            <style>
              td,
              th {
                border: 1px solid black;
                font-size: 10px;
                font-weight: 500;
              }
              th{
                  text-align: center !important;
              }
            
              .planestudio{
                  width: 300px;  
              }
              .planestudionuev{
                  width: 300px;  
              }
              .ciclo{
                  text-align: center !important;
              }
              .nota{
                  text-align: center !important;
              }
              .periodo{
                  text-align: center !important;
              }
              
            </style>
<h5 class=""instituto"">INSTITUTO DE EDUCACION SUPERIOR TECNOLOGICO PRIVADO DE COMERCIO EXTERIOR</h5>
        <p style=""font-size:12px;"" >CONVALIDACIONES POR CAMBIO DE PLAN DE ESTUDIOS</p>
        <p class=""convalidaciones"">ALUMNO: " + rv.nombrecompleto + @"</p>

          " + tabla + @"
            <p class=""convalidaciones"">
          El alumno(a) debe llevar las siguientes Unidades Didácticas en el
          periodo académico " + rv.periodoplanestudionuevo + @":
        </p>
<div style=""display: flex; justify-content: center; align-items: center;"">
            <div style=""flex: 1;"">
            " + unidades + @"
 </div>
<div style=""flex: 1; display: flex; justify-content: center"">
            <img
              src=""https://res.cloudinary.com/dfuuywyk9/image/upload/v1682539144/Sello_icnnu0.jpg""
              alt=""Sello""
              style=""margin-right: 20px; width: 20%; height: 20%;""
            />
            <img
              src=""https://res.cloudinary.com/dfuuywyk9/image/upload/v1682539143/Firma_nj9edk.jpg""
              alt=""Firma""
              style=""margin-left: 20px; width: 30%; height: 30%;""
            />
          </div>
          </div>
      </div>
    
   
  </body>
</html>
";
            return html;
        }
        private string GenerarTablaConvalidacion((string,string)planestudio, string contenido)
        {
            var tabla = $@"
<div style=""text-align: left"">
    <table style=""margin: 0 auto;border-collapse: collapse;"">
        <thead>
            <tr>
                <th class=""ciclo"">Ciclo</th>
                <th class=""planestudio"">{planestudio.Item1}</th>
                <th class=""nota"">Nota</th>
                <th class=""ciclo"">Ciclo</th>
                <th class=""planestudionuev"">{planestudio.Item2}</th>
                <th class=""nota"">Nota</th>
                <th class=""periodo"">Periodo Academico</th>
              </tr>
        </thead>
        <tbody>
            {contenido}
        
          </tbody>
    </table>
  </div>
";
            return tabla;
        }
        private string GenerarTablaUD(string contenido)
        {
            var unidades = $@"

    <table style=""margin: 0 auto;border-collapse: collapse;"">
        <thead>
            <tr>
                <th class=""ciclo"">Ciclo</th>
                <th class=""planestudio"">Unidades Didácticas</th>
                
              </tr>
        </thead>
        <tbody>
            {contenido}
        
          </tbody>
    </table>
 
";
            return unidades;
        }
        public ActionResult ObtenerCurriculums()
        {
            try
            {
                var lista = _BL_RESOLUCION.ListaCurriculum();
                return Json(new { data = lista, mensaje = "Consulta exitosa", success = true });
            }
            catch (Exception ex)
            {
                return Json(new { data = "", mensaje = $"Error en obtener curriculums: {ex.Message}", success = false });

            }
        }
        [HttpPost]
        public IActionResult GenerarResolucion([FromBody] ObtenerDatosView obj)
        {
            try
            {
                var mallaActual = _BL_RESOLUCION.ObtenerDatosMallaActual(obj.codigo);
                obj.codigo = $"P{obj.codigo}";

                // Obtener el HTML que se va a convertir a PDF
                var lista = _BL_RESOLUCION.SimulacionCambio(obj.anio, obj.semestre, obj.curriculum, obj.codigo).ToList();


                var cursosConvalidados = lista.Where(p => p.nota != "").ToList();
                var cursosPorConvalidar = lista.Where(p => p.nota == "").ToList();

                var obtenerCiclos = lista.Select(p => p.print_order).Distinct().ToList();

                //Con Notas
                List<(string, int)> rowCiclos = new List<(string, int)>();
                foreach (var x in obtenerCiclos)
                {
                    int row = lista.Count(p => p.print_order == x && p.nota != "");
                    rowCiclos.Add((x, row));
                }
                string generarContentido = "";
                int lastPrintOrder = -1;
                foreach (var x in cursosConvalidados)
                {
                    x.ciclo = CiclosEnRomano().Where(t => t.Item1 == x.print_order).FirstOrDefault().Item2;
                    if (x.nota != "")
                    {


                        if (Convert.ToInt32(x.print_order) != lastPrintOrder)
                        {
                            var tuplaEncontrada = rowCiclos.Where(t => t.Item1 == x.print_order).FirstOrDefault();

                            generarContentido += $@"
<tr>
        <td  rowspan=""{tuplaEncontrada.Item2}"" class=""ciclo"">{x.ciclo}</td>
        <td>{x.desc_curso}</td>
        <td class=""nota"">{x.nota}</td>
        <td class=""ciclo"">{x.ciclo}</td>
        <td>{x.desc_curso_conv}</td>
        <td class=""nota"">{x.nota}</td>
        <td class=""periodo"">{x.ano + "-" + x.semestre.Substring(x.semestre.Length - 2)}</td>
      </tr>
";
                            lastPrintOrder = Convert.ToInt32(x.print_order);

                        }
                        else
                        {
                            generarContentido += $@"
<tr>

        <td>{x.desc_curso}</td>
        <td class=""nota"">{x.nota}</td>
        <td class=""ciclo"">{x.ciclo}</td>
        <td>{x.desc_curso_conv}</td>
        <td class=""nota"">{x.nota}</td>
        <td class=""periodo"">{x.ano + "-" + x.semestre.Substring(x.semestre.Length - 2)}</td>
      </tr>
";
                        }

                    }

                }

                //Sin Notas
                List<(string, int)> rowCiclosSin = new List<(string, int)>();
                foreach (var x in obtenerCiclos)
                {
                    int row = lista.Count(p => p.print_order == x && p.nota == "");
                    rowCiclosSin.Add((x, row));
                }
                string generarContentidoSin = "";
                int lastPrintOrderSin = -1;
                foreach (var x in cursosPorConvalidar)
                {
                    x.ciclo = CiclosEnRomano().Where(t => t.Item1 == x.print_order).FirstOrDefault().Item2;

                    if (x.nota == "")
                    {
                        if (Convert.ToInt32(x.print_order) != lastPrintOrderSin)
                        {
                            var tuplaEncontrada = rowCiclosSin.Where(t => t.Item1 == x.print_order).FirstOrDefault();

                            generarContentidoSin += $@"
<tr>
        <td  rowspan=""{tuplaEncontrada.Item2}"" class=""ciclo"">{x.ciclo}</td>
        <td>{x.desc_curso}</td>
      </tr>
";
                            lastPrintOrderSin = Convert.ToInt32(x.print_order);

                        }
                        else
                        {
                            generarContentidoSin += $@"
<tr>

        <td>{x.desc_curso}</td>

      </tr>
";
                        }

                    }

                }

         
                ResolucionView rv = new ResolucionView();
                rv.fecha = FormatDatetimeDDdMMYYYY();
                rv.anio = GetAnio();
                rv.codigo = obj.codigo.Substring(obj.codigo.Length - 6);
                rv.nombrecompleto = mallaActual.nombrecompleto;
                rv.anioplanestudio = mallaActual.matric_year;
                rv.periodoplanestudio = $"{mallaActual.matric_year}-{CiclosEnRomano().Where(t => t.Item1 == mallaActual.matric_term.Substring(mallaActual.matric_term.Length - 1)).FirstOrDefault().Item2}";
                rv.carrera = mallaActual.long_desc;
                rv.periodoplanestudionuevo = $"{obj.anio}-{CiclosEnRomano().Where(t => t.Item1 == obj.semestre.Substring(obj.semestre.Length - 1)).FirstOrDefault().Item2}";
                rv.carreranuevo = _BL_RESOLUCION.ListaCurriculum().FirstOrDefault(p => p.code_value_key == obj.curriculum).long_desc;
                rv.anioplanestudionuevo = obj.anio;
                var html = ObtenerHtml(rv, GenerarTablaConvalidacion(("Plan de estudios 2019",mallaActual.planestudio), generarContentido), GenerarTablaUD(generarContentidoSin));

                // Crear el objeto IronPdf.HtmlToPdf para convertir el HTML a PDF
                var htmlToPdf = new HtmlToPdf();
                // Configuración de IronPDF
                htmlToPdf.PrintOptions.MarginTop = 15;
                htmlToPdf.PrintOptions.MarginBottom = 15;
                htmlToPdf.PrintOptions.MarginLeft = 10;
                htmlToPdf.PrintOptions.MarginRight = 10;
                // Convertir el HTML a PDF
                var pdf = htmlToPdf.RenderHtmlAsPdf(html);

                CrearCarpeta(obj.codigo);
                if (System.IO.File.Exists($@"D:\Perfil_DiegoGutarra\Documents\Repositorio\PwCToolsVersions\Power_Campus_Tools vIronPdf\Power_Campus_Tools\Power Campus Tools\wwwroot\archivos\{obj.codigo}\{obj.codigo}_RD_023_B_{obj.anio}.pdf"))
                {
                    System.IO.File.Delete($@"D:\Perfil_DiegoGutarra\Documents\Repositorio\PwCToolsVersions\Power_Campus_Tools vIronPdf\Power_Campus_Tools\Power Campus Tools\wwwroot\archivos\{obj.codigo}\{obj.codigo}_RD_023_B_{obj.anio}.pdf");
                }
                // Guardar en disco:
                pdf.SaveAs($@"D:\Perfil_DiegoGutarra\Documents\Repositorio\PwCToolsVersions\Power_Campus_Tools vIronPdf\Power_Campus_Tools\Power Campus Tools\wwwroot\archivos\{obj.codigo}\{obj.codigo}_RD_023_B_{obj.anio}.pdf");


                //var pdfBytes = pdf.BinaryData;
                //return File(pdfBytes, "application/pdf", "ejemplo.pdf");
                return Json(new
                {
                    success = true,
                    mensaje = $"Resolución Directoral Generado Correctamente {obj.codigo}_RD_023_B_{obj.anio}.pdf"
                });

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    mensaje = ex.Message
                });

            }
        }

    

        private void CrearCarpeta(string codigo)
        {
            string rootFolderPath = @"D:\Perfil_DiegoGutarra\Documents\Repositorio\PwCToolsVersions\Power_Campus_Tools vIronPdf\Power_Campus_Tools\Power Campus Tools\wwwroot\archivos\";
            string folderName = codigo;

            string folderPath = Path.Combine(rootFolderPath, folderName);

            // Validar si la carpeta existe, sino crearla
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }


        public IActionResult Index()
        {

            return View();
        }
    }
}

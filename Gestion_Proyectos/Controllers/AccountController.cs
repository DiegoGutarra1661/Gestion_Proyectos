using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Gestion_Proyectos_BL;
using Newtonsoft.Json;

namespace Gestion_Proyectos.Controllers
{
    public class AccountController : Controller
    {
        private readonly Usuario_BL _usuarioBL;
        public AccountController()
        {
            _usuarioBL = new Usuario_BL();
        }

        public class CapthaResponseViewModel
        {
            public bool Success { get; set; }
            [JsonProperty(PropertyName = "error-codes")]
            public IEnumerable<string> ErrorCodes { get; set; }
            [JsonProperty(PropertyName = "challenge_ts")]
            public DateTime ChallengeTime { get; set; }
            public string HostName { get; set; }
            public double Score { get; set; }
            public string Action { get; set; }
        }

        // GET: Account
        public ActionResult Login(string rutaOrigen = "")
        {
            ViewBag.rutaOrigen = rutaOrigen;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string correo, string clave, string rutaOrigen = "", string token = "")
        {
            var isCaptchaValid = await IsCaptchaValid(token);
            if (isCaptchaValid)
            {
                if (_usuarioBL.EstaEnAD(correo, clave))
                {


                    var usuario = _usuarioBL.Login(correo);
                    if (usuario != null)
                    {
                        _usuarioBL.IniciarSesion(usuario);
                        if (!string.IsNullOrEmpty(rutaOrigen))
                        {
                            return Redirect(rutaOrigen);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }

                    }
                    else
                    {
                        ViewBag.mensaje = "Sin acceso al sistema.";
                        ViewBag.rutaOrigen = rutaOrigen;
                        return View();
                    }

                }
                else
                {
                    ViewBag.mensaje = "Credenciales incorrectas.";
                    ViewBag.rutaOrigen = rutaOrigen;
                    return View();
                }
            }
            else
            {
                ViewBag.mensaje = "Falló en el captcha";
                ViewBag.rutaOrigen = rutaOrigen;
                return View();
            }

        }

        private async Task<bool> IsCaptchaValid(string token)
        {
            try
            {
                var secret = "6LdxtNEcAAAAAGwvG3fuJj6tLYo5xrdRmHYgmtTl";
                using (var client = new HttpClient())
                {
                    var values = new Dictionary<string, string>
                    {
                        { "secret", secret},
                        { "response", token},
                        { "remoteip", Request.UserHostAddress}
                    };

                    var content = new FormUrlEncodedContent(values);
                    var verify = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
                    var captchaResponseJson = await verify.Content.ReadAsStringAsync();
                    var capthaResult = JsonConvert.DeserializeObject<CapthaResponseViewModel>(captchaResponseJson);
                    return capthaResult.Success
                        && capthaResult.Action == "submit"
                        && capthaResult.Score > 0.5;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public ActionResult Logout()
        {
            _usuarioBL.CerrarSesion();
            return RedirectToAction("Login", "Account");
        }
    }
}
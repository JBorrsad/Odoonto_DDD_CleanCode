using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace WF.Mimetic.UI.Server.Controllers.Logs
{
    [Route("api/v0/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        [HttpGet]
        public ObjectResult GetApiLogs(DateTime date)
        {
            string filePath = Path.Combine("Logs", $"log-{date.ToString("yyyyMMdd")}.txt");
            return LoadLogs(filePath);
        }

        [HttpGet("/Outside")]
        public ObjectResult GetOutsideLogs(DateTime date)
        {
            string filePath = Path.Combine("../WF.Mimetic.Outside.Api/Logs", $"log-{date.ToString("yyyyMMdd")}.txt");
            return LoadLogs(filePath);
        }

        [HttpGet("/Inside")]
        public ObjectResult GetInsideLogs(DateTime date)
        {
            string filePath = Path.Combine("../WF.Mimetic.Inside.Api/Logs", $"log-{date.ToString("yyyyMMdd")}.txt");
            return LoadLogs(filePath);
        }

        private ObjectResult LoadLogs(string filePath)
        {
            try
            {

                if (!System.IO.File.Exists(filePath))
                {
                    throw new Exception("Log not found.");
                }

                using (FileStream sourceStream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader reader = new StreamReader(sourceStream))
                {
                    string content = reader.ReadToEnd();
                    return Ok(content);
                }
            }
            catch (IOException ex)
            {
                // Maneja excepciones de E/S, como el archivo está siendo utilizado por otro proceso
                return StatusCode(500, $"Error al abrir el archivo: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Maneja otras excepciones
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.RegularExpressions;
using Telegram.Bot;
using System.IO;

namespace Agems.Controllers
{
    public class TelegramController : Controller
    {

        private readonly IConfiguration _config;

        public TelegramController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> SendPic(string dataUrl)
        {
            string savePath = "wwwroot/images/image.png";
            string base64Data = dataUrl.Substring(dataUrl.LastIndexOf(',') + 1);
            byte[] data = Convert.FromBase64String(base64Data);
            await System.IO.File.WriteAllBytesAsync(savePath, data);

            string token = _config["TelegramToken"];
            var bot = new TelegramBotClient(token);

            var imageFile = System.IO.File.Open(savePath, FileMode.Open);
            await bot.SendPhotoAsync("@agems_channel", photo: imageFile);

            imageFile.Close();

            return RedirectToPage("/SendPic");
        }

    }
}

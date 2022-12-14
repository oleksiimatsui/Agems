using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.RegularExpressions;
using Telegram.Bot;
using System.IO;

namespace Agems.Controllers
{
    public class TelegramController : Controller
    {

        private string token;

        public TelegramController(string _token)
        {
            token = _token;
        }

        [HttpPost]
        public async Task<IActionResult> SendPic(string dataUrl)
        {
            string savePath = "wwwroot/images/image.png";
            string base64Data = dataUrl.Substring(dataUrl.LastIndexOf(',') + 1);
            byte[] data = Convert.FromBase64String(base64Data);
            await System.IO.File.WriteAllBytesAsync(savePath, data);

            var bot = new TelegramBotClient(token);

            var imageFile = System.IO.File.Open(savePath, FileMode.Open);
            await bot.SendPhotoAsync("@agems_channel", photo: imageFile);

            imageFile.Close();

            return RedirectToPage("/SendPic");
        }

    }
}

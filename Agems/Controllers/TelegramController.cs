using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.RegularExpressions;
using Telegram.Bot;
using System.IO;

namespace Agems.Controllers
{
    public class TelegramController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> SendPic(string dataUrl)
        {
            string savePath = "wwwroot/images/image.png";
            string base64Data = dataUrl.Substring(dataUrl.LastIndexOf(',') + 1);
            byte[] data = Convert.FromBase64String(base64Data);
            await System.IO.File.WriteAllBytesAsync(savePath, data);

            var bot = new TelegramBotClient("5733346512:AAE9hlWfiTkIvjPHAtBY4qMpyaeV2hWCaUY");

            var imageFile = System.IO.File.Open(savePath, FileMode.Open);
            await bot.SendPhotoAsync("@agems_channel", photo: imageFile);

            imageFile.Close();

            return RedirectToPage("/SendPic");
        }

    }
}

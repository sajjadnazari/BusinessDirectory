using BusinessDirectory.Application.Features.Businesses.Commands.CreateBusiness;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BusinessDirectory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // تزریق ISender در همان خط تعریف کلاس (C# 14)
    public class BusinessesController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateBusiness([FromBody] CreateBusinessCommand command, CancellationToken cancellationToken)
        {
            // ما فقط Command را به MediatR می‌دهیم. او خودش هندلر را پیدا و اجرا می‌کند.
            var businessId = await sender.Send(command, cancellationToken);

            // برگرداندن کد 201 Created به همراه آیدی کسب‌وکار جدید
            return Created("", new { Id = businessId, Message = "کسب‌وکار با موفقیت ثبت شد." });
        }
    }
}

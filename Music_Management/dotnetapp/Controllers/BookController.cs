using Microsoft.AspNetCore.Mvc;
using dotnetapp.Models;
using dotnetapp.Services;

namespace dotnetapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MobilePhoneController : ControllerBase
    {
        private readonly IMobilePhoneService _mobilePhoneService;

        public MobilePhoneController(IMobilePhoneService mobilePhoneService)
        {
            _mobilePhoneService = mobilePhoneService;
        }

        [HttpGet]
        public IActionResult GetAllMobilePhones()
        {
            var mobilePhones = _mobilePhoneService.GetAllMobilePhones();
            return Ok(mobilePhones);
        }

        [HttpGet("{id}")]
        public IActionResult GetMobilePhoneById(int id)
        {
            var mobilePhone = _mobilePhoneService.GetMobilePhoneById(id);
            if (mobilePhone == null)
            {
                return NotFound();
            }
            return Ok(mobilePhone);
        }

        [HttpPost]
        public IActionResult AddMobilePhone(MobilePhone mobilePhone)
        {
            _mobilePhoneService.AddMobilePhone(mobilePhone);
            return CreatedAtAction(nameof(GetMobilePhoneById), new { id = mobilePhone.MobilePhoneId }, mobilePhone);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMobilePhone(int id, MobilePhone mobilePhone)
        {
            if (id != mobilePhone.MobilePhoneId)
            {
                return BadRequest();
            }
            _mobilePhoneService.UpdateMobilePhone(mobilePhone);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMobilePhone(int id)
        {
            _mobilePhoneService.DeleteMobilePhone(id);
            return NoContent();
        }
    }
}

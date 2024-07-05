using Microsoft.AspNetCore.Mvc;
using dotnetapp.Models;
using dotnetapp.Services;

namespace dotnetapp.Controllers
{
    [Route("api/[controller]")]
[ApiController]
public class MusicRecordController : ControllerBase
{
    private readonly IMusicRecordService _service;

    public MusicRecordController(IMusicRecordService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAllMusicRecords()
    {
        var musicRecords = _service.GetMusicRecords();
        return Ok(musicRecords);
    }

    [HttpGet("{id}")]
    public IActionResult GetMusicRecordById(int id)
    {
        var musicRecord = _service.GetMusicRecord(id);
        if (musicRecord == null)
            return NotFound();
        
        return Ok(musicRecord);
    }

    [HttpPost]
    public IActionResult AddMusicRecord([FromBody] MusicRecord musicRecord)
    {
        if (musicRecord == null)
            return BadRequest();

        var createdRecord = _service.SaveMusicRecord(musicRecord);
        return CreatedAtAction(nameof(GetMusicRecordById), new { id = createdRecord.MusicRecordId }, createdRecord);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateMusicRecord(int id, [FromBody] MusicRecord musicRecord)
    {
        if (musicRecord == null)
            return BadRequest();

        var updatedRecord = _service.UpdateMusicRecord(id, musicRecord);
        if (updatedRecord == null)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteMusicRecord(int id)
    {
        var isDeleted = _service.DeleteMusicRecord(id);
        if (!isDeleted)
            return NotFound();

        return NoContent();
    }
}

}

using System.Collections.Generic;
using System.Linq;
using dotnetapp.Models;
using dotnetapp.Repository;

namespace dotnetapp.Services
{
    public class MusicRecordService : IMusicRecordService
{
    private readonly MusicRecordRepository _repository;

    public MusicRecordService(MusicRecordRepository repository)
    {
        _repository = repository;
    }

    public List<MusicRecord> GetMusicRecords() => _repository.GetAll();

    public MusicRecord GetMusicRecord(int id) => _repository.GetById(id);

    public MusicRecord SaveMusicRecord(MusicRecord musicRecord) => _repository.Add(musicRecord);

    public MusicRecord UpdateMusicRecord(int id, MusicRecord musicRecord) => _repository.Update(id, musicRecord);

    public bool DeleteMusicRecord(int id) => _repository.Delete(id);
}

}

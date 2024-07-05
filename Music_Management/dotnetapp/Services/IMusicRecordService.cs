using System.Collections.Generic;
using dotnetapp.Models;

namespace dotnetapp.Services
{
    public interface IMusicRecordService
    {
        List<MusicRecord> GetMusicRecords();
        MusicRecord GetMusicRecord(int id);
        MusicRecord SaveMusicRecord(MusicRecord musicRecord);
        MusicRecord UpdateMusicRecord(int id, MusicRecord musicRecord);
        bool DeleteMusicRecord(int id);
    }
}

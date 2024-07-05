using System.Collections.Generic;
using System.Linq;
using dotnetapp.Models;

namespace dotnetapp.Repository
{
    public class MusicRecordRepository : IMusicRecordRepository
    {
        private readonly List<MusicRecord> _musicRecords = new List<MusicRecord>
        {
            new MusicRecord { MusicRecordId = 1, Artist = "Artist1", Album = "Album1", Genre = "Genre1", Price = 10.99m, StockQuantity = 5 },
            new MusicRecord { MusicRecordId = 2, Artist = "Artist2", Album = "Album2", Genre = "Genre2", Price = 15.99m, StockQuantity = 3 }
        };

        public List<MusicRecord> GetAll() => _musicRecords;

        public MusicRecord GetById(int id) => _musicRecords.FirstOrDefault(m => m.MusicRecordId == id);

        public MusicRecord Add(MusicRecord musicRecord)
        {
            musicRecord.MusicRecordId = _musicRecords.Count > 0 ? _musicRecords.Max(m => m.MusicRecordId) + 1 : 1;
            _musicRecords.Add(musicRecord);
            return musicRecord;
        }

        public MusicRecord Update(int id, MusicRecord musicRecord)
        {
            var existingRecord = GetById(id);
            if (existingRecord == null) return null;

            existingRecord.Artist = musicRecord.Artist;
            existingRecord.Album = musicRecord.Album;
            existingRecord.Genre = musicRecord.Genre;
            existingRecord.Price = musicRecord.Price;
            existingRecord.StockQuantity = musicRecord.StockQuantity;

            return existingRecord;
        }

        public bool Delete(int id)
        {
            var musicRecord = GetById(id);
            if (musicRecord == null) return false;

            _musicRecords.Remove(musicRecord);
            return true;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using dotnetapp.Models;

namespace dotnetapp.Repository
{
    public class MusicRecordRepository
    {
        private static List<MusicRecord> _musicRecords = new List<MusicRecord>()
        {
            new MusicRecord 
            { 
                MusicRecordId = 1, 
                Artist = "The Beatles", 
                Album = "Abbey Road", 
                Genre = "Rock", 
                Price = 19.99m, 
                StockQuantity = 50 
            },
            new MusicRecord 
            { 
                MusicRecordId = 2, 
                Artist = "Pink Floyd", 
                Album = "The Dark Side of the Moon", 
                Genre = "Progressive Rock", 
                Price = 24.99m, 
                StockQuantity = 35 
            },
            new MusicRecord 
            { 
                MusicRecordId = 3, 
                Artist = "Michael Jackson", 
                Album = "Thriller", 
                Genre = "Pop", 
                Price = 21.99m, 
                StockQuantity = 45 
            }
            // Add more records as needed
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

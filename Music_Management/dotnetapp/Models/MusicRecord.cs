using System;

namespace dotnetapp.Models
{
    public class MusicRecord
    {
        public int MusicRecordId { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
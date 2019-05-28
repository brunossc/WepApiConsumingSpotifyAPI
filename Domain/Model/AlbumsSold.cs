using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    [Serializable]
    public class AlbumsSold
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int AlbumsSoldId { get; set; }
        public int? AlbumsId { get; set; }
        public Albums Album { get; set; }
        public int? SalesId { get; set; }
        public Sales Sales { get; set; }
        public decimal CashBack { get; set; }
        public decimal Price { get; set; }
    }
}
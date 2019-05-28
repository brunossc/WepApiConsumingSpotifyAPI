using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    [Serializable]
    public class Sales
    {

        public Sales()
        {
            this.Albums = new Collection<AlbumsSold>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int SalesId { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<AlbumsSold> Albums { get; private set; }

        public void AddAlbums(AlbumsSold album)
        {
            this.Albums.Add(album);
        }


    }
}
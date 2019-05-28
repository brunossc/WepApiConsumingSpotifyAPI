using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    [Serializable]
    public class CashBack
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CashBackId { get; set; }
        public int Weekday { get; set; }
        public string Genre { get; set; }
        public decimal value { get; set; }
    }
}
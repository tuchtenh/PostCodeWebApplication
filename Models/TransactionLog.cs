using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PostCodeWebApplication.Models
{
    [Table("TransactionLog")]
    public class TransactionLog
    {
        [MaxLength(6)]
        public string Command { get; set; }
        [MaxLength(100)]
        public string OldName { get; set; }
        [MaxLength(100)]
        public string OldAddress { get; set; }
        [MaxLength(7)]
        public string OldPostCode { get; set; }
        [MaxLength(100)]
        public string NewName { get; set; }
        [MaxLength(100)]
        public string NewAddress { get; set; }
        [MaxLength(7)]
        public string NewPostCode { get; set; }
        public DateTime Modified { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Day14MiniProj.Models
{
    public class Bank
    {
        [Key]
        public int AccNo { get; set; }
        public float Amount { get; set; }
        public DateOnly Created_Date { get; set; }
        public int CusId { get; set; }

        [ForeignKey("CusId")]
        public Customer customer { get; set; }
    }
}

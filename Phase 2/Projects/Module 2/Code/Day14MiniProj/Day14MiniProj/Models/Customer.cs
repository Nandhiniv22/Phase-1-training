using System.ComponentModel.DataAnnotations;

namespace Day14MiniProj.Models
{
    public class Customer
    {
        [Key]
        public int CusId { get; set; }
        public string CusName { get; set; }
        public int Age { get; set; }
    }
}

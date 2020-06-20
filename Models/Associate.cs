using System.ComponentModel.DataAnnotations;

namespace wedding_planner.Models
{
    public class Associate 
    {
        [Key]
        public int AssociateId {get;set;}
        [Required]
        public int UserId {get;set;}
        [Required]
        public int WeddingId {get;set;}
        ////
        public User User {get;set;}
        public Wedding Wedding {get;set;}
    }
}
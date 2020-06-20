using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace wedding_planner.Models
{
    public class Wedding 
    {
        [Key]
        public int WeddingId {get;set;}
        [Required]
        public string WedderOne {get;set;}
        [Required]
        public string WedderTwo {get;set;}
        [Required][Future][DataType(DataType.Date)]
        public DateTime Date {get;set;}
        [Required]
        public int UserId {get;set;}
        public DateTime CreatAt {get;set;} = DateTime.Now;
        public DateTime UpdateAt {get;set;} = DateTime.Now;
        ////////
        public User Creator {get;set;}
        public List<Associate> AllGuests {get;set;}
    }
}
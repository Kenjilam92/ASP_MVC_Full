using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wedding_planner.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}
        [Required]
        public string FirstName {get;set;}
        [Required]
        public string LastName {get;set;}
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email {get;set;}
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8,ErrorMessage="Password need to be at least 8 charcters")]
        public string Password {get;set;}
        public DateTime CreatAt {get;set;} = DateTime.Now;
        public DateTime UpdateAt {get;set;} = DateTime.Now;

        //////////
        public List<Associate> AllWeddings {get;set;}
        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string CFPassword {get;set;}
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wedding_planner.Models
{
    public class LoginUser
    {   
        [Required]
        [DataType(DataType.EmailAddress)]
        public string LoginEmail {get;set;}
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8,ErrorMessage="Password need to be at least 8 charcters")]
        public string LoginPassword {get;set;}    
    }
}
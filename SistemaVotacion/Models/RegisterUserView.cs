using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaVotacion.Models
{
    public class RegisterUserView:UserView
    {
        [Required]
        [StringLength(maximumLength:30,MinimumLength =8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: 30, MinimumLength = 8)]
        [Compare("Password",ErrorMessage ="password and confirmPassword does'n same.")]
        public string ConfirmPassword { get; set; }
    }
}
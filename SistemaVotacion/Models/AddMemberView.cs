using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaVotacion.Models
{
    public class AddMemberView
    {
        public int GroupID { get; set; }

        [Required(ErrorMessage ="you must select a user")]
        public int UserID { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaVotacion.Models
{
    public class AddGroupView
    {
        public int VotingID { get; set; }

        [Required(ErrorMessage ="you must select a group")]
        public int GroupID { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaVotacion.Models
{
    public class AddCandidateView
    {
        public int VotingID { get; set; }

        [Required(ErrorMessage ="You must select a user")]
        public int UserID { get; set; }
    }
}
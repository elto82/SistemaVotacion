using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaVotacion.Models
{
    public class Candidate
    {
        [Key]
        public int CandidateID { get; set; }
        public int VotingID { get; set; }
        public int UserID { get; set; }
        public int QuantityVotes { get; set; }
        public virtual Voting Voting { get; set; }
        public virtual User User { get; set; }

    }
}
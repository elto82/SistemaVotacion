using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaVotacion.Models
{
    public class VotingGroup
    {
        [Key]
        public int VotinGroupID { get; set; }

        public int VotingID { get; set; }

        public int GroupID { get; set; }

        public virtual Voting Voting { get; set; }

        public virtual Group Group { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaVotacion.Models
{
    public class Group
    {
        [Key]
        public int GroupID { get; set; }
        [Required]
        [StringLength(maximumLength:30,MinimumLength =4)]
        public string  Description { get; set; }

        public virtual ICollection <GroupMember> GroupMembers { get; set; }

        public virtual ICollection<VotingGroup> VotingGroups { get; set; }

    }
}
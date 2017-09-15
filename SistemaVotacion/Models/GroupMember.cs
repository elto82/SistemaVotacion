using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaVotacion.Models
{
    public class GroupMember
    {
        [Key]
        public int GroupMemberID { get; set; }

        public int GroupID { get; set; }

        public int UserID { get; set; }

        public virtual Group Group { get; set; }

        public virtual User User  { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaVotacion.Models
{
    public class GroupDetailsView
    {
        public int GroupID { get; set; }
      
        public string Description { get; set; }

        public List <GroupMember> Members { get; set; }
    }
}
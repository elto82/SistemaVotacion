using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaVotacion.Models
{
    public class State
    {
        [Key]
        public int StateID { get; set; }
        [Required]
        [StringLength(maximumLength:30,MinimumLength =2)]
        [Display(Name = "State Description")]
        public string Description { get; set; }

        public virtual ICollection<Voting> Votings { get; set; }
    }
}

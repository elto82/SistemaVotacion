using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaVotacion.Models
{
    public class DetailsVotingView
    {
        public int VotingID { get; set; }

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 2)]
        [Display(Name = "Vouting Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "State")]
        public int StateID { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Required]
        [Display(Name = "Date Time Start")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyy-MM-dd HH:MM tt}", ApplyFormatInEditMode = true)]
        public DateTime DateTimeStart { get; set; }

        [Required]
        [Display(Name = "Date Time End")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyy-MM-dd HH:MM tt}", ApplyFormatInEditMode = true)]
        public DateTime DateTimeEnd { get; set; }

        [Required]
        [Display(Name = "Is For All Users?")]
        public bool IsForAllUsers { get; set; }

        [Required]
        [Display(Name = "Is Enable Blank Vote")]
        public bool IsEnableBlankVote { get; set; }

        [Display(Name = "Quantity Votes")]
        public int QuantityVotes { get; set; }

        [Display(Name = "Quantity Blank Votes")]
        public int QuantityBlankVotes { get; set; }

        [Display(Name = "Winner")]
        public int CandidateWindID { get; set; }

        public  State State { get; set; }

        public  List<VotingGroup> VotingGroups { get; set; }

        public  List<Candidate> Candidates { get; set; }

    }
}
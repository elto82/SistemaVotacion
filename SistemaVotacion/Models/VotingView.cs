using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaVotacion.Models
{
    public class VotingView
    {
        [Key]
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
        [Display(Name = "Date Start")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateStart { get; set; }

        [Required]
        [Display(Name = "Date End")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyy-MM-dd }", ApplyFormatInEditMode = true)]
        public DateTime DateEnd { get; set; }

        [Required]
        [Display(Name = "Time Start")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:MM tt }", ApplyFormatInEditMode = true)]
        public DateTime TimeStart { get; set; }

        [Required]
        [Display(Name = "Time End")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:MM tt}", ApplyFormatInEditMode = true)]
        public DateTime TimeEnd { get; set; }

        [Required]
        [Display(Name = "Is For All Users?")]
        public bool IsForAllUsers { get; set; }

        [Required]
        [Display(Name = "Is Enable Blank Vote")]
        public bool IsEnableBlankVote { get; set; }


    }
}
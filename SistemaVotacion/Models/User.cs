using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemaVotacion.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Index("UserNameIndex", IsUnique = true)]
        [DataType(DataType.EmailAddress)]
        [StringLength(maximumLength:100 ,MinimumLength =7)]
        [Display(Name ="E-mail")]
        [Required]
        public string UserName { get; set; }

        [StringLength(maximumLength: 50, MinimumLength = 2)]
        [Display(Name = "First name")]
        [Required]
        public string FirstName { get; set; }

        [StringLength(maximumLength: 50, MinimumLength = 2)]
        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name ="User")]
        public string  FullName { get {return string.Format ($"{FirstName}{LastName}");}}

        [StringLength(maximumLength: 20, MinimumLength = 7)]
        [Required]
        public string Phone { get; set; }

        [StringLength(maximumLength: 100, MinimumLength = 10)]
        [Required]
        public string Address { get; set; }

        public string Grade { get; set; }

        public string Group { get; set; }

        [DataType(DataType.ImageUrl)]
        [StringLength(maximumLength: 200, MinimumLength = 5)]
        public string Photo { get; set; }

        public virtual ICollection<GroupMember> GroupMembers { get; set; }

        public virtual ICollection<Candidate> Candidates { get; set; }

    }
}
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class UserProfile
    {
       public int Id { get; set; }
       public string UserId { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Display(Name = "Profile Name")]
        public string ProfileName { get; set; }
        public string Password{ get; set; }

        //public string Email { get; set; }
        
        //public string PhoneNumber { get; set; }      

        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
        [Display(Name = "Phone Number")]

        [Required(ErrorMessage = "Please enter your phone number.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Please enter a valid 11-digit phone number.")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        public string Operation { get; set; }
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }
        [Display(Name = "Sage User Name")]
        public string SageUserName { get; set; }
		[Display(Name = "PF No.")]
		public int PFNo { get; set; }
        public string Designation { get; set; }
        public bool IsArchive { get; set; }


        [Display(Name = "Branch Name")]
        public string BranchID { get; set; }
        public string BranchName { get; set; }


        public List<UserProfileAttachments> AttachmentsList { get; set; }
        public IList<IFormFile>? Attachments { get; set; }
        public Audit Audit;
        public UserProfile()
        {
            Audit = new Audit();
            AttachmentsList = new List<UserProfileAttachments>();
        }


    }
}

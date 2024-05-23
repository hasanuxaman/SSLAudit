using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class Audit
    {
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedFrom { get; set; }
        public string? LastUpdateBy { get; set; }
        public DateTime? LastUpdateOn { get; set; }
        public string? LastUpdateFrom { get; set; }
        public DateTime? LastUpdateAt { get; set; }
        public string? PostedBy { get; set; }
        public DateTime? PostedOn { get; set; }
        public string? PostedFrom { get; set; }
        public string? SageUser { get; set; }
        public string? PushedBy { get; set; }
        public DateTime? PushedOn { get; set; }
        public string? PushedFrom { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }

    }
    public class Issue
    {
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedFrom { get; set; }
        public string? LastUpdateBy { get; set; }
        public DateTime? LastUpdateOn { get; set; }
        public string? LastUpdateFrom { get; set; }
        public DateTime? LastUpdateAt { get; set; }
        public string? PostedBy { get; set; }
        public DateTime? PostedOn { get; set; }
        public string? PostedFrom { get; set; }
        public string? SageUser { get; set; }
        public string? PushedBy { get; set; }
        public DateTime? PushedOn { get; set; }
        public string? PushedFrom { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }

    }

    public class Approval
    {
        public bool IsApprovedL1 { get; set; }
        public string ApprovedByL1 { get; set; }
        public DateTime ApprovedDateL1 { get; set; }
        public string CommentsL1 { get; set; }




        public bool IsRejected { get; set; }
        public string RejectedBy { get; set; }
        public DateTime RejectedDate { get; set; }
        public string RejectedComments { get; set; }


    }

}

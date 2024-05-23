using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class UserProfileAttachments
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FileName { get; set; }
        public string ProfileName { get; set; }
        public string Designation { get; set; }

        private string _displayName;
        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(_displayName)) return _displayName;

                if (FileName is not null)
                    return Path.GetFileNameWithoutExtension(this.FileName).Split("_shp_")[0] + Path.GetExtension(this.FileName);

                return "";
            }

            set
            {
                this._displayName = value;
            }
        }


    }
}

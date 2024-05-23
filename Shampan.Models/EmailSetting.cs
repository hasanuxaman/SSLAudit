using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shampan.Models
{
  

    public class EmailSettings
    {
        public string MailToAddress { get; set; }
        public string MailFromAddress = "roonny.khan@gmail.com";
        public bool USsel = true;
       
        public string UserName = "roonny.khan@gmail.com";
		public string Password = "Pa$$word@Gmail1";
		public string ServerName = "smtp.gmail.com";
        public string MailBody { get; set; }
        public string MailHeader { get; set; }
      
        public int Port = 587;


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Diagnostics;

namespace Shampan.Services
{


        public static class MailService
        {

        #region PendingAuditApproval
        public static bool PendingAuditApprovalEamil(string to, string Approvurl, string AuditName)
        {
            string subject = "PendingAuditApproval"; string mailBody; string url = Approvurl; string userFullName = "Sir"; Attachment pdfLink;
            var sent = false;
            subject = "PendingAuditApproval";

            var bodyString = "<tr>" +

                            "</tr>" +
                          "<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +


                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> Hello! </span></font></div>" +
                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> We hope this email finds you well. </span></font></div>" +
                                            "<div><font face = 'Calibri' size = '2'><span style = 'font-size:10pt'>We are seeking for your approval for the upcoming audit <b>" + AuditName + "</b> </span></font></div>" +
                                             "<div> &nbsp;</div> " +

                                       "</div> " +
                                   "</td> " +
                            "</tr> " +
                            "<tr>" +
                                "<td  style='height:50px;'> " +

                                         "<div style = 'margin-left:10px;color:black;'> " +

                                              "<a href='" + url + "' style = 'text-decoration: none;margin:10px 0px 30px 0px;border-radius:4px;padding:3px 5px;border: 0;color:#fff;background-color:#005daa;'>Click here </a>" +

                                          "</div> " +


                                "</td>" +

                                "<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                            "<div style='margin:17px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Thank you for your attention.</span></font></div>" +
                                            //"<div> &nbsp;</div> " +

                                       "</div> " +
                                    "</td> " +
                               "</tr> " +

                               "<tr> " +

                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Regards</span></font></div>" +
                                            //"<div> &nbsp;</div> " +

                                       "</div> " +
                                    "</td> " +

                               "</tr> " +



                                "<tr> " +

                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                         "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>GDIC, IAC</span></font></div>" +
                                          //"<div> &nbsp;</div> " +

                                       "</div> " +
                               "</tr> " +

                            "</tr> ";

            var message = new MailMessage();
            message.To.Add(new MailAddress(to));
            message.Subject = subject;

            sent = PendingAuditApprovalMail(to, bodyString, message);

            return sent;
        }
        public static bool PendingAuditApprovalMail(string To, string body, MailMessage message)
        {

            var content = "<html>" +
                                "<head>" +
                                "</head>" +
                                "<body>" +
                                    "<table border='0' width='100%' style='margin:auto;padding:10px;background-color: #F3F3F3;border:1px solid #0C143B;'>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%'>" +
                                                    "<tr>" +
                                                        "<td style='text-align: center;'>" +
                                                            "<h1>" +
                                                                "<img src='cid:companylogo'  width='350px' height='80px'/> " +
                                                            "</h1>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' cellpadding='0' cellspacing='0' style='text-align:center;width:100%;background-color: #FFFF;'>" +
                                                body
                                                + "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%' style='border-radius: 5px;text-align: center;'>" +


                                                    "<tr>" +
                                                        "<td>" +
                                                            "<div style='margin-top: 20px;color:black;'>" +


                                                            "</div>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                                "</html>";


            AlternateView av1 = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);

            message.IsBodyHtml = true;

            using (MailMessage mail = new MailMessage())
            {

                mail.From = new MailAddress("iac@green-delta.com");
                mail.To.Add(To);
                mail.Subject = "PendingAuditApproval";
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("mail.green-delta.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("iac@green-delta.com", "9ty@u3{24E;%");
                    smtp.EnableSsl = true;

                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        return false;
                    }
                }
                
            }

        }

        #endregion

        public static bool SendAuditApprovalMail(string to, string Approvurl,string AuditName)
        {
            string subject = "PendingAuditApproval"; string mailBody; string url = Approvurl; string userFullName = "Sir"; Attachment pdfLink;
            var sent = false;
            subject = "PendingAuditApproval";

            var bodyString = "<tr>" +
                                 
                            "</tr>" +
                          "<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +


                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> Hello! </span></font></div>" + "<div> &nbsp;</div> " +
											"<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> We hope this email finds you well. </span></font></div>" + "<div> &nbsp;</div> " +
											"<div><font face = 'Calibri' size = '2'><span style = 'font-size:10pt'>The upcoming audit <b>" + AuditName + "</b> has been approved, requesting to initiate audit.  </span></font></div>" + "<div> &nbsp;</div> " +

											 "<div> &nbsp;</div> " +
                                       "</div> " +
                                   "</td> " +
                            "</tr> " +
                            "<tr>" +
                                "<td  style='height:50px;'> " +

                                    "<div style = 'margin-left:10px;color:black;'> " +
                                         "<a href='" + url + "' style = 'text-decoration: none;margin:10px 0px 30px 0px;border-radius:4px;padding:3px 5px;border: 0;color:#fff;background-color:#005daa;'>Click here </a>" +
                                      "</div> " +

                                "</td>" +

                                "<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                            "<div style='margin:17px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Thank you for your attention.</span></font></div>" +
                                            //"<div> &nbsp;</div> " +

                                       "</div> " +
                                    "</td> " +
                               "</tr> " +

                               "<tr> " +

                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Regards</span></font></div>" +
                                            //"<div> &nbsp;</div> " +

                                       "</div> " +
                                    "</td> " +

                               "</tr> " +



							   "<tr> " +
								"<td style = 'text-align:left;'>" +
									"<div style = 'margin-left:10px;color:black;'> " +

										 "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Internal Audit & Compliance</span></font></div>" +
										 "<div> &nbsp;</div> " +

									"</div> " +
							   "</tr> " +


							   "<tr> " +
								"<td style = 'text-align:left;'>" +
									"<div style = 'margin-left:10px;color:black;'> " +

										 "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Green Delta Insurance Company Limited</span></font></div>" +
										 "<div> &nbsp;</div> " +

									"</div> " +
							   "</tr> " +


							"</tr> ";

            var message = new MailMessage();
            message.To.Add(new MailAddress(to));
            message.Subject = subject;

            sent = SendAuditApprovalMailToTeam(to, bodyString, message);

            return sent;
        }

		public static bool SendAuditApprovalMailToTeam(string To, string body, MailMessage message)
		{

			var content = "<html>" +
								"<head>" +
								"</head>" +
								"<body>" +
									"<table border='0' width='100%' style='margin:auto;padding:10px;background-color: #F3F3F3;border:1px solid #0C143B;'>" +
										"<tr>" +
											"<td>" +
												"<table border='0' width='100%'>" +
													"<tr>" +
														"<td style='text-align: center;'>" +
															"<h1>" +
																"<img src='cid:companylogo'  width='350px' height='80px'/> " +
															"</h1>" +
														"</td>" +
													"</tr>" +
												"</table>" +
											"</td>" +
										"</tr>" +
										"<tr>" +
											"<td>" +
												"<table border='0' cellpadding='0' cellspacing='0' style='text-align:center;width:100%;background-color: #FFFF;'>" +
												body
												+ "</table>" +
											"</td>" +
										"</tr>" +
										"<tr>" +
											"<td>" +
												"<table border='0' width='100%' style='border-radius: 5px;text-align: center;'>" +


													"<tr>" +
														"<td>" +
															"<div style='margin-top: 20px;color:black;'>" +


															"</div>" +
														"</td>" +
													"</tr>" +
												"</table>" +
											"</td>" +
										"</tr>" +
									"</table>" +
								"</body>" +
								"</html>";


			AlternateView av1 = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);

			message.IsBodyHtml = true;

			using (MailMessage mail = new MailMessage())
			{

				//mail.From = new MailAddress("iac@green-delta.com");
				mail.From = new MailAddress("iac@green-delta.com", "Internal Audit & Compliance");
				mail.To.Add(To);
				mail.Subject = "Confirmation Of Audit Approval";
				mail.Body = body;
				mail.IsBodyHtml = true;

				using (SmtpClient smtp = new SmtpClient("mail.green-delta.com", 587))
				{
					smtp.Credentials = new NetworkCredential("iac@green-delta.com", "9ty@u3{24E;%");
					smtp.EnableSsl = true;

					try
					{
						smtp.Send(mail);
						return true;
					}
					catch (Exception ex)
					{
						Console.WriteLine($"Failed to send email: {ex.Message}");
						return false;
					}
				}
			}

		}


		public static bool PendingAuditApprovalSend(string to, string Approvurl, string AuditName)
		{
			string subject = "PendingAuditApproval"; string mailBody; string url = Approvurl; string userFullName = "Sir"; Attachment pdfLink;
			var sent = false;
			subject = "PendingAuditApproval";

			var bodyString = "<tr>" +

							"</tr>" +
						  "<tr> " +
								"<td style = 'text-align:left;'>" +
									"<div style = 'margin-left:10px;color:black;'> " +


											"<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> Hello! </span></font></div>" +
											"<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> We hope this email finds you well. </span></font></div>" +
											"<div><font face = 'Calibri' size = '2'><span style = 'font-size:10pt'>We are seeking for your approval for the upcoming audit <b>" + AuditName + "</b> </span></font></div>" +

											 "<div> &nbsp;</div> " +
									   "</div> " +
								   "</td> " +
							"</tr> " +
							"<tr>" +
								"<td  style='height:50px;'> " +

									"<div style = 'margin-left:10px;color:black;'> " +
										 "<a href='" + url + "' style = 'text-decoration: none;margin:10px 0px 30px 0px;border-radius:4px;padding:3px 5px;border: 0;color:#fff;background-color:#005daa;'>Click here </a>" +
									  "</div> " +

								"</td>" +

								"<tr> " +
								"<td style = 'text-align:left;'>" +
									"<div style = 'margin-left:10px;color:black;'> " +

											"<div style='margin:17px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Thank you for your attention.</span></font></div>" +
									   //"<div> &nbsp;</div> " +

									   "</div> " +
									"</td> " +
							   "</tr> " +

							   "<tr> " +

								"<td style = 'text-align:left;'>" +
									"<div style = 'margin-left:10px;color:black;'> " +

											"<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Regards</span></font></div>" +
									   //"<div> &nbsp;</div> " +

									   "</div> " +
									"</td> " +

							   "</tr> " +



							   "<tr> " +
								"<td style = 'text-align:left;'>" +
									"<div style = 'margin-left:10px;color:black;'> " +

										 "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Internal Audit & Compliance</span></font></div>" +
										 "<div> &nbsp;</div> " +

									"</div> " +
							   "</tr> " +


							   "<tr> " +
								"<td style = 'text-align:left;'>" +
									"<div style = 'margin-left:10px;color:black;'> " +

										 "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Green Delta Insurance Company Limited</span></font></div>" +
										 "<div> &nbsp;</div> " +

									"</div> " +
							   "</tr> " +


							"</tr> ";

			var message = new MailMessage();
			message.To.Add(new MailAddress(to));
			message.Subject = subject;

			sent = PendingAuditApprovalSendMail(to, bodyString, message);

			return sent;
		}

		public static bool PendingAuditApprovalSendMail(string To, string body, MailMessage message)
        {

            var content = "<html>" +
                                "<head>" +
                                "</head>" +
                                "<body>" +
                                    "<table border='0' width='100%' style='margin:auto;padding:10px;background-color: #F3F3F3;border:1px solid #0C143B;'>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%'>" +
                                                    "<tr>" +
                                                        "<td style='text-align: center;'>" +
                                                            "<h1>" +
                                                                "<img src='cid:companylogo'  width='350px' height='80px'/> " +
                                                            "</h1>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' cellpadding='0' cellspacing='0' style='text-align:center;width:100%;background-color: #FFFF;'>" +
                                                body
                                                + "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%' style='border-radius: 5px;text-align: center;'>" +


                                                    "<tr>" +
                                                        "<td>" +
                                                            "<div style='margin-top: 20px;color:black;'>" +


                                                            "</div>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                                "</html>";


            AlternateView av1 = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);

            message.IsBodyHtml = true;

            using (MailMessage mail = new MailMessage())
            {

                //mail.From = new MailAddress("iac@green-delta.com");
				mail.From = new MailAddress("iac@green-delta.com", "Internal Audit & Compliance");
				mail.To.Add(To);
                mail.Subject = "Pending Audit Approval";
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("mail.green-delta.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("iac@green-delta.com", "9ty@u3{24E;%");
                    smtp.EnableSsl = true;

                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        return false;
                    }
                }
            }






        }


        #region ForgetPassword
        public static bool SendForgetPasswordMail(string to, string Approvurl)
        {
            string subject = "ForgetPassword"; string mailBody; string url = Approvurl; string userFullName = "Sir"; Attachment pdfLink;
            var sent = false;
            subject = "ForgetPassword";

            var bodyString = "<tr>" +

                            "</tr>" +

                          "<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> Hello! </span></font></div>" +
                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> We hope this email finds you well. </span></font></div>" +                                           
                                               "<div> &nbsp;</div> " +

                                       "</div> " +
                                   "</td> " +
                            "</tr> " +

                            "<tr>" +
                                "<td  style='height:0px;'> " +

                                          "<div style = 'margin-left:10px;color:black;'> " +
                                               "<a href='" + url + "' style = 'text-decoration: none;margin:0px 0px 0px 0px;border-radius:4px;padding:0px 0px;border: 0;color:#fff;background-color:#005daa;'>Click here </a>" +
                                          "</div> " +
                                       //"<a href='" + url + "' style = 'text-decoration: none;margin:0px 0px 0px 0px;border-radius:4px;padding:0px 0px;border: 0;color:#fff;background-color:#005daa;'>Click here </a>" +

                                "</td>" +

                            


                             "<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                            "<div style='margin:17px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Thank you for your attention.</span></font></div>" +
                                            //"<div> &nbsp;</div> " +

                                       "</div> " +
                                    "</td> " +
                               "</tr> " +


                               "<tr> " +
                                 "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +
                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Regards</span></font></div>" +
                                            //"<div> &nbsp;</div> " +
                                       "</div> " +
                                    "</td> " +
                               "</tr> " +


                             "<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                         "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>GDIC, IAC</span></font></div>" +
                                            //"<div> &nbsp;</div> " +

                                       "</div> " +
                              "</tr> " +


                            "</tr> ";

            var message = new MailMessage();
            message.To.Add(new MailAddress(to));
            message.Subject = subject;

            sent = SendForgetPasswordMailSend(to, bodyString, message);

            return sent;
        }
        public static bool SendForgetPasswordMailSend(string To, string body, MailMessage message)
        {

            var content = "<html>" +
                                "<head>" +
                                "</head>" +
                                "<body>" +
                                    "<table border='0' width='100%' style='margin:auto;padding:10px;background-color: #F3F3F3;border:1px solid #0C143B;'>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%'>" +
                                                    "<tr>" +
                                                        "<td style='text-align: center;'>" +
                                                            "<h1>" +
                                                                "<img src='cid:companylogo'  width='350px' height='80px'/> " +
                                                            "</h1>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' cellpadding='0' cellspacing='0' style='text-align:center;width:100%;background-color: #FFFF;'>" +
                                                body
                                                + "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%' style='border-radius: 5px;text-align: center;'>" +
                                                    "<tr>" +
                                                        "<td>" +
                                                            "<div style='margin-top: 20px;color:black;'>" +
                                                            "</div>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                                "</html>";


            AlternateView av1 = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);
            message.IsBodyHtml = true;
            using (MailMessage mail = new MailMessage())
            {

                mail.From = new MailAddress("iac@green-delta.com"); 
                mail.To.Add(To);
                mail.Subject = "Forget Password";
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("mail.green-delta.com", 587)) 
                {
                    smtp.Credentials = new NetworkCredential("iac@green-delta.com", "9ty@u3{24E;%"); 
                    smtp.EnableSsl = true; 

                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        return false;
                    }
                }

                ////mail.From = new MailAddress("auditgdic@gmail.com");
                //mail.From = new MailAddress("iac@green-delta.com");
                //mail.To.Add(To);
                //mail.Subject = "ForgetPasswordEmail";
                //mail.Body = body;
                //mail.IsBodyHtml = true;
                ////using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                //using (SmtpClient smtp = new SmtpClient("smtp.green-delta.com", 465))
                //{
                //    //smtp.Credentials = new NetworkCredential("auditgdic@gmail.com", "hbdf subq yxfl tboj");
                //    smtp.Credentials = new NetworkCredential("iac@green-delta.com", "9ty@u3{24E;%");
                //    smtp.EnableSsl = true;
                //    //AddNew
                //    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //    smtp.UseDefaultCredentials = false;
                //    //smtp.Timeout = 10000; 
                //    smtp.Send(mail);
                //    return true;
                //}

            }

        }

        #endregion

        public static bool SendMail(string To, string body, MailMessage message)
        {
            var content = "<html>" +
                                "<head>" +
                                "</head>" +
                                "<body>" +
                                    "<table border='0' width='100%' style='margin:auto;padding:10px;background-color: #F3F3F3;border:1px solid #0C143B;'>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%'>" +
                                                    "<tr>" +
                                                        "<td style='text-align: center;'>" +
                                                            "<h1>" +
                                                                "<img src='cid:companylogo'  width='350px' height='80px'/> " +
                                                            "</h1>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' cellpadding='0' cellspacing='0' style='text-align:center;width:100%;background-color: #FFFF;'>" +
                                                body
                                                + "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%' style='border-radius: 5px;text-align: center;'>" +


                                                    "<tr>" +
                                                        "<td>" +
                                                            "<div style='margin-top: 20px;color:black;'>" +


                                                            "</div>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                                "</html>";


            AlternateView av1 = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);

            message.IsBodyHtml = true;

            using (MailMessage mail = new MailMessage())
            {

				mail.From = new MailAddress("iac@green-delta.com", "Internal Audit & Compliance");
				//mail.From = new MailAddress("iac@green-delta.com");

                mail.To.Add(To);
                mail.Subject = "BranchFeedbackPending";
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("mail.green-delta.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("iac@green-delta.com", "9ty@u3{24E;%");
                    smtp.EnableSsl = true;

                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        return false;
                    }
                }
            }
        }
        public static bool SendAuditTeamUserToBranchFeedbackMail(string to, string Approvurl,string AuditName)
        {
            string subject = "An Audit"; string mailBody; string url = Approvurl; string userFullName = "Sir"; Attachment pdfLink;          
            var sent = false;
            subject = "An Audit";


            var bodyString = "<tr>" +
                                 
                            "</tr>" +
                          "<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +


                                            "<div style='margin:10px 0 20px 0'><font size = '2' ><span style='font-size:10pt'> Hello! </span></font></div>" +
                                            "<div style='margin:10px 0 20px 0'><font size = '2' ><span style='font-size:10pt'> We hope this email finds you well. </span></font></div>" +
                                            "<div><font face = 'Calibri' size = '2'><span style = 'font-size:10pt'>We are requesting you to provide your updated Feedback/Response on audit observation <b>" + AuditName + "</b> </span></font></div>" +                                           
                                             "<div> &nbsp;</div> " +

                                       "</div> " +
                                   "</td> " +
                            "</tr> " +
                            "<tr>" +
                                "<td  style='height:50px;'> " +

                                         "<div style = 'margin-left:10px;color:black;'> " +
                                              "<a href='" + url + "' style = 'text-decoration: none;margin:10px 0px 30px 0px;border-radius:4px;padding:3px 5px;border: 0;color:#fff;background-color:#005daa;'>Click here </a>" +
                                          "</div> " +

                                "</td>" +

                             "<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                            "<div style='margin:17px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Thank you for your attention.</span></font></div>" +
                                            "<div> &nbsp;</div> " +

                                       "</div> " +
                                    "</td> " +
                               "</tr> " +

                               "<tr> " +

                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Regards</span></font></div>" +
                                            "<div> &nbsp;</div> " +

                                       "</div> " +
                                    "</td> " +
                               "</tr> " +





								"<tr> " +

								"<td style = 'text-align:left;'>" +
									"<div style = 'margin-left:10px;color:black;'> " +

										 "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Internal Audit & Compliance</span></font></div>" +
									     "<div> &nbsp;</div> " +

									"</div> " +
							   "</tr> " +


							   "<tr> " +
								"<td style = 'text-align:left;'>" +
									"<div style = 'margin-left:10px;color:black;'> " +

										 "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Green Delta Insurance Company Limited</span></font></div>" +
									     "<div> &nbsp;</div> " +

									"</div> " +
							   "</tr> " +

							"</tr> ";

            var message = new MailMessage();
            message.To.Add(new MailAddress(to));
            message.Subject = subject;

            sent = SendMail(to, bodyString, message);

            return sent;
        }


        #region ForAuditBranchFeedbackAuditTeamUser
        public static bool SendAuditBranchFeedbackTeamUserMail(string to, string Approvurl,string AuditName)
        {
            string subject = "AuditBranchFeedbackTeamUser"; string mailBody; string url = Approvurl; string userFullName = "Sir"; Attachment pdfLink;     
            var sent = false;
            subject = "AuditBranchFeedbackTeamUser";


            var bodyString = "<tr>" +                               
                             "</tr>" +
                          "<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                            "<div style='margin:10px 0 20px 0'><font size = '2' ><span style='font-size:10pt'> Hello! </span></font></div>" +
                                            "<div style='margin:10px 0 20px 0'><font size = '2' ><span style='font-size:10pt'> We hope this email finds you well. </span></font></div>" +
                                            "<div><font face = 'Calibri' size = '2'><span style = 'font-size:10pt'>We are requesting you to provide your updated Feedback/Response on audit observation <b>" + AuditName + "</b> </span></font></div>" +
                                              
                                             "<div> &nbsp;</div> " +

                                       "</div> " +
                                   "</td> " +
                            "</tr> " +
                            "<tr>" +
                                "<td  style='height:50px;'> " +

                                         "<div style = 'margin-left:10px;color:black;'> " +
                                             "<a href='" + url + "' style = 'text-decoration: none;margin:10px 0px 30px 0px;border-radius:4px;padding:3px 5px;border: 0;color:#fff;background-color:#005daa;'>Click here </a>" +
                                          "</div> " +

                                "</td>" +

                                "<tr> " +

                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                            "<div style='margin:17px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Thank you for your attention.</span></font></div>" +
                                            "<div> &nbsp;</div> " +

                                       "</div> " +
                                    "</td> " +
                               "</tr> " +

                               "<tr> " +

                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Regards</span></font></div>" +
                                            "<div> &nbsp;</div> " +

                                       "</div> " +
                                    "</td> " +

                               "</tr> " +



							   "<tr> " +
								"<td style = 'text-align:left;'>" +
									"<div style = 'margin-left:10px;color:black;'> " +

										 "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Internal Audit & Compliance</span></font></div>" +
									     "<div> &nbsp;</div> " +

									"</div> " +
							   "</tr> " +

							   "<tr> " +
								"<td style = 'text-align:left;'>" +
									"<div style = 'margin-left:10px;color:black;'> " +

										 "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Green Delta Insurance Company Limited</span></font></div>" +
									    "<div> &nbsp;</div> " +

									"</div> " +
							   "</tr> " +

							//"<tr> " +
							//"<td style = 'text-align:left;'>" +
							//    "<div style = 'margin-left:10px;color:black;'> " +
							//         "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>GDIC, IAC</span></font></div>" +                                       
							//       "</div> " +
							//"</tr> " +


							"</tr> ";

            var message = new MailMessage();
            message.To.Add(new MailAddress(to));
            message.Subject = subject;

            sent = AuditBranchFeedbackTeamUserSendMail(to, bodyString, message);

            return sent;
        }

        public static bool AuditBranchFeedbackTeamUserSendMail(string To, string body, MailMessage message)
        {
            var content = "<html>" +
                                "<head>" +
                                "</head>" +
                                "<body>" +
                                    "<table border='0' width='100%' style='margin:auto;padding:10px;background-color: #F3F3F3;border:1px solid #0C143B;'>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%'>" +
                                                    "<tr>" +
                                                        "<td style='text-align: center;'>" +
                                                            "<h1>" +
                                                                "<img src='cid:companylogo'  width='350px' height='80px'/> " +
                                                            "</h1>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' cellpadding='0' cellspacing='0' style='text-align:center;width:100%;background-color: #FFFF;'>" +
                                                body
                                                + "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%' style='border-radius: 5px;text-align: center;'>" +


                                                    "<tr>" +
                                                        "<td>" +
                                                            "<div style='margin-top: 20px;color:black;'>" +


                                                            "</div>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                                "</html>";


            AlternateView av1 = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);

            message.IsBodyHtml = true;

            using (MailMessage mail = new MailMessage())
            {

				mail.From = new MailAddress("iac@green-delta.com", "Internal Audit & Compliance");
				//mail.From = new MailAddress("iac@green-delta.com");
				mail.To.Add(To);
                mail.Subject = "TeamFeedbackPending";
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("mail.green-delta.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("iac@green-delta.com", "9ty@u3{24E;%");
                    smtp.EnableSsl = true;

                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        return false;
                    }
                }
            }
        }

        #endregion AuditBranchFeedbackTeamUser


        #region AuditBranchFeedbackUser
        public static bool SendAuditBranchFeedbackUserMail(string to, string Approvurl,string AuditName,string str)
        {

            string subject = "AuditBranchFeedbackUser"; string mailBody; string url = Approvurl; string userFullName = "Sir"; Attachment pdfLink;         
            var sent = false;
            subject = "AuditBranchFeedbackUser";

            //string uniqueId = "image_" + Guid.NewGuid().ToString("N");
            //var imagePath = Path.Combine("images", "email.jpg");
            //var fullPath = Path.Combine(str, imagePath);


            var bodyString = "<tr>" +
    
                            "</tr>" +
                          "<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> Hello! </span></font></div>" +
                                            "<div> &nbsp;</div> " +

											"<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> We trust this message finds you well. </span></font></div>" +

											"<div> &nbsp;</div> " +

											"<div><font face = 'Calibri' size = '2'><span style = 'font-size:10pt'>We are requesting you to provide your Feedback/Response on the audit observations (<b>" + AuditName + "</b>) </span></font></div>" +
                                            
                                            "<div> &nbsp;</div> " +
                                       "</div> " +
                                   "</td> " +
                            "</tr> " +


                            "<tr>" +
                                "<td  style='height:50px;'> " +
                                        "<div style = 'margin-left:10px;color:black;'> " +
                                                "<a href='" + url + "' style = 'text-decoration: none;margin:0px 0px 0px 0px;border-radius:4px;padding:3px 5px;border: 0;color:#fff;background-color:#005daa;'>Click here </a>" +  
                                               "<div> &nbsp;</div> " +

										"</div> " +
                                "</td>" +  
                            "</tr> " +


							 "<tr>" +
								"<td  style='height:50px;'> " +

										"<div style = 'margin-left:10px;color:black;'> " +

												//"<font size = '2' ><span style='font-size:15pt;margin-top:5px;'> Process Flow – Response on Audit Issues </span></font>" +
                                                "<font size='2'><strong><span style='font-size:20pt;margin-top:5px;'>Process Flow – Response on Audit Issues</span></strong></font>"+

                                                "<div> &nbsp;</div> " +

										"</div> " +
								"</td>" +
							"</tr> " +


							"<tr>" +

								"<td  style='height:50px;'> " +
										"<div style = 'margin-left:10px;color:black;'> " +

												//"<font size = '2' ><span style='height:70px;width:225px;font-size:10pt;margin-top:15px;border-radius:5px;padding:18px;color:white;background-color: #114232;display: inline-block;'> Email Link </span></font>" +
												"<font size = '2' ><span style='font-size:10pt;margin-top:15px;border-radius:5px;padding:20px;color:white;background-color: #114232;display: inline-block;'> Email Link </span></font>" +
												"<font size = '2' ><span style='font-weight:bold;font-size:50px;' class=arrow>&rarr;</span></font>" +

												"<font size = '2' ><span style='font-size:10pt;margin-top:15px;border-radius:5px;padding:20px;color:white;background-color: #114232;display: inline-block;'> Auditee Response  </span></font>" +
												"<font size = '2' ><span style='font-weight:bold;font-size:50px;' class=arrow>&rarr;</span></font>" +

												"<font size = '2' ><span style='font-size:10pt;margin-top:15px;border-radius:5px;padding:20px;color:white;background-color: #114232;display: inline-block;'> Add Branch Feedback  </span></font>" +
												"<font size = '2' ><span style='font-weight:bold;font-size:50px;' class=arrow>&rarr;</span></font>" +

												"<font size = '2' ><span style='font-size:10pt;margin-top:15px;border-radius:5px;padding:20px;color:white;background-color: #114232;display: inline-block;'> Select Issue Heading  </span></font>" +
												"<font size = '2' ><span style='font-weight:bold;font-size:50px;' class=arrow>&rarr;</span></font>" +

												"<font size = '2' ><span style='font-size:10pt;margin-top:15px;border-radius:5px;padding:20px;color:white;background-color: #114232;display: inline-block;'> View Audit Issue Preview </span></font>" +
												"<font size = '2' ><span style='font-weight:bold;font-size:50px;' class=arrow>&rarr;</span></font>" +

												"<font size = '2' ><span style='font-size:10pt;margin-top:15px;border-radius:5px;padding:20px;color:white;background-color: #114232;display: inline-block;'>Feedback Heading  </span></font>" +
												"<font size = '2' ><span style='font-weight:bold;font-size:50px;' class=arrow>&rarr;</span></font>" +

												"<font size = '2' ><span style='font-size:10pt;margin-top:15px;border-radius:5px;padding:20px;color:white;background-color: #114232;display: inline-block;'> Provide response heading (Optional)  </span></font>" +
												"<font size = '2' ><span style='font-weight:bold;font-size:50px;' class=arrow>&rarr;</span></font>" +

												"<font size = '2' ><span style='font-size:10pt;margin-top:15px;border-radius:5px;padding:20px;color:white;background-color: #114232;display: inline-block;'> Details – Provide response details  </span></font>" +
												"<font size = '2' ><span style='font-weight:bold;font-size:50px;' class=arrow>&rarr;</span></font>" +

												"<font size = '2' ><span style='font-size:10pt;margin-top:15px;border-radius:5px;padding:20px;color:white;background-color: #114232;display: inline-block;'> Provide supporting docs (if applicable)  </span></font>" +
												"<font size = '2' ><span style='font-weight:bold;font-size:50px;' class=arrow>&rarr;</span></font>" +

												"<font size = '2' ><span style='font-size:10pt;margin-top:15px;border-radius:5px;padding:20px;color:white;background-color: #114232;display: inline-block;'> Save – (Draft) -> Branch Feedback (To issue to Audit Team)  </span></font>" +
																						
										"</div> " +
								"</td>" +
							"</tr> " +


							"<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                            "<div style='margin:17px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Thank you for your attention.</span></font></div>" +

                                            "<div> &nbsp;</div> " +

                                       "</div> " +
                                 "</td> " +

                            //"</tr> " +


                            "<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Regards</span></font></div>" +
                                            //"<div> &nbsp;</div> " +

                                       "</div> " +
                                    "</td> " +

                               "</tr> " +

                                "<tr> " +

                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                         "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Internal Audit & Compliance</span></font></div>" +
                                           //"<div> &nbsp;</div> " +

                                    "</div> " +
                               "</tr> " +


                               "<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                         "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Green Delta Insurance Company Limited</span></font></div>" +
                                       //"<div> &nbsp;</div> " +

                                    "</div> " +
                               "</tr> " +


                            "</tr> ";


           
            var message = new MailMessage();
            message.To.Add(new MailAddress(to));
            message.Subject = subject;


            //var imageAttachment = new Attachment("wwwroot/Images/auditimage.jpg", MediaTypeNames.Image.Jpeg);
            //imageAttachment.ContentDisposition.Inline = true;
            //imageAttachment.ContentId = uniqueId;
            //message.Attachments.Add(imageAttachment);
            //var Pathimg = Path.Combine("images", "98.jpg");
            //var fullPath = Path.Combine(str, Pathimg);
            //string imagePath = fullPath;
			//LinkedResource imageResource = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg);
			//imageResource.ContentId = "image1";
			//bodyString += "<tr><td><img src=\"cid:" + imageResource.ContentId + "\"></td></tr>";


			sent = AuditBranchFeedbackSendMail(to, bodyString, message,str);

            return sent;
        }

        public static bool AuditBranchFeedbackSendMail(string To, string body, MailMessage message,string str)
        {

            var content = "<html>" +
                                "<head>" +
                                "</head>" +
                                "<body>" +
                                    "<table border='0' width='100%' style='margin:auto;padding:10px;background-color: #F3F3F3;border:1px solid #0C143B;'>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%'>" +
                                                    "<tr>" +
                                                        "<td style='text-align: center;'>" +
                                                            "<h1>" +
                                                                "<img src='cid:companylogo'  width='350px' height='80px'/> " +
                                                            "</h1>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' cellpadding='0' cellspacing='0' style='text-align:center;width:100%;background-color: #FFFF;'>" +
                                                body
                                                + "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%' style='border-radius: 5px;text-align: center;'>" +


                                                    "<tr>" +
                                                        "<td>" +
                                                            "<div style='margin-top: 20px;color:black;'>" +


                                                            "</div>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                                "</html>";




            //var imagePath = Path.Combine("images", "email.jpg");
            //var fullPath = Path.Combine(str, imagePath);
            //LinkedResource linkedImage = new LinkedResource(fullPath, MediaTypeNames.Image.Jpeg);
            //linkedImage.ContentId = "companylogo";
            //AlternateView av1 = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);
            //av1.LinkedResources.Add(linkedImage);



            AlternateView av1 = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);
            message.IsBodyHtml = true;



            //message.AlternateViews.Add(av1);


            using (MailMessage mail = new MailMessage())
            {

                //mail.From = new MailAddress("iac@green-delta.com");
                mail.From = new MailAddress("iac@green-delta.com", "Internal Audit & Compliance");

                mail.To.Add(To);
                mail.Subject = "Audit BranchFeedback";
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("mail.green-delta.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("iac@green-delta.com", "9ty@u3{24E;%");
                    smtp.EnableSsl = true;
                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        return false;
                    }
                }

            }

        }

        #endregion



        #region AuditReportSendingEmail
        public static bool SendAuditReportMail(string to, string Approvurl, string AuditName)
        {
            string subject = "Audit Report"; string mailBody; string url = Approvurl; string userFullName = "Sir"; Attachment pdfLink;         
            var sent = false;
            subject = "Audit Report";
            var bodyString = "<tr>" +                          
                             "</tr>" +

                          "<tr> " +

                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> Hello! </span></font></div>" +
                                           "<div> &nbsp;</div> " +

                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> We trust this message finds you well.  </span></font></div>" +
                                             "<div> &nbsp;</div> " +
                                            "<div><font face = 'Calibri' size = '2'><span style = 'font-size:10pt'>We are pleased to inform you that the Final Audit report - <b>" + AuditName + "</b> has been concluded, requesting to click on the below link to view the report.</span></font></div>" +                                             
                                             "<div> &nbsp;</div> " +

                                       "</div> " +
                                   "</td> " +
                            "</tr> " +

                            "<tr>" +
                                "<td  style='height:0px;'> " +

                                          "<div style = 'margin-left:10px;color:black;'> " +
                                                "<a href='" + url + "' style = 'text-decoration: none;margin:0px 0px 0px 0px;border-radius:4px;padding:0px 0px;border: 0;color:#fff;background-color:#005daa;'>Click here </a>" +
                                          "</div> " +

                                "</td>" +


                                "<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                            "<div style='margin:17px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Should you have any queries or need clarification, our team is available for discussion. </span></font></div>" +
                                            "<div> &nbsp;</div> " +
                                            "<div> &nbsp;</div> " +
                                            "<div> &nbsp;</div> " +



                                       "</div> " +
                                    "</td> " +
                               "</tr> " +

                               "<tr> " +

                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Regards</span></font></div>" +
                                            "<div> &nbsp;</div> " +


                                       "</div> " +
                                    "</td> " +

                               "</tr> " +


                                "<tr> " +

                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                         "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Internal Audit & Compliance</span></font></div>" +
                                         "<div> &nbsp;</div> " +

                                    "</div> " +
                               "</tr> " +


                               "<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                         "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Green Delta Insurance Company Limited</span></font></div>" +
                                         "<div> &nbsp;</div> " +

                                    "</div> " +
                               "</tr> " +



                            "</tr> ";

            var message = new MailMessage();
            message.To.Add(new MailAddress(to));
            message.Subject = subject;
            sent = AuditReportSendMail(to, bodyString, message);
            return sent;
        }
        public static bool AuditReportSendMail(string To, string body, MailMessage message)
        {


            var content = "<html>" +
                                "<head>" +
                                "</head>" +
                                "<body>" +
                                    "<table border='0' width='100%' style='margin:auto;padding:10px;background-color: #F3F3F3;border:1px solid #0C143B;'>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%'>" +
                                                    "<tr>" +
                                                        "<td style='text-align: center;'>" +
                                                            "<h1>" +
                                                                "<img src='cid:companylogo'  width='350px' height='80px'/> " +
                                                            "</h1>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' cellpadding='0' cellspacing='0' style='text-align:center;width:100%;background-color: #FFFF;'>" +
                                                body
                                                + "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%' style='border-radius: 5px;text-align: center;'>" +


                                                    "<tr>" +
                                                        "<td>" +
                                                            "<div style='margin-top: 20px;color:black;'>" +


                                                            "</div>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                                "</html>";


            AlternateView av1 = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);

            message.IsBodyHtml = true;

            using (MailMessage mail = new MailMessage())
            {

                //mail.From = new MailAddress("iac@green-delta.com");
                mail.From = new MailAddress("iac@green-delta.com", "Internal Audit & Compliance");


                mail.To.Add(To);
                mail.Subject = "Audit Report";
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("mail.green-delta.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("iac@green-delta.com", "9ty@u3{24E;%");
                    smtp.EnableSsl = true;

                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        return false;
                    }
                }

            }

        }

        #endregion

        #region AuditFollowUpIssue
        public static bool FollowUpAuditIssueEamil(string to, string Approvurl, string AuditName)
        {
            string subject = "FollowUpAuditIssue"; string mailBody; string url = Approvurl; string userFullName = "Sir"; Attachment pdfLink;       
            var sent = false;
            subject = "FollowUpAuditIssue";

            var bodyString = "<tr>" +
                    
                            "</tr>" +
                          "<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> Hello! </span></font></div>" +

                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> We hope this email finds you well. </span></font></div>" +
                                            "<div><font face = 'Calibri' size = '2'><span style = 'font-size:10pt'>The audit issue- <b>" + AuditName + "</b> has been submitted for your review </span></font></div>" +                                            
                                               
                                       "</div> " +
                                   "</td> " +
                            "</tr> " +
                            "<tr>" +
                                "<td  style='height:50px;'> " +

                                         "<div style = 'margin-left:10px;color:black;'> " +
                                             "<a href='" + url + "' style = 'text-decoration: none;margin:10px 0px 30px 0px;border-radius:4px;padding:3px 5px;border: 0;color:#fff;background-color:#005daa;'>Click here </a>" +
                                          "</div> " +
                                "</td>" +

                                "<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                            "<div style='margin:18px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Thank you for your attention.</span></font></div>" +
                                            

                                       "</div> " +
                                    "</td> " +
                               "</tr> " +

                               "<tr> " +

                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +
                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Regards</span></font></div>" +                                          
                                       "</div> " +
                                    "</td> " +

                               "</tr> " +

                                "<tr> " +

                                "<td style = 'text-align:left;'>" +

                                    "<div style = 'margin-left:10px;color:black;'> " +

                                         "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>GDIC, IAC</span></font></div>" +
                                            
                                       "</div> " +

                               "</tr> " +

                            "</tr> ";

            var message = new MailMessage();
            message.To.Add(new MailAddress(to));
            message.Subject = subject;

            sent = FollowUpAuditIssueEamilSendMail(to, bodyString, message);

            return sent;
        }
        public static bool FollowUpAuditIssueEamilSendMail(string To, string body, MailMessage message)
        {
            var content = "<html>" +
                                "<head>" +
                                "</head>" +
                                "<body>" +
                                    "<table border='0' width='100%' style='margin:auto;padding:10px;background-color: #F3F3F3;border:1px solid #0C143B;'>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%'>" +
                                                    "<tr>" +
                                                        "<td style='text-align: center;'>" +
                                                            "<h1>" +
                                                                "<img src='cid:companylogo'  width='350px' height='80px'/> " +
                                                            "</h1>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' cellpadding='0' cellspacing='0' style='text-align:center;width:100%;background-color: #FFFF;'>" +
                                                body
                                                + "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%' style='border-radius: 5px;text-align: center;'>" +


                                                    "<tr>" +
                                                        "<td>" +
                                                            "<div style='margin-top: 20px;color:black;'>" +


                                                            "</div>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                                "</html>";


            AlternateView av1 = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);

            message.IsBodyHtml = true;

            using (MailMessage mail = new MailMessage())
            {

                mail.From = new MailAddress("iac@green-delta.com");
                mail.To.Add(To);
                mail.Subject = "FollowUp Audit Issue";
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("mail.green-delta.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("iac@green-delta.com", "9ty@u3{24E;%");
                    smtp.EnableSsl = true;

                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        return false;
                    }
                }
                
            }

        }

        #endregion

        #region IssuedeadLineLapsed
        public static bool IssuedeadLineLapsedEamil(string to, string Approvurl, string AuditName)
        {
            string subject = "IssuedeadLineLapsed"; string mailBody; string url = Approvurl; string userFullName = "Sir"; Attachment pdfLink;
            var sent = false;
            subject = "IssuedeadLineLapsed";

            var bodyString = "<tr>" +

                            "</tr>" +

                            "<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +
                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> Hello! </span></font></div>" +
                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> We hope this email finds you well. </span></font></div>" +
                                            "<div><font face = 'Calibri' size = '2'><span style = 'font-size:10pt'>The audit issue- <b>" + AuditName + "</b> has been submitted for your review </span></font></div>" +
                                              
                                       "</div> " +
                                   "</td> " +
                            "</tr> " +


                            "<tr>" +
                                "<td  style='height:50px;'> " +
                                         "<div style = 'margin-left:10px;color:black;'> " +
                                              "<a href='" + url + "' style = 'text-decoration: none;margin:10px 0px 30px 0px;border-radius:4px;padding:3px 5px;border: 0;color:#fff;background-color:#005daa;'>Click here </a>" +
                                          "</div> " +
                                "</td>" +


                                "<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +
                                            "<div style='margin:17px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Thank you for your attention.</span></font></div>" +
                                            
                                       "</div> " +
                                    "</td> " +
                               "</tr> " +


                               "<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +
                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Regards</span></font></div>" +
                                            
                                       "</div> " +
                                    "</td> " +

                               "</tr> " +


                                "<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                         "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>GDIC, IAC</span></font></div>" +
                                       "</div> " +
                               "</tr> " +

                            "</tr> ";

            var message = new MailMessage();
            message.To.Add(new MailAddress(to));
            message.Subject = subject;

            sent = IssuedeadLineLapsedSendMail(to, bodyString, message);

            return sent;
        }

        public static bool IssuedeadLineLapsedSendMail(string To, string body, MailMessage message)
        {
            var content = "<html>" +
                                "<head>" +
                                "</head>" +
                                "<body>" +
                                    "<table border='0' width='100%' style='margin:auto;padding:10px;background-color: #F3F3F3;border:1px solid #0C143B;'>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%'>" +
                                                    "<tr>" +
                                                        "<td style='text-align: center;'>" +
                                                            "<h1>" +
                                                                "<img src='cid:companylogo'  width='350px' height='80px'/> " +
                                                            "</h1>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' cellpadding='0' cellspacing='0' style='text-align:center;width:100%;background-color: #FFFF;'>" +
                                                body
                                                + "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%' style='border-radius: 5px;text-align: center;'>" +


                                                    "<tr>" +
                                                        "<td>" +
                                                            "<div style='margin-top: 20px;color:black;'>" +


                                                            "</div>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                                "</html>";


            AlternateView av1 = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);

            message.IsBodyHtml = true;

            using (MailMessage mail = new MailMessage())
            {

                mail.From = new MailAddress("iac@green-delta.com");
                mail.To.Add(To);
                mail.Subject = "Issue DeadLin Lapsed";
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("mail.green-delta.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("iac@green-delta.com", "9ty@u3{24E;%");
                    smtp.EnableSsl = true;

                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        return false;
                    }
                }
                
            }

        }

        #endregion

        #region TotalPendingIssuesReview
        public static bool TotalPendingIssuesReviewEamil(string to, string Approvurl, string AuditName)
        {
            string subject = "TotalPendingIssuesReviewEamil"; string mailBody; string url = Approvurl; string userFullName = "Sir"; Attachment pdfLink;
            var sent = false;
            subject = "TotalPendingIssuesReviewEamil";

            var bodyString = "<tr>" +

                            "</tr>" +
                          "<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                    "<div style='margin:10px 0 20px 0'><font size = '2' ><span style='font-size:10pt'> Hello! </span></font></div>" +

                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> We hope this email finds you well. </span></font></div>" +
                                            "<div><font face = 'Calibri' size = '2'><span style = 'font-size:10pt'>The audit issue- <b>" + AuditName + "</b> has been submitted for your review </span></font></div>" +

                                             "<div> &nbsp;</div> " +
                                       "</div> " +
                                   "</td> " +
                            "</tr> " +
                            "<tr>" +

                                "<td  style='height:50px;'> " +

                                         "<div style = 'margin-left:10px;color:black;'> " +
                                               "<a href='" + url + "' style = 'text-decoration: none;margin:0px 0px 0px 0px;border-radius:4px;padding:3px 5px;border: 0;color:#fff;background-color:#005daa;'>Click here </a>" +
                                          "</div> " +

                                "</td>" +

                                "<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                            "<div style='margin:17px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Thank you for your attention.</span></font></div>" +
                                            //"<div> &nbsp;</div> " +

                                       "</div> " +
                                    "</td> " +
                               "</tr> " +

                               "<tr> " +

                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Regards</span></font></div>" +
                                            //"<div> &nbsp;</div> " +

                                       "</div> " +
                                    "</td> " +

                               "</tr> " +

                                "<tr> " +

                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                         "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>GDIC, IAC</span></font></div>" +
                                          //"<div> &nbsp;</div> " +

                                       "</div> " +

                               "</tr> " +

                            "</tr> ";

            var message = new MailMessage();
            message.To.Add(new MailAddress(to));
            message.Subject = subject;

            sent = TotalPendingIssuesReviewSendMail(to, bodyString, message);

            return sent;
        }

        public static bool TotalPendingIssuesReviewSendMail(string To, string body, MailMessage message)
        {
            var content = "<html>" +
                                "<head>" +
                                "</head>" +
                                "<body>" +
                                    "<table border='0' width='100%' style='margin:auto;padding:10px;background-color: #F3F3F3;border:1px solid #0C143B;'>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%'>" +
                                                    "<tr>" +
                                                        "<td style='text-align: center;'>" +
                                                            "<h1>" +
                                                                "<img src='cid:companylogo'  width='350px' height='80px'/> " +
                                                            "</h1>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' cellpadding='0' cellspacing='0' style='text-align:center;width:100%;background-color: #FFFF;'>" +
                                                body
                                                + "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%' style='border-radius: 5px;text-align: center;'>" +


                                                    "<tr>" +
                                                        "<td>" +
                                                            "<div style='margin-top: 20px;color:black;'>" +


                                                            "</div>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                                "</html>";

            AlternateView av1 = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);
            message.IsBodyHtml = true;

            using (MailMessage mail = new MailMessage())
            {

                mail.From = new MailAddress("iac@green-delta.com");
                mail.To.Add(To);
                mail.Subject = "Total Pending Issues Review";
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("mail.green-delta.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("iac@green-delta.com", "9ty@u3{24E;%");
                    smtp.EnableSsl = true;

                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        return false;
                    }
                }
            }
        }

        #endregion

        //For Audit Team User
        public static bool SendAuditUserMail(string to, string Approvurl, string Name, string Code)
        {
            string subject = "AuditUser"; string mailBody; string url = Approvurl; string userFullName = "Sir"; Attachment pdfLink;     
            var sent = false;
            subject = "AuditUser";

            var bodyString = "<tr>" +

                             "</tr>" +
                          "<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +
                                         
                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> Hello! </span></font></div>" +
                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> We hope this email finds you well. </span></font></div>" +
                                            "<div><font face = 'Calibri' size = '2'><span style = 'font-size:10pt'><b>" + Name + "</b> is created.Please find it out. </span></font></div>" +
                                            "<div> &nbsp;</div> " +

                                       "</div> " +
                                   "</td> " +
                            "</tr> " +
                            "<tr>" +
                                "<td  style='height:25px;padding-left: 100px;'> " +

                                          "<div style = 'margin-left:10px;color:black;'> " +
                                                 "<a href='" + url + "' style = 'text-decoration: none;margin:0px 0px 0px 0px;border-radius:4px;padding:3px 5px;border: 0;color:#fff;background-color:#005daa;'>Click here </a>" +
                                          "</div> " +
                                "</td>" +


                                "<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                            "<div style='margin:17px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Thank you for your attention.</span></font></div>" +                                                                           
                                            //"<div> &nbsp;</div> " +

                                       "</div> " +
                                    "</td> " +
                               "</tr> " +

                               "<tr> " +

                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Regards</span></font></div>" +
                                            //"<div> &nbsp;</div> " +

                                       "</div> " +
                                    "</td> " +           
                               "</tr> " +



                                "<tr> " +

                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                         "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>GDIC, IAC</span></font></div>" +
                                           "<div> &nbsp;</div> " +

                                       "</div> " +
                                   
                               "</tr> " +


                         "</tr> ";

            var message = new MailMessage();
            message.To.Add(new MailAddress(to));
            message.Subject = subject;

            sent = AuditUserSendMail(to, bodyString, message);

            return sent;
        }

        public static bool AuditUserSendMail(string To, string body, MailMessage message)
        {
            var content = "<html>" +
                                "<head>" +
                                "</head>" +
                                "<body>" +
                                    "<table border='0' width='100%' style='margin:auto;padding:10px;background-color: #F3F3F3;border:1px solid #0C143B;'>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%'>" +
                                                    "<tr>" +
                                                        "<td style='text-align: center;'>" +
                                                            "<h1>" +
                                                                "<img src='cid:companylogo'  width='350px' height='80px'/> " +
                                                            "</h1>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' cellpadding='0' cellspacing='0' style='text-align:center;width:100%;background-color: #FFFF;'>" +
                                                body
                                                + "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%' style='border-radius: 5px;text-align: center;'>" +


                                                    "<tr>" +
                                                        "<td>" +
                                                            "<div style='margin-top: 20px;color:black;'>" +


                                                            "</div>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                                "</html>";


            AlternateView av1 = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);

            message.IsBodyHtml = true;

            using (MailMessage mail = new MailMessage())
            {

                mail.From = new MailAddress("iac@green-delta.com");
                mail.To.Add(To);
                mail.Subject = "AuditUser";
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("mail.green-delta.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("iac@green-delta.com", "9ty@u3{24E;%");
                    smtp.EnableSsl = true;

                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        return false;
                    }
                }
            }
        }
        //End Of Audit Team User


        //ForAuditIssueMailToReviewerUsers
        public static bool SendAuditIssueMail(string to, string Approvurl,string Name)
        {
            string subject = "AuditIssue"; string mailBody; string url = Approvurl; string userFullName = "Sir"; Attachment pdfLink;         
            var sent = false;
            subject = "AuditIssue";

            var bodyString = "<tr>" +

                             "</tr>" +
                          "<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +


                                             "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> Hello! </span></font></div>" +
                                             "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> We hope this email finds you well. </span></font></div>" +
                                             "<div><font face = 'Calibri' size = '2'><span style = 'font-size:10pt'>We are requesting you to provide your Feedback/Response of the audit observations <b>" + Name + "</b> </span></font></div>" +
                                               
                                             "<div> &nbsp;</div> " +

                                       "</div> " +
                                   "</td> " +
                            "</tr> " +

                            "<tr>" +
                                "<td  style='height:50px;padding-left: 0px;'> " +

                                          "<div style = 'margin-left:10px;color:black;'> " +
                                                 "<a href='" + url + "' style = 'text-decoration: none;margin:10px 0px 30px 0px;border-radius:4px;padding:3px 5px;border: 0;color:#fff;background-color:#005daa;'>Click here </a>" +
                                          "</div> " +

                                "</td>" +


                                "<tr> " +
                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                            "<div style='margin:17px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Thank you for your attention.</span></font></div>" +
                                            //"<div> &nbsp;</div> " +

                                       "</div> " +
                                    "</td> " +
                               "</tr> " +

                               "<tr> " +

                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Regards</span></font></div>" +
                                            //"<div> &nbsp;</div> " +

                                       "</div> " +
                                    "</td> " +

                               "</tr> " +

                                "<tr> " +

                                "<td style = 'text-align:left;'>" +
                                    "<div style = 'margin-left:10px;color:black;'> " +

                                         "<div style='margin:10px 0 20px 0'><font size = '2' ><span style='font-size:10pt'>GDIC, IAC</span></font></div>" +
                                           //"<div> &nbsp;</div> " +

                                       "</div> " +
                               "</tr> " +

                            "</tr> ";

            var message = new MailMessage();
            message.To.Add(new MailAddress(to));
            message.Subject = subject;
            sent = AuditIssueSendMail(to, bodyString, message);

            return sent;
        }

        public static bool AuditIssueSendMail(string To, string body, MailMessage message)
        {
            var content = "<html>" +
                                "<head>" +
                                "</head>" +
                                "<body>" +
                                    "<table border='0' width='100%' style='margin:auto;padding:10px;background-color: #F3F3F3;border:1px solid #0C143B;'>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%'>" +
                                                    "<tr>" +
                                                        "<td style='text-align: center;'>" +
                                                            "<h1>" +
                                                                "<img src='cid:companylogo'  width='350px' height='80px'/> " +
                                                            "</h1>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' cellpadding='0' cellspacing='0' style='text-align:center;width:100%;background-color: #FFFF;'>" +
                                                body
                                                + "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%' style='border-radius: 5px;text-align: center;'>" +


                                                    "<tr>" +
                                                        "<td>" +
                                                            "<div style='margin-top: 20px;color:black;'>" +


                                                            "</div>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                                "</html>";


            AlternateView av1 = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);
            message.IsBodyHtml = true;

            using (MailMessage mail = new MailMessage())
            {

                //mail.From = new MailAddress("iac@green-delta.com");
                mail.From = new MailAddress("iac@green-delta.com", "Internal Audit & Compliance");
                mail.To.Add(To);
                mail.Subject = "AuditIssue";
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("mail.green-delta.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("iac@green-delta.com", "9ty@u3{24E;%");
                    smtp.EnableSsl = true;
                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        return false;
                    }
                }
            }
        }

        //End Of Audit Issue

        public static bool SendHODMail(string to, string HODurl, string AuditName)
        {
            string subject = "AuditIssueApproval"; string mailBody; string url = HODurl; string userFullName = "Sir"; Attachment pdfLink;        
            var sent = false;
            subject = "AuditIssueApproval";

            var bodyString = "<tr>" +

                            "</tr>" +

                            "<tr> " +
                                  "<td style = 'text-align:left;'>" +
                                      "<div style = 'margin-left:10px;color:black;'> " +

                                             "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> Hello! </span></font></div>" +
                                             "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> We hope this email finds you well. </span></font></div>" +
                                             "<div><font face = 'Calibri' size = '2'><span style = 'font-size:10pt'>We are requesting you to approve Audit Issues- <b>" + AuditName + "</b> </span></font></div>" +

                                            
                                       "</div> " +
                                   "</td> " +
                             "</tr> " +

                           "<tr>" +

                                "<td  style='height:50px;'> " +   
                                
                                         "<a href='" + url + "' style = 'text-decoration: none;margin:10px 0px 30px 0px;border-radius:4px;padding:0px 0px;border: 0;color:#fff;background-color:#005daa;'>Click here </a>" +
                                         
                                "</td>" +

                                "<tr> " +
                                   "<td style = 'text-align:left;'>" +
                                       "<div style = 'margin-left:10px;color:black;'> " +
                                            "<div style='margin:17px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Thank you for your attention.</span></font></div>" +
                                            
                                       "</div> " +
                                    "</td> " +
                               "</tr> " +

                               "<tr> " +
                                    "<td style = 'text-align:left;'>" +
                                       "<div style = 'margin-left:10px;color:black;'> " +
                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Regards</span></font></div>" +
                                           
                                       "</div> " +
                                    "</td> " +
                               "</tr> " +

                                "<tr> " +
                                     "<td style = 'text-align:left;'>" +
                                       "<div style = 'margin-left:10px;color:black;'> " +
                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>GDIC, IAC</span></font></div>" +                                           
                                       "</div> " +
                               "</tr> " +

                          "</tr> ";

            var message = new MailMessage();
            message.To.Add(new MailAddress(to));
            message.Subject = subject;
            sent = SendHODMail(to, bodyString, message);
            return sent;
        }

        public static bool SendHODMail(string To, string body, MailMessage message)
        {

            var content = "<html>" +
                                "<head>" +
                                "</head>" +
                                "<body>" +
                                    "<table border='0' width='100%' style='margin:auto;padding:10px;background-color: #F3F3F3;border:1px solid #0C143B;'>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%'>" +
                                                    "<tr>" +
                                                        "<td style='text-align: center;'>" +
                                                            "<h1>" +
                                                                "<img src='cid:companylogo'  width='350px' height='80px'/> " +
                                                            "</h1>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' cellpadding='0' cellspacing='0' style='text-align:center;width:100%;background-color: #FFFF;'>" +
                                                body
                                                + "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%' style='border-radius: 5px;text-align: center;'>" +


                                                    "<tr>" +
                                                        "<td>" +
                                                            "<div style='margin-top: 20px;color:black;'>" +


                                                            "</div>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                                "</html>";


            AlternateView av1 = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);

            message.IsBodyHtml = true;

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("iac@green-delta.com");
                mail.To.Add(To);
                mail.Subject = "AuditIssueApproval";
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("mail.green-delta.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("iac@green-delta.com", "9ty@u3{24E;%");
                    smtp.EnableSsl = true;

                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        return false;
                    }
                }
            }
        }
    }
}

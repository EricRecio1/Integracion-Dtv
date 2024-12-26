using System;
using System.Net.Mail;
using IntegracionOcasaDtv;

public class MailHelper
{
	public static void SendMail(string message)
	{
		string Mail_To = ConfigurationHelper.GetValue("Configuration", "Mail_To");
		string Mail_From = ConfigurationHelper.GetValue("Configuration", "Mail_From");
		string Mail_Server = ConfigurationHelper.GetValue("Configuration", "Mail_Server");
		string Mail_Port = ConfigurationHelper.GetValue("Configuration", "Mail_Port");
		string Mail_Subjet = (Mail_Subjet = ConfigurationHelper.GetValue("Configuration", "Mail_Subject"));
		SendMail(Mail_To, Mail_From, Mail_Server, Mail_Port, Mail_Subjet, message);
	}

	private static void SendMail(string Mail_To, string Mail_From, string Mail_Host, string Mail_Port, string Mail_Subjet, string Mail_Body)
	{
		MailMessage Mail = new MailMessage();
		SmtpClient smtp = new SmtpClient();
		try
		{
			if (Mail_To.ToString().Contains(";"))
			{
				string[] array = Mail_To.ToString().Split(';');
				foreach (string cc in array)
				{
					if (cc != string.Empty)
					{
						Mail.To.Add(cc);
					}
				}
			}
			else
			{
				Mail.To.Add(Mail_To.ToString());
			}
			Mail.IsBodyHtml = true;
			Mail.From = new MailAddress(Mail_From);
			Mail.Subject = Mail_Subjet;
			Mail.Body = "<html>" + Mail_Body + "</html>";
			try
			{
				smtp.Port = Convert.ToInt32(Mail_Port);
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
			smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
			smtp.UseDefaultCredentials = false;
			smtp.Host = Mail_Host;
			smtp.Send(Mail);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
		finally
		{
			Mail = null;
			smtp = null;
		}
	}
}

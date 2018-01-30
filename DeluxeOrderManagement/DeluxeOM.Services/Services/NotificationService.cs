﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeluxeOM.Utils.Config;
using System.Net;
using System.Net.Mail;
using DeluxeOM.Models;
using DeluxeOM.Models.BulkUploader;
using DeluxeOM.Models.Common;
using DeluxeOM.Repository;

namespace DeluxeOM.Services
{
    public class NotificationService : INotificationService
    {
        private INotificationRepository _repository = null;
        public NotificationService()
        {
            _repository = new NotificationRepository();
        }

        public void SendJobNotification(JobNotificationType notificationTYpe, JobStatus jobStatus)
        {
            dlxMailMessage dlxMessage = DeluxeOM.Services.Common.Mapper.GetMailMessageModel(_repository.GetNotificationMessage(notificationTYpe)) ;
            //bool isSuccess = jobStatus.Success;
            //int jobId = jobStatus.JobId ;
            
            if (!App.Config.EnableEmailNotifications || !dlxMessage.IsActive)
            {
                return;
            }
            if (string.IsNullOrEmpty(dlxMessage.To))
            {
                return;
            }
            string recordImporetd = jobStatus.Success ? jobStatus.NoOfRecordsImported.ToString() : string.Empty;
            string body = jobStatus.Success ? dlxMessage.SuccessBody : dlxMessage.FailureBody;
            body = string.Format(body, jobStatus.FileName, recordImporetd, DateTime.Now.ToString());
            dlxMessage.SuccessBody = body;

            string subject = jobStatus.Success ? dlxMessage.SuccessSubject : dlxMessage.FailureSubject;
            dlxMessage.SuccessSubject = subject;

            SendEmail(dlxMessage);
        }

        public void SendAnnouncementNewTitlesNotification(JobStatus jobStatus)
        {
            dlxMailMessage dlxMessage = DeluxeOM.Services.Common.Mapper.GetMailMessageModel(_repository.GetNotificationMessage(JobNotificationType.NewTitlesNotification));

            if (!App.Config.EnableEmailNotifications || !dlxMessage.IsActive)
            {
                return;
            }
            if (string.IsNullOrEmpty(dlxMessage.To))
            {
                return;
            }

            string body = dlxMessage.SuccessBody;
            body = string.Format(body, jobStatus.FileName, getNewTitlesHTML(jobStatus.JobId), DateTime.Now.ToString());
            dlxMessage.SuccessBody = body;

            SendEmail(dlxMessage);

            #region Commented
            //var emailConfig = App.Config.SmtpMail;
            //MailMessage message = new MailMessage()
            //{
            //    From = new MailAddress(dlxMessage.From),
            //    Subject = subject
            //};
            //if (notificationTYpe == JobNotificationType.LoadAnnouncementNotification)
            //    message.Body = string.Format("{0}<br/><br/>{1}", body, getAnnouncementJobBody(jobStatus.JobId));
            //else
            //    message.Body = body;

            //dlxMessage.To.Split(',').ToList().ForEach(x => message.To.Add(x));
            //message.IsBodyHtml = true;

            //SmtpClient client = new SmtpClient();
            //client.Host = emailConfig.Server;
            //client.Port = int.Parse(emailConfig.Port);
            //client.UseDefaultCredentials = false;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.EnableSsl = emailConfig.EnableSSL;
            //client.Credentials = new NetworkCredential(emailConfig.UserName, emailConfig.Password);
            //client.Send(message); 
            #endregion
        }

        public void SendEmail(dlxMailMessage dlxMessage)
        {
            if (!App.Config.EnableEmailNotifications)
            {
                return;
            }

            var emailConfig = App.Config.SmtpMail;
            MailMessage message = new MailMessage()
            {
                From = new MailAddress(dlxMessage.From),
                Body = dlxMessage.SuccessBody,
                Subject = dlxMessage.SuccessSubject
            };
            dlxMessage.To.Split(',').ToList().ForEach(x => message.To.Add(x));
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient();
            client.Host = emailConfig.Server;
            client.Port = int.Parse(emailConfig.Port);
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = emailConfig.EnableSSL;
            client.Credentials = new NetworkCredential(emailConfig.UserName, emailConfig.Password);
            client.Send(message);
        }

        private string getNewTitlesHTML(int jobId)
        {
            List<string> newtitles = _repository.GetNewTitles(jobId);
            if (newtitles == null || !newtitles.Any())
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("<table border='1' cellpadding='0' cellspacing='0'>");
            sb.Append("<tr>");
            sb.Append("<td>New Titles</td>");
            sb.Append("</tr>");
            foreach (string title in newtitles)
            {
                sb.Append("<tr>");
                sb.Append("<td>" + title + "</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");

            return sb.ToString();
        }
        public List<Notification> GetAllNotification()
        {
            return _repository.GetAllNotification();
        }

        public bool UpdateNotification(List<Notification> notification)
        {
            return _repository.UpdateNotification(notification);
        }

        
    }
}

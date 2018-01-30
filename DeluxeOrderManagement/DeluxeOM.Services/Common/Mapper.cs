using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeluxeOM.Models;
using DeluxeOM.Models.Common;
using DeluxeOM.Repository;

namespace DeluxeOM.Services.Common
{
    public class Mapper
    {
        public static dlxMailMessage GetMailMessageModel(NotificationEntity ety)
        {
            return new dlxMailMessage()
            {
                From = ety.FromEmailAddress,
                To = ety.ToEmailAddress,
                SuccessSubject = ety.SuccessSubject,
                FailureSubject = ety.FailureSubject,
                SuccessBody = ety.SuccessBody,
                FailureBody = ety.FailureBody,
                IsActive = ety.Enabled 
                //NotificationTYpe = ety.Name
            };
        }
    }
}

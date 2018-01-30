using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeluxeOM.Repository;
using DeluxeOM.Models.BulkUploader;
using DeluxeOM.Models.Common;

namespace DeluxeOM.Services
{
    public class BulkUploadService : ServiceBase, DeluxeOM.Services.IBulkUploadService
    {
        IBulkUploadRepository _repository = null;
        INotificationService _nfnService = null;
        public BulkUploadService()
        {
            _repository = new BulkUploadRepository ();
            _nfnService = new NotificationService();
        }

        public FtpConfiguration GetFtpConfig(FtpConfigurationType configType)
        {
            return _repository.GetFtpConfig(configType);
        }


        public int PopulateAnnouncement(string fileName)
        {
           return _repository.BulkInsertAnnouncement(fileName);
        }

        public int PopulatePipeLineOrder(string fileName)
        {
            return _repository.BulkInsertPipeLineOrder(fileName);
        }

        public int PopulatePriceReport(string fileName)
        {
            return _repository.BulkInsertPriceReport(fileName);
        }

        public int PopulateQCReport(string fileName)
        {
            return _repository.BulkInsertQCReport(fileName);
        }

        public void PopulateVID(string fileName)
        {
            _repository.BulkInsertVID(fileName);
        }
        
        public void InsertPipeLineOrder()
        {
            _repository.InsertPipeLineOrder();
        }

        public void InsertPriceReport()
        {
            _repository.InsertPriceReport();
        }

        public void InsertQCReport()
        {
            _repository.InsertQCReport();
        }

        public void InsertAnnouncement(int id, DateTime? firstAnnouncedDate)
        {
            _repository.InsertAnnouncement(id, firstAnnouncedDate);
        }

        public void InsertJobAnnouncement(int id)
        {
            _repository.InsertJobAnnouncement(id);
        }

        public void ProcessTitleReport()
        {
            _repository.ProcessTitleReport();
        }

        public int JobLog(Job jobModel)
        {
            JOB job = new JOB();
            job.JobType = jobModel.JobType;
            job.TriggeredBy = jobModel.TriggeredBy;
            int id=_repository.JobLog(job);
            return id;
        }

        public void JobItemLog(JobItem jobItemModel)
        {
            Jobs_Items jobItem = new Jobs_Items()
            {
                JobId = jobItemModel.JobId,
                StartDate = jobItemModel.StartDate,
                EndDate = jobItemModel.EndDate,
                Status = jobItemModel.Status,
                TaskName = jobItemModel.TaskName,
                Description = jobItemModel.Description
            };
            _repository.JobItemLog(jobItem);

        }

        public void UpdateJobLog(Job jobModel)
        {
            JOB job = new JOB();
            job.Id = jobModel.Id;
            job.Status = jobModel.Status;
            job.Description = jobModel.Description;
            job.FileName = jobModel.FileName;
            _repository.UpdateJobLog(job);
        }

        public bool IsFileExists(string fileName)
        {
            return _repository.IsFileExists(fileName);
        }

        public void ProcessNotifications(Models.JobNotificationType jobNotificationType, JobStatus jobStatus)
        {
            
            _nfnService.SendJobNotification(jobNotificationType,  jobStatus);
        }

        public void SendAnnouncementNewTitlesNotification(JobStatus jobStatus)
        {
            _nfnService.SendAnnouncementNewTitlesNotification(jobStatus);
        }
    }
}

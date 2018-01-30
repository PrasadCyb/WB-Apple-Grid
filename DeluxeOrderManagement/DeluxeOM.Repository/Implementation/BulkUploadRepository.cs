using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeluxeOM.Models.BulkUploader;

namespace DeluxeOM.Repository
{
    public class BulkUploadRepository : IBulkUploadRepository
    {
        public FtpConfiguration GetFtpConfig(FtpConfigurationType configType)
        {
            var _ftpContext = new DeluxeOrderManagementEntities();
            var ftpConfiguration = (from ftpConfig in _ftpContext.FTPConfigs
                     where ftpConfig.FileName == configType.ToString()
                     select new FtpConfiguration { FtpUserName = ftpConfig.UserName,
                                                   FtpPassword = ftpConfig.Password,
                                                   FtpDirecrory = ftpConfig.FtpDirectory,
                                                   Host = ftpConfig.Host,
                                                   DownloadLocalDirectory = ftpConfig.DownloadTo,
                                                   Port = ftpConfig.Port.Value  ,
                                                   EnableSSL = ftpConfig.EnableSSL.Value ,
                                                   ArchiveDirectory = ftpConfig.FtpArchivalDirectory
                     }).FirstOrDefault();
            return ftpConfiguration;
        }

        public void BulkInsertVID(string fileName)
        {
            var _bulkInsertContext = new DeluxeOrderManagementEntities();
            _bulkInsertContext.usp_InsertDataToApplePriceStaging(fileName);
        }

        public int BulkInsertAnnouncement(string fileName)
        {
            var _bulkInsertContext = new DeluxeOrderManagementEntities();
            return _bulkInsertContext.usp_InsertDataToAnnouncementStaging(fileName);
        }

        public int BulkInsertPipeLineOrder(string fileName)
        {
            var _bulkInsertContext = new DeluxeOrderManagementEntities();
            return _bulkInsertContext.usp_InsertDataToPipeLineStaging(fileName);
        }

        public int BulkInsertPriceReport(string fileName)
        {
            var _bulkInsertContext = new DeluxeOrderManagementEntities();
            return _bulkInsertContext.usp_InsertDataToApplePriceStaging(fileName);
        }

        public int BulkInsertQCReport(string fileName)
        {
            var _bulkInsertContext = new DeluxeOrderManagementEntities();
            return _bulkInsertContext.usp_InsertDataToAppleQCStaging(fileName);
        }

        public void InsertPriceReport()
        {
            var _bulkInsertContext = new DeluxeOrderManagementEntities();
            _bulkInsertContext.usp_Insert_PriceReport();
        }

        public void InsertQCReport()
        {
            var _bulkInsertContext = new DeluxeOrderManagementEntities();
            _bulkInsertContext.usp_Insert_QCReport();
        }

        public void InsertPipeLineOrder()
        {
            var _bulkInsertContext = new DeluxeOrderManagementEntities();
            _bulkInsertContext.usp_Insert_PipelineOrder();
        }

        public void InsertAnnouncement(int id, DateTime? firstAnouncedDate)
        {
            var _bulkInsertContext = new DeluxeOrderManagementEntities();
            _bulkInsertContext.usp_Insert_Announcements(id, firstAnouncedDate);
        }

        public void InsertJobAnnouncement(int id)
        {
            var _bulkInsertContext = new DeluxeOrderManagementEntities();
            //_bulkInsertContext.usp_Insert_Jobs_Announcements(id);
        }

        public void ProcessTitleReport()
        {
            var _bulkInsertContext = new DeluxeOrderManagementEntities();
            _bulkInsertContext.usp_Get_Titles();
        }

        public int JobLog(JOB job)
        {
            var _jobContext = new DeluxeOrderManagementEntities();
            _jobContext.JOBS.Add(job);
            _jobContext.SaveChanges();
            return job.Id;
        }

        public void JobItemLog(Jobs_Items jobItem)
        {
            var _jobContext = new DeluxeOrderManagementEntities();
            _jobContext.Jobs_Items.Add(jobItem);
            _jobContext.SaveChanges();

        }

        public void UpdateJobLog(JOB job)
        {
            var _jobContext = new DeluxeOrderManagementEntities();
            var jobUpdate = _jobContext.JOBS.Where(x => x.Id == job.Id).FirstOrDefault();
            jobUpdate.Status = job.Status;
            jobUpdate.Description = job.Description;
            jobUpdate.FileName  = job.FileName ;
            _jobContext.SaveChanges();
        }

        public bool IsFileExists(string fileName)
        {
            using (var _context = new DeluxeOrderManagementEntities())
            {
                return _context.JOBS.Any(x => x.FileName == fileName && (x.Status.HasValue  && x.Status.Value) );
            }
        }


    }
}

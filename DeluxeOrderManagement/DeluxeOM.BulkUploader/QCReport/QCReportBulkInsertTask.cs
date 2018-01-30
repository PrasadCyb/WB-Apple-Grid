﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeluxeOM.BulkUploader;
using DeluxeOM.Models.BulkUploader;
using DeluxeOM.Services;

namespace DeluxeOM.QC.BulkUploader
{
    public class QCReportBulkInsertTask : ITask 
    {
        IBulkUploadService _service = null;
        string _fileName = string.Empty;
        public QCReportBulkInsertTask(string fileName)
        {
            _service = new BulkUploadService();
            _fileName = fileName;
        }

        public string Name
        {
            get
            {
                return "QCReport Bulk Insert Task";
            }
        }
        private dlxTaskResult _taskResult { get; set; }
        public dlxTaskResult PreviousTaskResult
        {
            get; set;
        }

        public void Execute(int id)
        {
            JobItem jobItem = new JobItem();
            jobItem.StartDate = DateTime.UtcNow;
            jobItem.JobId = id;
            jobItem.TaskName = Name;
            try
            {
                var taskresult = this.PreviousTaskResult.TaskData;

                var ftpConfig = _service.GetFtpConfig(FtpConfigurationType.QCReport );
                string filePath = string.IsNullOrEmpty(_fileName) ? taskresult.FilePath
                    : string.Format("{0}{1}/{2}", ftpConfig.DownloadLocalDirectory, ftpConfig.FtpDirecrory, _fileName);

                int recordsImported = _service.PopulateQCReport(filePath);
                //taskresult.NoOfRecordImported = recordsImported;
                _taskResult = new dlxTaskResult()
                {
                    IsSuccess = true,
                    TaskData = string.IsNullOrEmpty(_fileName) ? taskresult : new dlxTaskData { NoOfRecordImported = recordsImported, FilePath = filePath }
                };
                jobItem.Status = true;
                jobItem.Description = "QC Report Bulk Insert Successful";
                jobItem.EndDate = DateTime.UtcNow;
                _service.JobItemLog(jobItem);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                _taskResult = new dlxTaskResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "QC report Bulk Insert Failed"
                };
                jobItem.Status = false;
                jobItem.Description = string.Format("Error: {0} Inner Exception: {1} Error Message: {2}", _taskResult.ErrorMessage, ex.InnerException == null ? "" : ex.InnerException.ToString(), ex.Message);
                jobItem.EndDate = DateTime.UtcNow;
                _service.JobItemLog(jobItem);
            }
        }

        public dlxTaskResult GetTaskResult()
        {
            return _taskResult;
        }
        
    }
}

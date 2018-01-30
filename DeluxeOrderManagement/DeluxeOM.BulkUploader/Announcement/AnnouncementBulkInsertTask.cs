using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeluxeOM.BulkUploader;
using DeluxeOM.Models.BulkUploader;
using DeluxeOM.Services;

namespace DeluxeOM.Announcement.BulkUploader
{
    public class AnnouncementBulkInsertTask : ITask 
    {
        IBulkUploadService _service = null;
        string _fileName = string.Empty;
        public AnnouncementBulkInsertTask(string fileName)
        {
            _service = new BulkUploadService();
            _fileName = fileName;
        }
        public string Name
        {
            get
            {
                return "Announcement Bulk Insert Task";
            }
        }
        private dlxTaskResult _taskResult { get; set; }
        public dlxTaskResult PreviousTaskResult
        {
            get; set;
        }

        public void Execute(int id )
        {
            JobItem jobItem = new JobItem();
            jobItem.StartDate = DateTime.UtcNow;
            jobItem.JobId = id;
            jobItem.TaskName = Name;
            try
            {
                var taskresult = this.PreviousTaskResult.TaskData;
                var ftpConfig = _service.GetFtpConfig(FtpConfigurationType.Announcement);
                string filePath = string.IsNullOrEmpty(_fileName) ? taskresult.FilePath
                   : string.Format("{0}{1}/{2}", ftpConfig.DownloadLocalDirectory, ftpConfig.FtpDirecrory, _fileName);

                string fileName = filePath.Substring(filePath.LastIndexOf("/") + 1);
                if (_service.IsFileExists(fileName))
                {
                    throw new Exception(string.Format("File {0} is already processed. You can not process same file again.", fileName ));
                }
                int recordsImported = _service.PopulateAnnouncement(filePath);
                //taskresult.NoOfRecordImported = recordsImported;

                _taskResult = new dlxTaskResult()
                {
                    IsSuccess = true,
                    //TaskData = new dlxTaskData() { FilePath = taskresult.FilePath }
                    TaskData = string.IsNullOrEmpty(_fileName) ? taskresult : new dlxTaskData { NoOfRecordImported = recordsImported, FilePath = filePath }
                };
                jobItem.Status = true;
                jobItem.Description = "Announcement Bulk Insert Successful";
                jobItem.EndDate = DateTime.UtcNow;
                _service.JobItemLog(jobItem);
            }
            catch (Exception ex)
            {
                //Console.Write(ex.Message);
                _taskResult = new dlxTaskResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Announcement Bulk Insert Failed."
                };
                jobItem.Status = false;
                //jobItem.Description = _taskResult.ErrorMessage;
                jobItem.Description = string.Format("Error: {0} Inner Exception: {1} Error Message: {2}", _taskResult.ErrorMessage, ex.InnerException == null ? "" : ex.InnerException.ToString(), ex.Message);
                jobItem.EndDate = DateTime.UtcNow;
                _service.JobItemLog(jobItem);
            }
        }

        public dlxTaskResult GetTaskResult()
        {
            return _taskResult;
        }
        private dlxTaskResult Process()
        {
            //code to call SP
            return new dlxTaskResult();
        }
    }
}

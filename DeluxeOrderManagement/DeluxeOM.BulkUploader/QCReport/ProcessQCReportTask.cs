using System;
using DeluxeOM.Models.BulkUploader;
using DeluxeOM.Services;
using DeluxeOM.BulkUploader;

namespace DeluxeOM.QC.BulkUploader
{
    public class ProcessQCReportTask : ITask
    {
        IBulkUploadService _searvice = null;
        private dlxTaskResult _taskResult { get; set; }
        public ProcessQCReportTask()
        {
            _searvice = new BulkUploadService();
        }
        public string Name
        {
            get
            {
                return "Process QCReport Task";
            }
        }

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

                _searvice.InsertQCReport();
                _taskResult = new dlxTaskResult()
                {
                    IsSuccess = true,
                    TaskData = taskresult
                };
                jobItem.Status = true;
                jobItem.Description = "Process QCReport Successful";
                jobItem.EndDate = DateTime.UtcNow;
                _searvice.JobItemLog(jobItem);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _taskResult = new dlxTaskResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Process QCReport Task Failed"
                };
                jobItem.Status = false;
                jobItem.Description = string.Format("Error: {0} Inner Exception: {1} Error Message: {2}", _taskResult.ErrorMessage, ex.InnerException == null ? "" : ex.InnerException.ToString(), ex.Message);
                jobItem.EndDate = DateTime.UtcNow;
                _searvice.JobItemLog(jobItem);

            }
        }

        public dlxTaskResult GetTaskResult()
        {
            return _taskResult;
        }
    }
}

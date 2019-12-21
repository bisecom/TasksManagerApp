using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Data
{
    public enum StatusTask { NotConfirmed, Active, Processing, Executed };
    public class Task
    {
        public int Id { get; set; }
        public string TaskSubject { get; set; }
        public string TaskDescription { get; set; }
        public string ExecuterName { get; set; }
        public string ManagerName { get; set; }
        public DateTime TaskDateTimeStart { get; set; }
        public DateTime TaskDateTimeEnd { get; set; }
        public StatusTask status { get; set; }
        
        public Task()
        {
            TaskDateTimeStart = new DateTime();
            TaskDateTimeEnd = new DateTime();
        }


    }
}

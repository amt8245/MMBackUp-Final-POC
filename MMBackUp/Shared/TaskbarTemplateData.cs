using Smart.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
//using BlazorDemos;

namespace MMBackUp.Shared
{
    public class TaskbarTemplateData
    {
        public class TaskData
        {
            public int TaskId { get; set; }
            public string TaskName { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string Duration { get; set; }
            public int? Progress { get; set; }
            public string? Predecessor { get; set; }
            public int? ParentId { get; set; }

            public string? Tail { get; set; }
            public List<TaskData> SubTasks { get; set; }
        }

        public class TaskProperties
        {
            public string TaskName { get; set; }
            public double Duration { get; set; }
        }
        public class TaskbarData : TaskData
        {
            public string Performance { get; set; }
            public string Winner { get; set; }
            public string Movie { get; set; }
        }


        public static List<TaskData> DataCollection(List<Shared.FlightInfo> flights)
        {
            //Takes List of FlightInfo and Converts into List of TaskData for Gantt Displat

            List<TaskData> TaskDataCollection = new List<TaskData>();
            Dictionary<string, TaskData> tailsTask = new Dictionary<string, TaskData>();    //Uses Dictionary to group my Flight Tails for optimization
            List<string> tails = new List<string>();
            int count = 0;
            foreach (Shared.FlightInfo flight in flights)
            {
                if (!tailsTask.ContainsKey(flight.acreg))
                {
                    TaskData head = new TaskData()
                    {
                        //TaskId = (int)(flight.flt_id),
                        TaskId = count,
                        TaskName = flight.acreg,
                        StartDate = flight.STD,
                        EndDate = flight.STA,        //do not question why they are flipped it just needs to be
                        Tail = flight.acreg,
                        SubTasks = new List<TaskData>()
                    };
                    count++;
                    TaskData temp = new TaskData()
                    {
                        TaskId = (int)(flight.flt_id),
                        TaskName = flight.key_p,
                        StartDate = flight.STD,
                        EndDate = flight.STA,        //do not question why they are flipped it just needs to be
                        Tail = flight.acreg,
                    };
                    head.SubTasks.Add(temp);
                    tailsTask.Add(flight.acreg, head);
                }
                else {
                    TaskData temp = new TaskData()
                    {
                        //TaskId = Int32.Parse(flight.flt_nbr),
                        TaskId = (int)(flight.flt_id),
                        TaskName = flight.key_p,
                        StartDate = flight.STD,
                        EndDate = flight.STA,        //do not question why they are flipped it just needs to be
                        Tail = flight.acreg
                    };

                    //Assign Parent ID to previous Flight added to Flight's Subtask List
                    if (tailsTask[flight.acreg].SubTasks.Count() != 0) { temp.ParentId = (tailsTask[flight.acreg].SubTasks[tailsTask[flight.acreg].SubTasks.Count() - 1].TaskId); }
                    else { temp.ParentId = tailsTask[flight.acreg].TaskId; }
                    tailsTask[flight.acreg].SubTasks.Add(temp);
                }
            }

            //Iterate over Dictionary, sort Flights in each Tail by Date, and add to List of Flight Tasks
            foreach (var item in tailsTask.Values) {
                item.SubTasks = item.SubTasks.OrderBy(x => x.StartDate).ToList();
                item.StartDate = item.SubTasks[0].StartDate;
                item.EndDate = item.SubTasks[item.SubTasks.Count()-1].EndDate;
                TaskDataCollection.Add(item);
            }
            TaskDataCollection = TaskDataCollection.OrderBy(x => x.StartDate).ToList();
            return TaskDataCollection;
        }

        public static List<DateTime> getDates(int? days) {
            //Not the best location, but returns date range for Gantt View
            if (days == null) { days = 8; }
            List<DateTime> dates = new List<DateTime>();
            dates.Add(DateTime.Today);
            dates.Add(DateTime.Today.AddDays((double)days+1));
            return dates;
        }
    }
}

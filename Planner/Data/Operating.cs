using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Planner.Data.Task;

namespace Planner.Data
{
    static class Operating
    {

        public static List<Task> mytasks;
        public static List<TasksLayout> myLayout;
        public static string startTask;
        public static string endTask;

        static Operating()
        {
            mytasks = new List<Task>();
            //mytasks = new List<Task>{new Task{TaskSubject = "Report", TaskDescription = "It should be fulfilled by Friday", ExecuterName = "Petrov",
            //        ManagerName = "Sidorov", TaskDateTimeStart = new DateTime(2019, 04, 02, 08, 30, 0), TaskDateTimeEnd = new DateTime(2019, 04, 02, 12, 30, 0),
            //        status = StatusTask.Executed},
            //        new Task{TaskSubject = "Offer", TaskDescription = "It should be sent out by Monday", ExecuterName = "Petrov",
            //        ManagerName = "Sidorov", TaskDateTimeStart = new DateTime(2019, 04, 05, 15, 30, 0), TaskDateTimeEnd = new DateTime(2019, 04, 05, 17, 00, 0),
            //        status = StatusTask.NotConfirmed}};
            myLayout = new List<TasksLayout>();
            GetDataFromDB();
        }

        public static string GetDbConnection()
        {
            var settings = System.Configuration.ConfigurationManager.ConnectionStrings["MyPlanner"].ConnectionString;
            return settings;
        }

        public static int AddTask(Task obj)
        {
            using (SQLiteConnection connection = new SQLiteConnection(GetDbConnection()))
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(connection);
                string startTask = "";
                string endTask = "";

                startTask = obj.TaskDateTimeStart.ToString();
                endTask = obj.TaskDateTimeEnd.ToString();
                cmd.CommandText = $"INSERT INTO myTasksTable(TaskSubject, TaskDescription, ExecuterName, ManagerName, TaskDateTimeStart, TaskDateTimeEnd, status) VALUES ('{obj.TaskSubject}', '{obj.TaskDescription}', '{obj.ExecuterName}', '{obj.ManagerName}', '{startTask}', '{endTask}', '{obj.status}')";
                cmd.Prepare();
                cmd.ExecuteNonQuery();

                cmd.CommandText = $"SELECT Id FROM myTasksTable WHERE TaskDescription = '{obj.TaskDescription}';";
                cmd.Prepare();
                int index = Convert.ToInt32(cmd.ExecuteScalar());
                return index;

            }
        }

        public static List<Task> GetDataFromDB()
        {
            mytasks = new List<Task>();
            using (SQLiteConnection connection = new SQLiteConnection(GetDbConnection()))
            {
                const string query = @"SELECT s.Id, s.TaskSubject, s.TaskDescription, s.ExecuterName, s.ManagerName, s.TaskDateTimeStart, s.TaskDateTimeEnd, s.status FROM myTasksTable s";
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    mytasks = new List<Task>();
                    foreach (DbDataRecord row in reader)
                    {
                        startTask = row["TaskDateTimeStart"].ToString();
                        endTask = row["TaskDateTimeEnd"].ToString();

                        if (startTask[11] == '6') { startTask = startTask.Insert(11, "0"); }
                        if (startTask[11] == '7') { startTask = startTask.Insert(11, "0"); }
                        if (startTask[11] == '8') { startTask = startTask.Insert(11, "0"); }
                        if (startTask[11] == '9') { startTask = startTask.Insert(11, "0"); }
                        if (endTask[11] == '6') { endTask = endTask.Insert(11, "0"); }
                        if (endTask[11] == '7') { endTask = endTask.Insert(11, "0"); }
                        if (endTask[11] == '8') { endTask = endTask.Insert(11, "0"); }
                        if (endTask[11] == '9') { endTask = endTask.Insert(11, "0"); }

                        mytasks.Add(new Task()
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            TaskSubject = row["TaskSubject"].ToString(),
                            TaskDescription = row["TaskDescription"].ToString(),
                            ExecuterName = row["ExecuterName"].ToString(),
                            ManagerName = row["ManagerName"].ToString(),
                            TaskDateTimeStart = DateTime.ParseExact(startTask, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                            TaskDateTimeEnd = DateTime.ParseExact(endTask, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                            status = (StatusTask)Enum.Parse(typeof(StatusTask), row["status"].ToString())
                        });
                    }
                }
                reader.Close();
            }
            
            return mytasks;
        }

        public static void EditDataOfDB(Task obj)
        {
            startTask = obj.TaskDateTimeStart.ToString();
            endTask = obj.TaskDateTimeEnd.ToString();
            string query = $"UPDATE myTasksTable SET TaskSubject = '{obj.TaskSubject}', TaskDescription = '{obj.TaskDescription}', ExecuterName = '{obj.ExecuterName}', ManagerName = '{obj.ManagerName}', TaskDateTimeStart = '{startTask}', TaskDateTimeEnd = '{endTask}', status = '{obj.status}' WHERE Id = {obj.Id}";
            using (SQLiteConnection connection = new SQLiteConnection(GetDbConnection()))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
            }

        }

        public static void DeleteLineFromDB(int index)
        {
            string query = $"DELETE FROM myTasksTable WHERE Id = {index}";
            using (SQLiteConnection connection = new SQLiteConnection(GetDbConnection()))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
            }

        }

    }
}

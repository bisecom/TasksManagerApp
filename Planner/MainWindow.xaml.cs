using Planner.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace Planner
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DateTime today = DateTime.Today;
            DetermineCalendarHeader(today);
            FillCalendar(today, Operating.mytasks);
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedDate = new DateTime();
            selectedDate = (DateTime)calendar.SelectedDate;
            DetermineCalendarHeader(selectedDate);
            FillCalendar(selectedDate, Operating.mytasks);
        }

        private void TB_MouseLeftButtonDown(object sender, EventArgs e)
        {
            TextBlock tb = new TextBlock();
            tb = sender as TextBlock;

            int rowIndex = DetermineRowColumn(sender)[0];
            int columnIndex = DetermineRowColumn(sender)[1];
            int tempTaskIndex = 0;
            foreach (var elem in Operating.myLayout)
            {
                if (elem.TaskColumn == columnIndex && elem.TaskRow == rowIndex)
                {
                    tempTaskIndex = elem.TaskListIndex;
                }
            }
            tbTaskTimeDisplay.Text = String.Empty;
            tbTaskResponsDisplay.Text = String.Empty;
            tbTaskSubjDisplay.Text = String.Empty;
            tbTaskDescrDisplay.Text = String.Empty;

            tbTaskTimeDisplay.Text = Operating.mytasks[tempTaskIndex].TaskDateTimeStart.ToString() + "\n" +
                Operating.mytasks[tempTaskIndex].TaskDateTimeEnd.ToString();
            tbTaskResponsDisplay.Text = "Manager: " + Operating.mytasks[tempTaskIndex].ManagerName + "\n" +
                "Executer: " + Operating.mytasks[tempTaskIndex].ExecuterName + "\n" +
                "Status: " + Operating.mytasks[tempTaskIndex].status;
            tbTaskSubjDisplay.Text = Operating.mytasks[tempTaskIndex].TaskSubject;
            tbTaskDescrDisplay.Text = Operating.mytasks[tempTaskIndex].TaskDescription;
        }

        private void MenuItemCreate_Click(object sender, RoutedEventArgs e)
        {
            TaskWindow mytaskWnd = new TaskWindow();
            if (mytaskWnd.ShowDialog() == true)
            {
                Data.Task myTaskObj = new Data.Task();
                if (mytaskWnd.tbSubject.Text == string.Empty || mytaskWnd.tbDescription.Text == String.Empty /*|| mytaskWnd.datePicker.HasValue == false*/)
                {
                    MessageBox.Show("Please, enter all data!");
                }
                else
                {
                    string timeDateTmp = "";
                    myTaskObj.TaskSubject = mytaskWnd.tbSubject.Text;
                    myTaskObj.TaskDescription = mytaskWnd.tbDescription.Text;
                    myTaskObj.ExecuterName = mytaskWnd.tbResponsible.Text;
                    myTaskObj.ManagerName = mytaskWnd.tbManager.Text;
                    timeDateTmp = mytaskWnd.datePicker.SelectedDate.Value.Date.ToShortDateString();
                    timeDateTmp = timeDateTmp + " " + mytaskWnd.cbStartTime.SelectedItem.ToString();//yyyy-MM-dd
                    myTaskObj.TaskDateTimeStart = DateTime.ParseExact(timeDateTmp, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
                    timeDateTmp = "";
                    timeDateTmp = mytaskWnd.datePicker.SelectedDate.Value.Date.ToShortDateString();
                    timeDateTmp = timeDateTmp + " " + mytaskWnd.cbEndTime.SelectedItem.ToString();
                    myTaskObj.TaskDateTimeEnd = DateTime.ParseExact(timeDateTmp, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
                    myTaskObj.status = (StatusTask)Enum.Parse(typeof(StatusTask), mytaskWnd.cbTaskStatus.Text);
                    Operating.mytasks.Add(myTaskObj);
                    FillCalendar(myTaskObj.TaskDateTimeEnd.Date, Operating.mytasks);
                    Operating.mytasks[Operating.mytasks.Count - 1].Id = Operating.AddTask(myTaskObj);
                    MessageBox.Show("Thank you! \nThe Task is saved succesfully!");
                }
                          
            }
            else
            {
                MessageBox.Show("Sorry.. \nThe Task is not saved!");
            }

        }

        private void MenuItemAmendDelete_Click(object sender, RoutedEventArgs e)
        {
            var myTaskWind = new TaskWindow("Delete", "Amend");
            int rowIndex = DetermineRowColumn(sender)[0];
            int columnIndex = DetermineRowColumn(sender)[1];
            int tempTaskIndex = 0;
            foreach (var elem in Operating.myLayout)
            {
                if (elem.TaskColumn == columnIndex && elem.TaskRow == rowIndex)
                {
                    tempTaskIndex = elem.TaskListIndex;
                }
            }

            myTaskWind.tbSubject.Text = Operating.mytasks[tempTaskIndex].TaskSubject;
            myTaskWind.tbDescription.Text = Operating.mytasks[tempTaskIndex].TaskDescription;
            myTaskWind.tbResponsible.Text = Operating.mytasks[tempTaskIndex].ExecuterName;
            myTaskWind.tbManager.Text = Operating.mytasks[tempTaskIndex].ManagerName;
            myTaskWind.datePicker.SelectedDate = Operating.mytasks[tempTaskIndex].TaskDateTimeStart.Date;
            myTaskWind.cbStartTime.SelectedValue = Operating.mytasks[tempTaskIndex].TaskDateTimeStart.ToString("HH:mm");//yyyy-MM-dd
            myTaskWind.cbEndTime.SelectedValue = Operating.mytasks[tempTaskIndex].TaskDateTimeEnd.ToString("HH:mm");
            myTaskWind.cbTaskStatus.SelectedValue = Operating.mytasks[tempTaskIndex].status;

            if (myTaskWind.ShowDialog() == true)
            {
             
                Data.Task myTaskObj = new Data.Task();

                if (myTaskWind.tbSubject.Text == string.Empty || myTaskWind.tbDescription.Text == String.Empty /*|| mytaskWnd.datePicker.HasValue == false*/)
                {
                    MessageBox.Show("Please, enter all data!");
                }
                else
                {
                    string timeDateTmp = "";
                    myTaskObj.TaskSubject = myTaskWind.tbSubject.Text;
                    myTaskObj.TaskDescription = myTaskWind.tbDescription.Text;
                    myTaskObj.ExecuterName = myTaskWind.tbResponsible.Text;
                    myTaskObj.ManagerName = myTaskWind.tbManager.Text;
                    timeDateTmp = myTaskWind.datePicker.SelectedDate.Value.Date.ToShortDateString();
                    timeDateTmp = timeDateTmp + " " + myTaskWind.cbStartTime.SelectedItem.ToString();//yyyy-MM-dd
                    myTaskObj.TaskDateTimeStart = DateTime.ParseExact(timeDateTmp, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
                    timeDateTmp = "";
                    timeDateTmp = myTaskWind.datePicker.SelectedDate.Value.Date.ToShortDateString();
                    timeDateTmp = timeDateTmp + " " + myTaskWind.cbEndTime.SelectedItem.ToString();
                    myTaskObj.TaskDateTimeEnd = DateTime.ParseExact(timeDateTmp, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
                    myTaskObj.status = (StatusTask)Enum.Parse(typeof(StatusTask), myTaskWind.cbTaskStatus.Text);
                    Operating.mytasks.RemoveAt(tempTaskIndex);
                    Operating.mytasks.Insert(tempTaskIndex, myTaskObj);
                    FillCalendar(Operating.mytasks[tempTaskIndex].TaskDateTimeStart.Date, Operating.mytasks);
                    Operating.EditDataOfDB(myTaskObj);
                    MessageBox.Show("Thank you! \nThe Task is amended succesfully!");
                }
            }
            else
            {
                Operating.DeleteLineFromDB(Operating.mytasks[tempTaskIndex].Id);
                Operating.mytasks.RemoveAt(tempTaskIndex);
                FillCalendar((DateTime)myTaskWind.datePicker.SelectedDate, Operating.mytasks);
                MessageBox.Show("Thank you! \nThe Task is deleted succesfully!");
            }
        }

        private void DetermineCalendarHeader(DateTime objDate)
        {
            int day = ((int)objDate.DayOfWeek == 0) ? 7 : (int)objDate.DayOfWeek;
            objDate = objDate.AddDays(1 - day);
            objDate = objDate.Date;
            for (int i = 0; i < 7; i++)
            {
                Label dynamicLabel = new Label();
                dynamicLabel.Content = objDate.AddDays(i).ToShortDateString();
                dynamicLabel.Foreground = new SolidColorBrush(Colors.White);
                dynamicLabel.Background = new SolidColorBrush(Color.FromRgb(102, 153, 255));
                dynamicLabel.FontWeight = FontWeights.Bold;
                Grid.SetColumn(dynamicLabel, i);
                taskHeaderGrid.Children.Add(dynamicLabel);
            }
        }

        private void FillCalendar(DateTime objDate, List<Data.Task> tasksList)
        {
            int startPlanTime = 6;
            int day = ((int)objDate.DayOfWeek == 0) ? 7 : (int)objDate.DayOfWeek;
            objDate = objDate.AddDays(1 - day);
            Operating.myLayout.Clear();//clean temp List of visible tasks

            //foreach (TextBlock tb in FindVisualChildren<TextBlock>(internalGrid))
            //{
            //    if (tb is TextBlock /*&& tb.Name.Contains("TaskTB")*/)
            //    {
            //        internalGrid.Children.Remove(tb);
            //    }
            //}
            internalGrid.Children.Clear();

            for (int i = 0; i < 34; i += 2)
            {
                for (int j = 0; j < 7; j++)
                {
                    TextBlock dynamTB = new TextBlock();
                    dynamTB.Text = (startPlanTime + i / 2).ToString() + "-00";
                    dynamTB.Foreground = new SolidColorBrush(Color.FromRgb(0, 60, 179));
                    dynamTB.Background = new SolidColorBrush(Color.FromRgb(102, 153, 255));
                    dynamTB.TextAlignment = TextAlignment.Left;
                    dynamTB.FontSize = 10;
                    Grid.SetRow(dynamTB, i);
                    Grid.SetColumn(dynamTB, j);
                    internalGrid.Children.Add(dynamTB);
                }

            }
            if (tasksList != null)
            {
                for (int i = 0; i < 7; i++)
                {
                    int counter = 0;
                    foreach (var elem in tasksList)
                    {
                        if (elem.TaskDateTimeStart.Date == objDate.AddDays(i).Date)
                        {
                            TimeSpan t = elem.TaskDateTimeEnd - elem.TaskDateTimeStart;
                            double NrOfHours = t.TotalHours;
                            int rowNmr_ = (elem.TaskDateTimeStart.AddHours(-6)).Hour; //determine row position for textbox
                            if ((elem.TaskDateTimeStart.AddHours(-6)).Minute != 0)
                                rowNmr_ += 1;
                            TasksLayout temp = new TasksLayout();

                            TextBlock dynTaskTB = new TextBlock();
                            dynTaskTB.Text = elem.TaskSubject;
                            dynTaskTB.Name = "TaskTB" + counter.ToString();
                            dynTaskTB.TextWrapping = TextWrapping.Wrap;
                            dynTaskTB.Foreground = new SolidColorBrush(Colors.Blue);
                            dynTaskTB.Background = new SolidColorBrush(Color.FromRgb(255, 153, 102));
                            dynTaskTB.FontWeight = FontWeights.Bold;
                            dynTaskTB.MouseLeftButtonDown += new MouseButtonEventHandler(TB_MouseLeftButtonDown);
                            dynTaskTB.MouseRightButtonDown += new MouseButtonEventHandler(MenuItemAmendDelete_Click);

                            dynTaskTB.FontSize = 10;
                            Grid.SetRow(dynTaskTB, rowNmr_);
                            Grid.SetColumn(dynTaskTB, i);
                            Grid.SetRowSpan(dynTaskTB, Convert.ToInt32(NrOfHours * 10 / 5));
                            temp.TaskRow = rowNmr_; temp.TaskColumn = i; temp.TaskListIndex = counter;
                            Operating.myLayout.Add(temp);
                            internalGrid.Children.Add(dynTaskTB);
                        }
                        counter++;
                    }
                }
            }
        }


        public IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }
                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private List<int> DetermineRowColumn(object sender)//determinging click row and column indexes
        {
            var result = new List<int>(2);
            TextBlock mytb = new TextBlock();
            mytb = sender as TextBlock;
            int _row = (int)mytb.GetValue(Grid.RowProperty);
            int _column = (int)mytb.GetValue(Grid.ColumnProperty);
            result.Add(_row);
            result.Add(_column);
            return result;
        }

    }
}

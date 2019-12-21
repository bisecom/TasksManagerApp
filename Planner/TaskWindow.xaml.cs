using Planner.Data;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Planner
{
    /// <summary>
    /// Логика взаимодействия для TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        public TaskWindow()
        {
            InitializeComponent();
            FillingCB();
        }

        public TaskWindow(string strDel, string strAmend)
        {
            InitializeComponent();
            FillingCB();
            CreateDynButton(strDel, strAmend);
        }

        private void OkButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void FillingCB()
        {
            for (int i = 6; i < 23; i++)
            {
                if (i < 10)
                {
                    cbStartTime.Items.Add("0" + i.ToString() + ":00");
                    cbStartTime.Items.Add("0" + i.ToString() + ":30");
                    cbEndTime.Items.Add("0" + i.ToString() + ":00");
                    cbEndTime.Items.Add("0" + i.ToString() + ":30");
                }
                else
                {
                    cbStartTime.Items.Add(i.ToString() + ":00");
                    cbStartTime.Items.Add(i.ToString() + ":30");
                    cbEndTime.Items.Add(i.ToString() + ":00");
                    cbEndTime.Items.Add(i.ToString() + ":30");
                }
            }

            foreach (var item in Enum.GetValues(typeof(StatusTask)))
            {
                cbTaskStatus.Items.Add(item);
            }
        }

        private void CreateDynButton(string _delete, string _amend)
        {
            this.acceptButton.Content = _amend;
            this.cancelButton.Content = _delete;
        }

    }
}

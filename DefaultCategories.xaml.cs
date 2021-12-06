using ImageScreeningSystemHeaderFooter;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ImageScreeningSystem
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class DefaultCategories : Window
    {
        List<string> lines;
        string path = @"C:\ImageScreeningSystem\DefectList.txt";
        StreamWriter sw;

        public DefaultCategories()
        {
            InitializeComponent();

            lines = File.ReadAllLines(path).ToList();
            foreach(string name in lines)
            {
                listBox.Items.Add(name);
            }
        }

        private void addBtnClicked(object sender, RoutedEventArgs e)
        {
            bool repeat = false;
            if (categoryName.Text == "")
            {
                MessageBox.Show("Please enter default defect category name into the textbox");
            }
            else
            {
                foreach (string name in lines)
                {
                    if (categoryName.Text == name)
                    {
                        repeat = true;
                    }
                }
                if (repeat)
                {
                    MessageBox.Show("\" "+categoryName.Text + " \" default defect category already exist");
                }
                else
                {
                    sw = new StreamWriter(path);
                    listBox.Items.Add(categoryName.Text);
                    foreach (string item in listBox.Items)
                    {
                        sw.WriteLine(item);
                    }
                    sw.Close();
                    listBox.Items.Refresh();
                    categoryName.Text = "";
                }
            }
        }

        private void deleteBtnClicked(object sender, RoutedEventArgs e)
        {
            listBox.Items.RemoveAt(listBox.SelectedIndex);
            listBox.UnselectAll();
            listBox.Items.Refresh();

            sw = new StreamWriter(path);
            foreach (string item in listBox.Items)
            {
                sw.WriteLine(item);
            }
            sw.Close();

            deleteBtn.IsEnabled = false;
        }

        private void folderSelected(object sender, SelectionChangedEventArgs e)
        {
            deleteBtn.IsEnabled = true;
        }

        private void closeBtnClicked(object sender, RoutedEventArgs e)
        {
            SessionSelect sl = new SessionSelect();
            sl.Show();
            this.Close();
        }
    }
}

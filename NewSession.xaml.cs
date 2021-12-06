using ImageScreeningSystemHeaderFooter;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace ImageScreeningSystem
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class NewSession : Window
    {
        List<string> lines;
        string path = @"C:\ImageScreeningSystem\DefectList.txt";
        public NewSession()
        {
            InitializeComponent();

            lines = File.ReadAllLines(path).ToList();
            foreach (string name in lines)
            {
                listBox.Items.Add(name);
            }
        }

        private void deleteBtnClicked(object sender, RoutedEventArgs e)
        {
            listBox.Items.RemoveAt(listBox.SelectedIndex);
            listBox.UnselectAll();
            listBox.Items.Refresh();
            deleteBtn.IsEnabled = false;
        }

        private void addBtnClicked(object sender, RoutedEventArgs e)
        {
            bool repeat = false;
            if (categoryName.Text == "")
            {
                MessageBox.Show("Please enter session defect category name into the textbox");
            }
            else
            {
                foreach (string name in listBox.Items)
                {
                    if (categoryName.Text == name)
                    {
                        repeat = true;
                    }
                }
                if (repeat)
                {
                    MessageBox.Show("\" " + categoryName.Text + " \" session defect category already exist");
                }
                else
                {
                    listBox.Items.Add(categoryName.Text);
                    listBox.Items.Refresh();
                    categoryName.Text = "";
                }
            }
        }
        private void createBtnClicked(object sender, RoutedEventArgs e)
        {
            bool gotDuplicate = false;
            if (sessionName.Text == "")
            {
                MessageBox.Show("Please enter session name into the textbox");
            }
            else
            {
                string session = sessionName.Text;
                string directory = "C:\\ImageScreeningSystem\\";
                DirectoryInfo[] validation = new DirectoryInfo(directory).GetDirectories();

                foreach (DirectoryInfo folder in validation)
                {
                    if (session == folder.Name)
                    {
                        gotDuplicate = true;
                    }
                }
                if (gotDuplicate)
                {
                    MessageBox.Show("\" " + session + " \" session already exist");
                }
                else
                {
                    string newPath = "C:\\ImageScreeningSystem\\" + session;
                    if (!System.IO.Directory.Exists(newPath))
                    {
                        System.IO.Directory.CreateDirectory(newPath);
                        string newpassFolder = newPath + "\\Pass";
                        string newfailFolder = newPath + "\\Fail";
                        string newunprogressFolder = newPath + "\\Unprogress";
                        string newUploadedFolder = newPath + "\\Uploaded";
                        System.IO.Directory.CreateDirectory(newpassFolder);
                        System.IO.Directory.CreateDirectory(newfailFolder);
                        System.IO.Directory.CreateDirectory(newunprogressFolder);
                        System.IO.Directory.CreateDirectory(newUploadedFolder);

                        foreach (string failFolderPath in listBox.Items)
                        {
                            string filePath = newfailFolder + "\\" + failFolderPath;
                            string txtFile = newfailFolder + "\\" + failFolderPath + "\\" + failFolderPath + ".txt"; 
                            System.IO.Directory.CreateDirectory(filePath);
                            File.Create(txtFile);
                        }

                    }

                    MainWindow mw = new MainWindow(session);
                    mw.Show();
                    this.Close();
                }
            }
        }

        private void categorySelected(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
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

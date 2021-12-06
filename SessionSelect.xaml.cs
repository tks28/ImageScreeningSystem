using ImageScreeningSystem;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ImageScreeningSystemHeaderFooter
{
    /// <summary>
    /// Interaction logic for SessionSelect.xaml
    /// </summary>
    public partial class SessionSelect : Window
    {
        string MainPath;

        public SessionSelect()
        {
            InitializeComponent();
            MainPath = @"C:\ImageScreeningSystem";

            //Create main folder and text file if does not exist
            if (!System.IO.Directory.Exists(MainPath))
            {
                System.IO.Directory.CreateDirectory(MainPath);
                System.IO.File.Create(@"C:\ImageScreeningSystem\DefectList.txt");
            }
            loadSessionName();
        }

        private void loadSessionName()
        {
            SessionList.Items.Clear();

            string a = " ";
            DirectoryInfo dir = new DirectoryInfo(MainPath);
            DirectoryInfo[] arrfolders = dir.GetDirectories();
            foreach (DirectoryInfo folder in arrfolders)
            {
                string sFolderName = folder.Name;
                SessionList.Items.Add(sFolderName);
                a = a + sFolderName + "$$";
            }
        }

        private void deleteBtnClick(object sender, RoutedEventArgs e)
        {
            string mainDir = System.IO.Path.Combine(MainPath, SessionList.SelectedValue.ToString());

            //Delete image at listbox
            SessionList.Items.RemoveAt(SessionList.SelectedIndex);
            Directory.Delete(mainDir, true);
            SessionList.UnselectAll();
            SessionList.Items.Refresh();
            MessageBox.Show("Session deleted succesfully");

            deleteBtn.IsEnabled = false;
            resumeBtn.IsEnabled = false;
            path.Visibility = Visibility.Hidden;
            date.Visibility = Visibility.Hidden;
            numFile.Visibility = Visibility.Hidden;
            progressFile.Visibility = Visibility.Hidden;
            pathAns.Visibility = Visibility.Hidden;
            dateAns.Visibility = Visibility.Hidden;
            numFileAns.Visibility = Visibility.Hidden;
            progressFileAns.Visibility = Visibility.Hidden;
        }

        private void resumeBtnClick(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow(SessionList.SelectedValue.ToString());
            mw.Show();
            this.Close();
        } 

        private void newBtnClick(object sender, RoutedEventArgs e)
        {
            NewSession ns = new NewSession();
            ns.Show();
            this.Close();
        }

        private void listBoxSelected(object sender, SelectionChangedEventArgs e)
        {
            path.Visibility= Visibility.Visible;
            date.Visibility = Visibility.Visible;
            date.Visibility = Visibility.Visible;
            numFile.Visibility = Visibility.Visible;
            progressFile.Visibility = Visibility.Visible;
            deleteBtn.IsEnabled = true;
            resumeBtn.IsEnabled = true;

            if (SessionList.SelectedIndex >= 0)
            {
                string mainDir = System.IO.Path.Combine(MainPath, SessionList.SelectedValue.ToString());

                //count num of files
                DirectoryInfo uploadFolder = new DirectoryInfo(System.IO.Path.Combine(mainDir, "Uploaded"));
                float uploadedPicsNum = uploadFolder.GetFiles().Length;
                numFileAns.Content = uploadedPicsNum;

                //minus num of files
                DirectoryInfo unprogressFolder = new DirectoryInfo(System.IO.Path.Combine(mainDir, "Unprogress"));
                float upprogressPicsNum = unprogressFolder.GetFiles().Length;
                float diff = uploadedPicsNum - upprogressPicsNum;

                float percentage =  (diff/uploadedPicsNum) *100  ;
                Double diffPercentage = Math.Round((Double)percentage, 2);
                if (Double.IsNaN(diffPercentage) || diffPercentage == null)
                {
                    diffPercentage = 0;
                }

                string totalProgress = diff.ToString() + "/" + uploadedPicsNum.ToString() +"(" + diffPercentage.ToString() + "%)";
                progressFileAns.Content = totalProgress;

                //get file path 
                DirectoryInfo dirPath = new DirectoryInfo(mainDir);
                pathAns.Content = dirPath.FullName;

                //get file date
                DateTime dt = Directory.GetCreationTime(mainDir);
                dateAns.Content = dt;
            }

            pathAns.Visibility = Visibility.Visible;
            dateAns.Visibility = Visibility.Visible;
            numFileAns.Visibility = Visibility.Visible;
            progressFileAns.Visibility = Visibility.Visible;
        }

        private void openDefaultCat(object sender, RoutedEventArgs e)
        {
            DefaultCategories dc = new DefaultCategories();
            dc.Show();
            this.Close();
        }
    }
} 
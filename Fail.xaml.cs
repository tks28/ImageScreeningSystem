using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Windows.Media;

namespace ImageScreeningSystemHeaderFooter
{

    public partial class Fail : Page
    {
        string fileName = "";
        int counter;
        int selectedindex = 0;
        List<FailImage> failImageList = new List<FailImage>();
        String currentCategory = "";
        string mainPath;
        string failFolder;
        string uploadFolder;
        string unprogress;
        string fullPath;
        class FailImage
        {
            public string failImagePath { get; set; }
            public string failImageName { get; set; }
        }

        public Fail(string Path)
        {
            InitializeComponent();
            mainPath = Path;
            failFolder = mainPath + @"\Fail\";
            uploadFolder = mainPath + @"\Uploaded\";
            unprogress = mainPath + @"\Unprogress\";
            DynamicComboBox();
        }

        private void Defect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            picData.Visibility = Visibility.Hidden;
            failImageList.Clear();

            counter = 0;

            if (DefectComboBox.SelectedItem != null)
            {
                currentCategory = SelectedFolder.Text.ToString();

                fullPath = failFolder + currentCategory;

                DirectoryInfo folder = new DirectoryInfo(fullPath);

                //Access all the folders in Fail Folder
                foreach (FileInfo fileInfo in folder.GetFiles())
                {
                    fileName = System.IO.Path.GetFileName(fileInfo.FullName);

                    if (!(fileName == currentCategory + ".txt"))
                    { 
                        failImageList.Add(new FailImage()
                        {
                            failImagePath = System.IO.Path.Combine(uploadFolder, fileName),
                            failImageName = fileName
                        });
                        counter++;
                    }
                }
                DefectListView.ItemsSource = failImageList;
                countNumFile.Content = "Counter: " + counter;
                DefectListView.Items.Refresh();
            }
        }

        private void DynamicComboBox()
        {

            DirectoryInfo[] Categories = (new DirectoryInfo(failFolder)).GetDirectories();
            foreach (DirectoryInfo folder in Categories)
            {
                DefectComboBox.Items.Add(addComboBox(folder.Name));
            }
        }

        private ComboBoxItem addComboBox(string name)
        {
            ComboBoxItem temp = new ComboBoxItem();
            temp.Content = name;
            temp.Height = 35;
            temp.FontFamily = new System.Windows.Media.FontFamily("Ubuntu");
            temp.FontSize = 25;
            temp.HorizontalContentAlignment = HorizontalAlignment.Center;
            return temp;
        }

        private void imageSelectedDelete(object sender, SelectionChangedEventArgs e)
        {
            string[] picName = new string[100];
            picData.Items.Clear();
            if (DefectListView.SelectedIndex > -1)
            {
                selectedindex = DefectListView.SelectedIndex;
                deleteBtn.IsEnabled = true;

                picData.Visibility = Visibility.Visible;
                string selectedPic = failImageList[selectedindex].failImageName;

                string[] line = File.ReadAllLines(fullPath + "\\" + currentCategory + ".txt");

                for (int i = 0; i < line.Length; i++)
                {
                    picName = line[i].Split(";");

                    if (picName[0] == selectedPic)
                    {
                        for (int u = 1; u < picName.Length -1; u++)
                        {
                            Label lb = new Label();
                            lb.Content = picName[u];
                            lb.BorderThickness = new Thickness(2);
                            lb.BorderBrush = Brushes.Black;
                            lb.Foreground = Brushes.Black;
                            lb.Width = 280;
                            lb.Height = 40;
                            lb.FontSize = 20;

                            picData.Items.Add(lb);

                        }
                    }
                }
            }
        }

        private void failDeleteBtnClicked(object sender, RoutedEventArgs e)
        {
            string[] newfile = new string[100];
            string[] picName = new string[100];
            int count = 0;
            picData.Visibility = Visibility.Hidden;
            string selectedPic = failImageList[selectedindex].failImageName;

            string[] line = File.ReadAllLines(fullPath + "\\" + currentCategory + ".txt");

            for(int i = 0; i < line.Length; i++) {
                picName = line[i].Split(";");

                if (picName[0] != selectedPic)
                {
                    newfile[count] = line[i];
                }
            }

            File.WriteAllLines(fullPath + "\\" + currentCategory + ".txt", newfile);

            //Delete Picture in Folder
            File.Copy(System.IO.Path.Combine(fullPath, failImageList[selectedindex].failImageName), System.IO.Path.Combine(unprogress, failImageList[selectedindex].failImageName));
            File.Delete(System.IO.Path.Combine(fullPath, failImageList[selectedindex].failImageName));

            //Delete image at listview
            foreach (FailImage removedItem in DefectListView.SelectedItems)
            {
                (DefectListView.ItemsSource as List<FailImage>).Remove(removedItem);
            }
            counter--;
            DefectListView.Items.Refresh();


            //Update counter
            countNumFile.Content = "Counter: " + counter;
            deleteBtn.IsEnabled = false;
        }

    }
}
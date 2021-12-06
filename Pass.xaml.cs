using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace ImageScreeningSystemHeaderFooter
{
    /// <summary>
    /// Interaction logic for PassImage.xaml
    /// </summary>
    public partial class Pass : Page
    {
        string fileName = "";
        int counter = 0;
        List<PassImages> passImageList = new List<PassImages>();
        int intselectedindex = 0;
        string mainPath;
        string passFolder;
        string uploadFolder;
        string unprogress;

        class PassImages
        {
            public string passImagePath { get; set; }
            public string passImageName { get; set; }
        }
        public Pass(string item)
        {
            InitializeComponent();

            mainPath = item;
            passFolder = mainPath + @"\Pass\";
            uploadFolder = mainPath + @"\Uploaded";
            unprogress = mainPath + @"\Unprogress";

            DirectoryInfo folder = new DirectoryInfo(passFolder);


            foreach (FileInfo fileInfo in folder.GetFiles())
            {
                fileName = System.IO.Path.GetFileName(fileInfo.FullName);
                passImageList.Add(new PassImages()
                {
                    passImagePath = System.IO.Path.Combine(uploadFolder, fileName),
                    passImageName = fileName
                }) ;
                counter++;
            }
            listView.ItemsSource = passImageList;
            countNumFile.Content = "Counter: " + counter;
        }

        private void imageSelectedDelete(object sender, SelectionChangedEventArgs e)
        {
            deleteBtn.IsEnabled = true;
            intselectedindex = listView.SelectedIndex;
        }

        private void deleteBtnClicked(object sender, RoutedEventArgs e)
        {
            //Delete Picture in Folder

            File.Copy(System.IO.Path.Combine(passFolder, passImageList[intselectedindex].passImageName), System.IO.Path.Combine(unprogress, passImageList[intselectedindex].passImageName));
            File.Delete(System.IO.Path.Combine(passFolder, passImageList[intselectedindex].passImageName));

            //Delete image at listview
            foreach (PassImages removedItem in listView.SelectedItems)
            {
                (listView.ItemsSource as List<PassImages>).Remove(removedItem);
            }
            counter--;
            listView.Items.Refresh();

            //Update counter
            countNumFile.Content = "Counter: " + counter;
            deleteBtn.IsEnabled = false;
        }
    }
}


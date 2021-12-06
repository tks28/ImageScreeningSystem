using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ImageScreeningSystemHeaderFooter
{
    /// <summary>
    /// Interaction logic for ManageDefectCategories.xaml
    /// </summary>
    public partial class ManageDefectCategories : Page
    {
        string failFolder;
        public ManageDefectCategories(string item)
        {
            InitializeComponent();
            failFolder = item;
            DirectoryInfo[] Categories = (new DirectoryInfo(failFolder)).GetDirectories();
            foreach (DirectoryInfo folder in Categories)
            {
                listBox.Items.Add(folder.Name);
            }
        }

        private void addBtnClicked(object sender, RoutedEventArgs e)
        {
            string fullDirectory = System.IO.Path.Combine(failFolder, categoryName.Text);

            //Verify if folder already exist
            if (categoryName.Text == "")
            {
                MessageBox.Show("Please enter session defect category name into the textbox");
            }


            else if (Directory.Exists(fullDirectory))
            {
                MessageBox.Show("\" " + categoryName.Text + " \" session defect category already exist");
            }

            else
            {
                System.IO.Directory.CreateDirectory(fullDirectory);
                string txtFile = fullDirectory +"\\" + categoryName.Text + ".txt";
                File.Create(txtFile);
                listBox.Items.Add(categoryName.Text);
                categoryName.Text = "";

                MessageBox.Show(categoryName.Text + " folder created");
            }
        }

        private void deleteBtnClicked(object sender, RoutedEventArgs e)
        {
            string tempName = listBox.SelectedItem.ToString();
            string fullDirectory = System.IO.Path.Combine(failFolder, listBox.SelectedItem.ToString());
            DirectoryInfo temp = new DirectoryInfo(fullDirectory);

            if (temp.GetFiles().Length < 2)
            {
                Directory.Delete(fullDirectory, true);
                listBox.Items.RemoveAt(listBox.SelectedIndex);
                deleteBtn.IsEnabled = false;
                //editBtn.IsEnabled = false;
                System.Windows.MessageBox.Show(tempName + " folder Deleted");
            }
            else
            {
                MessageBox.Show(tempName + " folder contain image");
            }
        }

        private void folderSelected(object sender, SelectionChangedEventArgs e)
        {
            deleteBtn.IsEnabled = true;
            //editBtn.IsEnabled = true;
        }
        /*private void editBtnClicked(object sender, RoutedEventArgs e)
        {
            string oldFullDirectory = System.IO.Path.Combine(failFolder, listBox.SelectedItem.ToString());

            if (Directory.EnumerateFileSystemEntries(oldFullDirectory).Any())
            {
                System.Windows.MessageBox.Show(listBox.SelectedItem.ToString() + " folder must be empty in order to rename");
            }

            else
            {
                renamePopOut.IsOpen = true;
                renamePopOutBorder.Visibility = Visibility.Visible;
            }
        }

        private void renamebtnClicked(object sender, RoutedEventArgs e)
        {
            string newFullDirectory = System.IO.Path.Combine(failFolder, newName.Text);
            if (newName.Text == "")
            {
                renamePopOut.IsOpen = false;
                renamePopOutBorder.Visibility = Visibility.Hidden;
                MessageBox.Show("Please enter a new session defect category name into the textbox");
            }


            else if (Directory.Exists(newFullDirectory))
            {
                renamePopOut.IsOpen = false;
                renamePopOutBorder.Visibility = Visibility.Hidden;
                MessageBox.Show("\" " + newName.Text + " \" session defect category already exist");
            }

            else
            {
                //Verify contents is empty only can rename
                string oldFullDirectory = System.IO.Path.Combine(failFolder, listBox.SelectedItem.ToString());
                Directory.Delete(oldFullDirectory);
                listBox.Items.RemoveAt(listBox.SelectedIndex);
                System.IO.Directory.CreateDirectory(newFullDirectory);
                listBox.Items.Add(newName.Text);

                newName.Text = "";
                renamePopOut.IsOpen = false;
                renamePopOutBorder.Visibility = Visibility.Hidden;
                System.Windows.MessageBox.Show("Session defect category Renamed");
                deleteBtn.IsEnabled = false;
                editBtn.IsEnabled = false;
            }
        }

        private void closeBtnClicked(object sender, RoutedEventArgs e)
        {
            newName.Text = "";
            renamePopOut.IsOpen = false;
            renamePopOutBorder.Visibility = Visibility.Hidden;
        }*/
    }
}

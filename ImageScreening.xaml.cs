using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageScreeningSystemHeaderFooter
{
    public partial class ImageScreening : Page
    {
        //Initialise variables
        string sessionName;
        string imageFileName = "";
        string passFolder;
        string failFolder;
        string unprogressFolder;
        string MainPath;
        string uploadFolder;
        string errorImageName = "";

        int selectedindex = 0;

        double uploadCounter;
        int unprogressCounter;
        int currProgressCounter;
        double progressPercentage;
        int remainImageCounter;
        double remainPercentage;
        int totalPassCounter;
        double passPercentage;
        int totalFailCounter;
        double failPercentage;

        string DefectSelected;
        int FailPicCount;
        double FailPicPercentage;


        //Create a list to store the image details
        List<UploadImage> uploadImageList = new List<UploadImage>();

        //The list created is based on this class
        class UploadImage
        {
            public string uploadImagePath { get; set; }
            public string uploadImageName { get; set; }
        }

        public ObservableCollection<cateName> CateList { get; set; }
        public class cateName
        {
            public string name { get; set; }
            public bool checking { get; set; }
        }

        public ImageScreening(string items)
        {
            InitializeComponent();
            MainPath = items;
            sessionName = System.IO.Path.GetFileName(MainPath);
            passFolder = MainPath + @"\Pass\";
            failFolder = MainPath + @"\Fail\";
            unprogressFolder = MainPath + @"\Unprogress\";
            uploadFolder = MainPath + @"\Uploaded";
            CateList = new ObservableCollection<cateName>();
            loadSession();
            DynamicCheckBox();
        }

        public void reload()
        {
            DirectoryInfo folder = new DirectoryInfo(unprogressFolder);
            string fileName;
            uploadImageList.Clear();

            DynamicCheckBox();

            foreach (FileInfo fileInfo in folder.GetFiles())
            {
                fileName = System.IO.Path.GetFileName(fileInfo.FullName);
                uploadImageList.Add(new UploadImage()
                {
                    uploadImagePath = System.IO.Path.Combine(uploadFolder, fileName),
                    uploadImageName = fileName
                });

                listBox.ItemsSource = uploadImageList;
                listBox.Items.Refresh();
            }

            if(listBox.Items.Count > 0)
            {
                leftBtn.IsEnabled = true;
                rightBtn.IsEnabled = true;
                passBtn.Visibility = Visibility.Visible;
                failBtn.Visibility = Visibility.Visible;

                listBox.SelectedIndex = 0;
                picHolder.Source = new BitmapImage(new Uri(uploadImageList[listBox.SelectedIndex].uploadImagePath.ToString()));
                picHolderName.Content = uploadImageList[listBox.SelectedIndex].uploadImageName.ToString();
                selectedindex = 0;
            }

            uploadCounter = Directory.GetFiles(uploadFolder).Length;

            unprogressCounter = Directory.GetFiles(unprogressFolder).Length;
            currProgressCounter = Convert.ToInt32(uploadCounter) - unprogressCounter;
            progressPercentage = (currProgressCounter / uploadCounter) * 100;

            remainImageCounter = Directory.GetFiles(unprogressFolder).Length;
            remainPercentage = (remainImageCounter / uploadCounter) * 100;

            totalPassCounter = Directory.GetFiles(passFolder).Length;
            passPercentage = (totalPassCounter / uploadCounter) * 100;

            totalFailCounter = Convert.ToInt32(uploadCounter) - unprogressCounter - totalPassCounter;
            failPercentage = (totalFailCounter / uploadCounter) * 100;

            currCounter.Content = currProgressCounter.ToString() + " / " + uploadCounter.ToString() + " (" + Math.Round((Double)progressPercentage, 2).ToString() + "%)";
            remainCounter.Content = remainImageCounter.ToString() + " / " + uploadCounter.ToString() + " (" + Math.Round((Double)remainPercentage, 2).ToString() + "%)";
            passCounter.Content = totalPassCounter + " (" + Math.Round((Double)passPercentage, 2).ToString() + "%)";
            failCounter.Content = totalFailCounter + " (" + Math.Round((Double)failPercentage, 2).ToString() + "%)";

            progressBar.Maximum = uploadCounter;
            progressBar.Minimum = 0;

            if (currProgressCounter > 0)
            {
                progressBar.Value = currProgressCounter;

                if (progressPercentage < 50)
                {
                    progressBar.Foreground = new SolidColorBrush(Colors.Red);
                }
                else if (progressPercentage < 80)
                {
                    progressBar.Foreground = new SolidColorBrush(Colors.Orange);
                }
                else
                {
                    progressBar.Foreground = new SolidColorBrush(Colors.LimeGreen);
                }
            }
            else
            {
                progressBar.Value = currProgressCounter;
                progressBar.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        //When load this page
        private void loadSession()
        {
            //Load default picture, session name
            picHolder.Source = new BitmapImage(new Uri(@"C:\INTI\Semester 2\Real World Project\EP\ImageScreeningSystemHeaderFooter\ImageScreeningSystemHeaderFooter\SystemImage\noImage.jpg"));
            sessionNameLabel.Content = "Session Name: " + sessionName;

            //unprogress folder not empty
            if (Directory.GetFiles(unprogressFolder).Length > 0)
            {
                string[] filePaths = Directory.GetFiles(unprogressFolder);

                //Load unprogress folder images to listbox
                foreach (string imagePath in filePaths)
                {
                    if (imagePath != null && imagePath != string.Empty)
                    {
                        //To get just the file name, not entire path
                        imageFileName = System.IO.Path.GetFileName(imagePath);
                        uploadImageList.Add(new UploadImage()
                        {
                            uploadImagePath = System.IO.Path.Combine(uploadFolder, imageFileName),
                            uploadImageName = imageFileName
                        });
                    }
                }

                listBox.ItemsSource = uploadImageList;

                //automatically load first image without needing to select
                listBox.SelectedIndex = 0;
                picHolder.Source = new BitmapImage(new Uri(uploadImageList[listBox.SelectedIndex].uploadImagePath.ToString()));
                picHolderName.Content = uploadImageList[listBox.SelectedIndex].uploadImageName.ToString();
                selectedindex = 0;

                //enable buttons
                leftBtn.IsEnabled = true;
                rightBtn.IsEnabled = true;
                passBtn.Visibility = Visibility.Visible;
                failBtn.Visibility = Visibility.Visible;
            }
            
            uploadCounter = Directory.GetFiles(uploadFolder).Length;

            if (uploadCounter == 0)
            {
                currentPro.Visibility = Visibility.Hidden;
                progressBar.Visibility = Visibility.Hidden;
                currBox.Visibility = Visibility.Hidden;
                currCounter.Visibility = Visibility.Hidden;
                RemainImage.Visibility = Visibility.Hidden;
                remainCounter.Visibility = Visibility.Hidden;
                PassImage.Visibility = Visibility.Hidden;
                passCounter.Visibility = Visibility.Hidden;
                FailImage.Visibility = Visibility.Hidden;
                failCounter.Visibility = Visibility.Hidden;
                defectcat.Visibility = Visibility.Hidden;
                defectBox.Visibility = Visibility.Hidden;
                defectBtn.Visibility = Visibility.Hidden;
            }

            else
            {
                unprogressCounter = Directory.GetFiles(unprogressFolder).Length;
                currProgressCounter = Convert.ToInt32(uploadCounter) - unprogressCounter;
                progressPercentage = (currProgressCounter / uploadCounter) * 100;

                remainImageCounter = Directory.GetFiles(unprogressFolder).Length;
                remainPercentage = (remainImageCounter / uploadCounter) * 100;


                totalPassCounter = Directory.GetFiles(passFolder).Length;
                passPercentage = (totalPassCounter / uploadCounter) * 100;

                totalFailCounter = Convert.ToInt32(uploadCounter) - unprogressCounter - totalPassCounter;
                failPercentage = (totalFailCounter / uploadCounter) * 100;

                currCounter.Content = currProgressCounter.ToString() + " / " + uploadCounter.ToString() + " (" + Math.Round((Double)progressPercentage, 2).ToString() + "%)";
                remainCounter.Content = remainImageCounter.ToString() + " / " + uploadCounter.ToString() + " (" + Math.Round((Double)remainPercentage, 2).ToString() + "%)";
                passCounter.Content = totalPassCounter + " (" + Math.Round((Double)passPercentage, 2).ToString() + "%)";
                failCounter.Content = totalFailCounter + " (" + Math.Round((Double)failPercentage, 2).ToString() + "%)";

                progressBar.Maximum = uploadCounter;
                progressBar.Minimum = 0;


                if (currProgressCounter > 0)
                {
                    progressBar.Value = currProgressCounter;

                    if (progressPercentage < 50)
                    {
                        progressBar.Foreground = new SolidColorBrush(Colors.Red);
                    }
                    else if (progressPercentage < 80)
                    {
                        progressBar.Foreground = new SolidColorBrush(Colors.Orange);
                    }
                    else
                    {
                        progressBar.Foreground = new SolidColorBrush(Colors.LimeGreen);
                    }
                }
                else
                {
                    progressBar.Foreground = new SolidColorBrush(Colors.Red);
                }
            }
        }
        
        //When Source Button is clicked
        private void sourceBtnClicked(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openImageFiles = new Microsoft.Win32.OpenFileDialog();
            //Allow select multiple files
            openImageFiles.Multiselect = true;
            bool? response = openImageFiles.ShowDialog();
            string imageFileSizeError = "";

            if (response == true)
            {
                //Load image's path based on the user's selection
                foreach (string imagePath in openImageFiles.FileNames)
                {

                    //Get image information based on image path
                    FileInfo imageInfo = new FileInfo(imagePath);

                    //NOT WITHIN 500kb, 75mb
                    if (imageInfo.Length < 512000 || imageInfo.Length > 78643200)
                    {
                        if (imageInfo.Length < 512000)
                        {
                            imageFileName = System.IO.Path.GetFileName(imagePath);
                            imageFileSizeError = imageFileSizeError + imageFileName + " - lesser than 500kb\n";
                        }

                        else
                        {
                            imageFileName = System.IO.Path.GetFileName(imagePath);
                            imageFileSizeError = imageFileSizeError + imageFileName + " - larger than 75mb\n";
                        }
                    }

                    //WITHIN 500kb, 75mb
                    else
                    {

                        imageFileName = System.IO.Path.GetFileName(imagePath);
                        string existPath = System.IO.Path.Combine(uploadFolder, imageFileName);

                        if (File.Exists(existPath))
                        {
                            errorImageName = errorImageName + imageFileName + "\n";
                        }
                        else
                        {
                            File.Copy(imagePath, System.IO.Path.Combine(uploadFolder, imageFileName));
                            File.Copy(imagePath, System.IO.Path.Combine(unprogressFolder, imageFileName));
                            uploadImageList.Add(new UploadImage()
                            {
                                uploadImagePath = System.IO.Path.Combine(uploadFolder, imageFileName),
                                uploadImageName = imageFileName
                            });


                            listBox.ItemsSource = uploadImageList;

                            selectedindex = 0;
                            listBox.SelectedIndex = 0;
                            picHolder.Source = new BitmapImage(new Uri(uploadImageList[listBox.SelectedIndex].uploadImagePath.ToString()));
                            picHolderName.Content = uploadImageList[listBox.SelectedIndex].uploadImageName.ToString();
                            //enable buttons
                            leftBtn.IsEnabled = true;
                            rightBtn.IsEnabled = true;
                            passBtn.Visibility = Visibility.Visible;
                            failBtn.Visibility = Visibility.Visible;
                            listBox.Items.Refresh();

                            currentPro.Visibility = Visibility.Visible;
                            progressBar.Visibility = Visibility.Visible;
                            currBox.Visibility = Visibility.Visible;
                            currCounter.Visibility = Visibility.Visible;
                            RemainImage.Visibility = Visibility.Visible;
                            remainCounter.Visibility = Visibility.Visible;
                            PassImage.Visibility = Visibility.Visible;
                            passCounter.Visibility = Visibility.Visible;
                            FailImage.Visibility = Visibility.Visible;
                            failCounter.Visibility = Visibility.Visible;
                            defectcat.Visibility = Visibility.Visible;
                            defectBox.Visibility = Visibility.Visible;
                            defectBtn.Visibility = Visibility.Visible;

                            uploadCounter = Directory.GetFiles(uploadFolder).Length;
                            remainImageCounter = Directory.GetFiles(unprogressFolder).Length;

                            progressPercentage = (currProgressCounter / uploadCounter) * 100;
                            remainPercentage = (remainImageCounter / uploadCounter) * 100;

                            if(totalPassCounter == 0)
                            {
                                passPercentage = 0;
                                passCounter.Content = 0 + " (" + 0 + "%)";
                            }
                            else
                            {
                                passPercentage = (totalPassCounter / uploadCounter) * 100;
                                passCounter.Content = totalPassCounter + " (" + Math.Round((Double)passPercentage, 2).ToString() + "%)";
                            }

                            if (totalFailCounter == 0)
                            {
                                failPercentage = 0;
                                failCounter.Content = 0 + " (" + 0 + "%)";
                            }
                            else
                            {
                                failPercentage = (totalFailCounter / uploadCounter) * 100;
                                failCounter.Content = totalFailCounter + " (" + Math.Round((Double)failPercentage, 2).ToString() + "%)";
                            }


                            currCounter.Content = currProgressCounter.ToString() + " / " + uploadCounter.ToString() + " (" + Math.Round((Double)progressPercentage, 2).ToString() + "%)";
                            remainCounter.Content = remainImageCounter.ToString() + " / " + uploadCounter.ToString() + " (" + Math.Round((Double)remainPercentage, 2).ToString() + "%)";


                            progressBar.Maximum = uploadCounter;

                            if (currProgressCounter > 0)
                            {
                                progressBar.Value = currProgressCounter;

                                if (progressPercentage < 50)
                                {
                                    progressBar.Foreground = new SolidColorBrush(Colors.Red);
                                }
                                else if (progressPercentage < 80)
                                {
                                    progressBar.Foreground = new SolidColorBrush(Colors.Orange);
                                }
                                else
                                {
                                    progressBar.Foreground = new SolidColorBrush(Colors.LimeGreen);
                                }
                            }

                            else
                            {
                                progressBar.Foreground = new SolidColorBrush(Colors.Red);
                            }
                        }
                    }
                }

                if (errorImageName != "")
                {
                    System.Windows.Forms.DialogResult confirm = System.Windows.Forms.MessageBox.Show(errorImageName + "\nThe above pictures have already been inspected!", "Error", System.Windows.Forms.MessageBoxButtons.OK);
                    errorImageName = "";
                }

                if (imageFileSizeError != "")
                {
                    System.Windows.Forms.DialogResult confirm = System.Windows.Forms.MessageBox.Show(imageFileSizeError + "\n The above image cannot be uploaded, file size must be within 500kb to 75mb!", "Error", System.Windows.Forms.MessageBoxButtons.OK);
                    imageFileSizeError = "";
                }
            }

            //Display error message if no pictures were uploaded
            else
            {
                System.Windows.MessageBox.Show("There were no images uploaded");
            }
        }

        //When a picture from listbox is selected
        private void listBoxSelected(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //Open selected image
                selectedindex = listBox.SelectedIndex;
                picHolderName.Content = uploadImageList[selectedindex].uploadImageName.ToString();
                picHolder.Source = new BitmapImage(new Uri(uploadImageList[selectedindex].uploadImagePath.ToString()));
            }

            catch (Exception)
            {
                selectedindex = 0;
                //If the listbox is empty after complete inspection
                if (listBox.Items.Count == 0)
                {
                    picHolderName.Content = "*Please Import Images*";
                    picHolder.Source = new BitmapImage(new Uri(@"C:\INTI\Semester 2\Real World Project\EP\ImageScreeningSystemHeaderFooter\ImageScreeningSystemHeaderFooter\SystemImage\noImage.jpg"));
                    leftBtn.IsEnabled = false;
                    rightBtn.IsEnabled = false;
                    passBtn.Visibility = Visibility.Hidden;
                    failBtn.Visibility = Visibility.Hidden;
                }

                //If the listbox still got image after pass or fail
                else
                {
                    picHolderName.Content = uploadImageList[selectedindex].uploadImageName.ToString();
                    picHolder.Source = new BitmapImage(new Uri(uploadImageList[selectedindex].uploadImagePath.ToString()));
                }
            }
        }

        private void leftBtnClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                selectedindex--;
                string nextPath = uploadImageList[selectedindex].uploadImagePath;
                picHolderName.Content = uploadImageList[selectedindex].uploadImageName;
                picHolder.Source = new BitmapImage(new Uri(nextPath));
            }

            //If no more picture before this picture
            catch (Exception)
            {
                selectedindex++;
                System.Windows.MessageBox.Show("There is no picture before this picture");
            }
        }

        private void rightBtnClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                selectedindex++;
                string nextPath = uploadImageList[selectedindex].uploadImagePath;
                picHolderName.Content = uploadImageList[selectedindex].uploadImageName;
                picHolder.Source = new BitmapImage(new Uri(nextPath));
            }

            //If no more picture after this picture
            catch (Exception)
            {
                selectedindex--;
                System.Windows.MessageBox.Show("There is no picture after this picture");
            }
        }

        private void passBtnClicked(object sender, RoutedEventArgs e)
        {

            //Copy image to the pass folder
            File.Delete(System.IO.Path.Combine(unprogressFolder, uploadImageList[selectedindex].uploadImageName));
            File.Copy(uploadImageList[selectedindex].uploadImagePath, System.IO.Path.Combine(passFolder, uploadImageList[selectedindex].uploadImageName));
            
            listBox.SelectedIndex = selectedindex;
            //Delete image at listbox
            foreach (UploadImage removedItem in listBox.SelectedItems)
            {
                (listBox.ItemsSource as List<UploadImage>).Remove(removedItem);
            }

            listBox.Items.Refresh();

            uploadCounter = Directory.GetFiles(uploadFolder).Length;
            currProgressCounter = currProgressCounter + 1;
            progressPercentage = (currProgressCounter / uploadCounter) * 100;

            remainImageCounter = listBox.Items.Count;
            remainPercentage = (remainImageCounter / uploadCounter) * 100;

            totalPassCounter = totalPassCounter + 1;
            passPercentage = (totalPassCounter / uploadCounter) * 100;

            failPercentage = (totalFailCounter / uploadCounter) * 100;

            currCounter.Content = currProgressCounter.ToString() + " / " + uploadCounter.ToString() + " (" + Math.Round((Double)progressPercentage, 2).ToString() + "%)";
            remainCounter.Content = remainImageCounter.ToString() + " / " + uploadCounter.ToString() + " (" + Math.Round((Double)remainPercentage, 2).ToString() + "%)";
            passCounter.Content = totalPassCounter + " (" + Math.Round((Double)passPercentage, 2).ToString() + "%)";
            failCounter.Content = totalFailCounter + " (" + Math.Round((Double)failPercentage, 2).ToString() + "%)";

            progressBar.Maximum = uploadCounter;
            progressBar.Minimum = 0;


            if (currProgressCounter > 0)
            {
                progressBar.Value = currProgressCounter;

                if (progressPercentage < 50)
                {
                    progressBar.Foreground = new SolidColorBrush(Colors.Red);
                }
                else if (progressPercentage < 80)
                {
                    progressBar.Foreground = new SolidColorBrush(Colors.Orange);
                }
                else
                {
                    progressBar.Foreground = new SolidColorBrush(Colors.LimeGreen);
                }
            }

            else
            {
                progressBar.Foreground = new SolidColorBrush(Colors.Red);
            }
  

            //If all Images are inspected
            if (listBox.Items.Count == 0)
            {
                System.Windows.MessageBox.Show("All Images had been inspected");
            }

            else
            {
                //Change to Next Picture
                //Get image path for the selected index to display
                selectedindex = listBox.SelectedIndex;
                selectedindex++;
                string nextPath = uploadImageList[selectedindex].uploadImagePath;
                picHolderName.Content = uploadImageList[selectedindex].uploadImageName;
                picHolder.Source = new BitmapImage(new Uri(nextPath));
            }

            DynamicCheckBox();

        }

        private void FailBtn_Click(object sender, RoutedEventArgs e)
        {
            FailPopOut.IsOpen = true;
            failPopOutBorder.Visibility = Visibility.Visible;

        }

        private void PercentageBtn_Click(object sender, RoutedEventArgs e)
        {
            listCate.Items.Clear();
            listPercentage.Items.Clear();
            DynamicSelectedCategory();

            PercentagePopOut.IsOpen = true;
            percentagePopOutBorder.Visibility = Visibility.Visible;
            
            FailPopOut.IsOpen = false;
            failPopOutBorder.Visibility = Visibility.Hidden;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            FailPopOut.IsOpen = false;
            failPopOutBorder.Visibility = Visibility.Hidden;
            DynamicCheckBox();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            PercentagePopOut.IsOpen = false;
            percentagePopOutBorder.Visibility = Visibility.Hidden;

            int[] EnteredPercentage = new int[100];
            var res = (from item in CateList where item.checking == true select item).ToList<cateName>();
            string findCategory = "";
            int sum = 0, highest = 0, highestIndex = 0, duplicate = 0;
            bool error = false;

            string selectedPath = uploadImageList[selectedindex].uploadImagePath;
            string selectedName = uploadImageList[selectedindex].uploadImageName;

            string information = selectedName + ";";

            int count = 0;
            foreach (cateName item in res)
            {
                string[] temp = (listPercentage.Items[count].ToString()).Split('.');
                string[] temp2 = (temp[3].Split(": "));
                if(temp2.Length < 2)
                {
                    MessageBox.Show("Please enter number for percentage");
                    error = true;
                    break;
                }
                else if(temp2[1].All(char.IsDigit)){
                    EnteredPercentage[count] = int.Parse(temp2[1]);


                    if (highest < EnteredPercentage[count])
                    {
                        highest = EnteredPercentage[count];
                        highestIndex = count;
                    }

                    sum += EnteredPercentage[count];
                    information += res[count].name.ToString() + ": " + EnteredPercentage[count] + "%;";

                    count++;
                }
                else
                {
                    MessageBox.Show("Only number will be accepted for percentage");
                    error = true;
                    break;
                }
            }

            if (!error)
            {
                for (int i = 0; i < EnteredPercentage.Length; i++)
                {
                    if (highest == EnteredPercentage[i])
                    {
                        duplicate++;
                    }
                }

                if (sum > 100)
                {
                    MessageBox.Show("Total percentage over 100%");
                }
                else if (sum < 100)
                {
                    MessageBox.Show("Total percentage less than 100%");
                }
                else if (duplicate > 1)
                {
                    MessageBox.Show("There are 2 or more same highest value");
                }
                else
                {
                    PercentagePopOut.IsOpen = false;
                    percentagePopOutBorder.Visibility = Visibility.Hidden;

                    findCategory = failFolder + res[highestIndex].name.ToString()+ "\\" + res[highestIndex].name.ToString() + ".txt";

                    using (StreamWriter file = File.AppendText(findCategory))
                    {
                        file.WriteLine(information);
                        file.Dispose();
                    }

                    findCategory = failFolder + res[highestIndex].name.ToString() + "\\" + selectedName;
                    
                    File.Delete(System.IO.Path.Combine(unprogressFolder, selectedName));
                    File.Copy(selectedPath, findCategory);

                    listBox.SelectedIndex = selectedindex;

                    //Delete image at listbox
                    foreach (UploadImage removedItem in listBox.SelectedItems)
                    {
                        (listBox.ItemsSource as List<UploadImage>).Remove(removedItem);
                    }

                    listBox.Items.Refresh();

                    //If all Images are inspected
                    if (listBox.Items.Count == 0)
                    {
                        System.Windows.MessageBox.Show("All Images had been inspected");
                    }
                    else
                    {
                        //Change to Next Picture
                        //Get image path for the selected index to display
                        selectedindex = listBox.SelectedIndex;
                        selectedindex++;
                        string nextPath = uploadImageList[selectedindex].uploadImagePath;
                        picHolderName.Content = uploadImageList[selectedindex].uploadImageName;
                        picHolder.Source = new BitmapImage(new Uri(nextPath));
                    }



                    uploadCounter = Directory.GetFiles(uploadFolder).Length;
                    currProgressCounter = currProgressCounter + 1;
                    progressPercentage = (currProgressCounter / uploadCounter) * 100;

                    remainImageCounter = listBox.Items.Count;
                    remainPercentage = (remainImageCounter / uploadCounter) * 100;

                    passPercentage = (totalPassCounter / uploadCounter) * 100;

                    totalFailCounter = totalFailCounter + 1;
                    failPercentage = (totalFailCounter / uploadCounter) * 100;

                    currCounter.Content = currProgressCounter.ToString() + " / " + uploadCounter.ToString() + " (" + Math.Round((Double)progressPercentage, 2).ToString() + "%)";
                    remainCounter.Content = remainImageCounter.ToString() + " / " + uploadCounter.ToString() + " (" + Math.Round((Double)remainPercentage, 2).ToString() + "%)";
                    passCounter.Content = totalPassCounter + " (" + Math.Round((Double)passPercentage, 2).ToString() + "%)";
                    failCounter.Content = totalFailCounter + " (" + Math.Round((Double)failPercentage, 2).ToString() + "%)";

                    progressBar.Maximum = uploadCounter;
                    progressBar.Minimum = 0;

                    if (currProgressCounter > 0)
                    {
                        progressBar.Value = currProgressCounter;

                        if (progressPercentage < 50)
                        {
                            progressBar.Foreground = new SolidColorBrush(Colors.Red);
                        }
                        else if (progressPercentage < 80)
                        {
                            progressBar.Foreground = new SolidColorBrush(Colors.Orange);
                        }
                        else
                        {
                            progressBar.Foreground = new SolidColorBrush(Colors.LimeGreen);
                        }
                    }

                    else
                    {
                        progressBar.Foreground = new SolidColorBrush(Colors.Red);
                    }

                    DynamicCheckBox();

                }
            }

        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            FailPopOut.IsOpen = true;
            failPopOutBorder.Visibility = Visibility.Visible;

            PercentagePopOut.IsOpen = false;
            percentagePopOutBorder.Visibility = Visibility.Hidden;
        }

        private void DynamicCheckBox()
        {

            if (Directory.Exists(failFolder + "\\"))
            {
                DirectoryInfo[] Categories = (new DirectoryInfo(failFolder + "\\")).GetDirectories();

                CateList.Clear();

                foreach (DirectoryInfo folder in Categories)
                {
                    CateList.Add(new cateName { checking = false, name = folder.Name });
                }

                this.DataContext = this;
            }
            else
            {
                System.Windows.MessageBox.Show("Fail folder not found!");
            }
        }

        private void DynamicSelectedCategory()
        {
            var checkSelected = (from item in CateList where item.checking == true select item).ToList<cateName>();

            foreach (cateName item in checkSelected)
            {
                Label temp = new Label();
                temp.Content = item.name;
                temp.BorderThickness = new Thickness(2);
                temp.BorderBrush = Brushes.Black;
                temp.Foreground = Brushes.Black;
                temp.Width = 180;
                temp.FontSize = 20;
                listCate.Items.Add(temp);

                TextBox nw = new TextBox();
                nw.Text = "";
                nw.Foreground = Brushes.Black;
                nw.BorderBrush = Brushes.Black;
                nw.BorderThickness = new Thickness(2);
                nw.FontSize = 20;
                nw.Width = 180;
                nw.Height = 40;
                listPercentage.Items.Add(nw);

            }

        }

        private void defectBtnClicked(object sender, RoutedEventArgs e)
        {       
            DefectName_text.Visibility = Visibility.Hidden;
            DefectSelectedName.Visibility = Visibility.Hidden;
            DefectCount_Text.Visibility = Visibility.Hidden;
            DefectPercentage.Visibility = Visibility.Hidden;

            checkDefectProgress.IsOpen = true;
            popUpBorder.Visibility = Visibility.Visible;

            DefectCategoryListBox.Items.Clear();
            DirectoryInfo[] names = new DirectoryInfo(failFolder).GetDirectories();
            foreach (DirectoryInfo name in names)
            {
                DefectCategoryListBox.Items.Add(name.Name);
            }

            DefectCategoryListBox.Items.Refresh();
        }

        private void DefectName(object sender, SelectionChangedEventArgs e)
        {
            DefectName_text.Visibility = Visibility.Visible;
            DefectSelectedName.Visibility = Visibility.Visible;
            DefectCount_Text.Visibility = Visibility.Visible;
            DefectPercentage.Visibility = Visibility.Visible;

            if (DefectCategoryListBox.SelectedIndex >= 0)
            {
                DefectSelected = DefectCategoryListBox.SelectedItem.ToString();

                DefectSelectedName.Content = DefectSelected;
                FailPicCount = Directory.GetFiles(failFolder + DefectSelected + "\\").Length -1;
                FailPicPercentage = Convert.ToDouble(FailPicCount) / Convert.ToDouble(totalFailCounter) * 100;

                if (Double.IsNaN(FailPicPercentage) || FailPicPercentage == null)
                {
                    FailPicPercentage = 0;
                }

                DefectPercentage.Content = FailPicCount + " (" + Math.Round((Double)FailPicPercentage, 2) + "%)";
            }
        }

        private void DefectCategoryListBox_Closing(object sender, RoutedEventArgs e)
        {
            DefectCategoryListBox.UnselectAll();
            popUpBorder.Visibility = Visibility.Hidden;
            checkDefectProgress.IsOpen = false;
        }

    }
}
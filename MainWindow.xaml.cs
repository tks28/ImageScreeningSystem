using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace ImageScreeningSystemHeaderFooter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Initialise variables
        string passFolder;
        string failFolder;
        string unprogressFolder;
        string MainPath;
        string uploadFolder;
        ImageScreening mainPage;

        public MainWindow(string items)
        {
            InitializeComponent();
            MainPath = @"C:\ImageScreeningSystem\";
            MainPath = MainPath + items;
            passFolder = MainPath + @"\Pass\";
            failFolder = MainPath + @"\Fail\";
            uploadFolder = MainPath + @"\Uploaded";
            unprogressFolder = MainPath + @"\Unprogress\";

            mainPage = new ImageScreening(MainPath);
            Main.Content = mainPage;
        }

        private void Button_ClickScreening(object sender, RoutedEventArgs e)
        {
            GridCursor_onManage.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Transparent");
            GridCursor_onViewImage.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Transparent");
            GridCursor_onSession.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Transparent");

            GridCursor_onImageScreening.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF2196F3");

            Main.Content = mainPage;
            mainPage.reload();
        }

        private void Button_ClickManage(object sender, RoutedEventArgs e)
        {
            GridCursor_onImageScreening.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Transparent");
            GridCursor_onViewImage.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Transparent");
            GridCursor_onSession.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Transparent");

            GridCursor_onManage.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF2196F3");

            Main.Content = new ManageDefectCategories(failFolder);

        }

        private void Button_ClickPass(object sender, RoutedEventArgs e)
        {
            GridCursor_onImageScreening.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Transparent");
            GridCursor_onManage.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Transparent");
            GridCursor_onSession.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Transparent");

            GridCursor_onViewImage.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF2196F3");

            Main.Content = new Pass(MainPath);

        }

        private void Button_ClickFail(object sender, RoutedEventArgs e)
        {
            GridCursor_onImageScreening.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Transparent");
            GridCursor_onManage.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Transparent");
            GridCursor_onSession.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Transparent");

            GridCursor_onViewImage.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF2196F3");

            Main.Content = new Fail(MainPath);
        }

        private void Button_ClickSession(object sender, RoutedEventArgs e)
        {
            GridCursor_onImageScreening.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Transparent");
            GridCursor_onManage.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Transparent");
            GridCursor_onViewImage.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Transparent");

            GridCursor_onSession.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF2196F3");

            SessionSelect ss = new SessionSelect();
            ss.Show();
            this.Close();
        }
    }
    }


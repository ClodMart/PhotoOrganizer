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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using PhotoOrganizer.ViewModels;
using System.IO;
using PhotoOrganizer.Classes;
using Path = System.IO.Path;
using System.Drawing.Imaging;
using System.Drawing;


namespace PhotoOrganizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FolderBrowserDialog dialog = new FolderBrowserDialog();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new AppViewModel();
        }

        private void SelectSourcePath_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (Directory.Exists(((AppViewModel)DataContext).Settings.SourcePath))
            {
                dialog.InitialDirectory = ((AppViewModel)DataContext).Settings.SourcePath;
            }
            dialog.ShowDialog();
            ((AppViewModel)DataContext).Settings.SourcePath = dialog.SelectedPath;
        }
        private void SelectOutPath_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (Directory.Exists(((AppViewModel)DataContext).Settings.OutputPath))
            {
                dialog.InitialDirectory = ((AppViewModel)DataContext).Settings.OutputPath;
            }
                dialog.ShowDialog();
            ((AppViewModel)DataContext).Settings.OutputPath = dialog.SelectedPath;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((AppViewModel)DataContext).Settings.SaveSettings();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //((AppViewModel)DataContext).Settings.LoadSettings();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            PhotoOrganizer.Settings s = new PhotoOrganizer.Settings();
            s.DataContext = ((AppViewModel)DataContext).Settings;
            s.ShowDialog();
        }

        private void StartProcessing_Click(object sender, RoutedEventArgs e)
        {
            ((AppViewModel)DataContext).Log += "Start Processing.....";
            string Name = ""; string Taken_Date = ""; string Taken_Time = ""; string Location = ""; string Creator = ""; string Title = ""; string Folder_Name = ""; string Username = ""; string Today_Date = ""; string Now_Time = "";
            List<String> Files = ((AppViewModel)DataContext).GetImages();

            foreach(String x in Files)
            {
                FileInfo f = new FileInfo(x);
                if(f.Extension == ".jpg" || f.Extension == ".jpeg" || f.Extension == ".png" || f.Extension == ".bmp")
                {
                    using (FileStream fs = new FileStream(f.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        BitmapSource img = BitmapFrame.Create(fs);
                        BitmapMetadata md = (BitmapMetadata)img.Metadata;

                        try
                        {
                            if (((AppViewModel)DataContext).Settings.UseMetadata)
                            {
                                if (md.DateTaken != null)
                                {
                                    Taken_Date = (DateTime.Parse(md.DateTaken)).Date.ToString().Substring(0, 10).Replace('/','-');
                                    Taken_Time = (DateTime.Parse(md.DateTaken)).TimeOfDay.ToString().Replace(':', '.');
                                }
                                else
                                {
                                    Taken_Date = Taken_Time = "_";
                                }
                                if (f.Name != null)
                                {
                                    Name = f.Name.Substring(0,f.Name.Length -4);
                                }
                                else
                                {
                                    Name = "_";
                                }
                                if(md.Location != null)
                                {
                                    ulong[] latitude = md.GetQuery("/app1/ifd/gps/subifd:{ulong=2}") as ulong[];
                                    ulong[] longitude = md.GetQuery("/app1/ifd/gps/subifd:{ulong=4}") as ulong[];
                                    double lat = Math.Round(ConvertCoordinate(latitude),4);
                                    double longit = Math.Round(ConvertCoordinate(longitude), 4);
                                    try
                                    {
                                        Location = new ApiRequest(lat, longit).GetCity();
                                    }
                                    catch
                                    {
                                        Location = "Gps not available";
                                    }
                                }
                                else
                                {
                                    Location = "_";
                                }
                                if (md.Author != null)
                                {
                                    foreach (string s in md.Author)
                                    {
                                        Creator += s;
                                    }
                                }
                                else
                                {
                                    Creator = "_";
                                }
                                if (md.Title != null)
                                {
                                    Title = md.Title;
                                }
                                else
                                {
                                    Title = "_";
                                }
                                if (f.FullName != null)
                                {
                                    Folder_Name = Path.GetDirectoryName(f.FullName);
                                    int c = Folder_Name.LastIndexOf('\\') + 1;
                                    Folder_Name = Folder_Name.Substring(c, Folder_Name.Length - c );
                                }
                                else
                                {
                                    Folder_Name = "_";
                                }
                            }
                            Username = Environment.UserName;
                            Today_Date = DateTime.Now.Date.ToString().Substring(0, 10).Replace('/', '-');
                            Now_Time = DateTime.Now.TimeOfDay.ToString().Replace(':', '.');
                            fs.Close();
                            ((AppViewModel)DataContext).Log += "\nProcessing image: " + f.Name + "\nMetadata:\n" +
                                "Date taken: " + Taken_Date + " " + Taken_Time +
                                "\nName: " + Name +
                                "\nLocation: " + Location +
                                "\nCreator: " + Creator +
                                "\nTitle: " + Title +
                                "\nFolder Name: " + Folder_Name +
                                "\nUsername: " + Username +
                                "\nNow: " + Today_Date + " " + Now_Time + "\n";

                        }
                        catch
                        {
                            try
                            {
                                fs.Close();
                                img = ImageFromGDIplus(f.FullName);
                                md = (BitmapMetadata)img.Metadata;
                                Taken_Date = (DateTime.Parse(md.DateTaken)).Date.ToString().Substring(0, 10).Replace('/', '-');
                                Taken_Time = (DateTime.Parse(md.DateTaken)).TimeOfDay.ToString().Replace(':', '.');
                                Name = f.Name;
                                Location = md.Location;
                                foreach (string s in md.Author)
                                {
                                    Creator += s;
                                }
                                Title = md.Title;
                                Folder_Name = Path.GetDirectoryName(f.FullName);
                                Username = Environment.UserName;
                                Today_Date = DateTime.Now.Date.ToString().Replace('/', '-');
                                Now_Time = DateTime.Now.TimeOfDay.ToString().Replace(':', '.');
                            }
                            catch
                            {
                                ((AppViewModel)DataContext).Log += "\n Bad metadata for image" + f.FullName + " Can't process try without using metadata";
                            }
                        }
                    }
                    //Name = (string)pngInplace.GetQuery("/Text/Name");
                }
                string NewName = ((AppViewModel)DataContext).Settings.NamingConventionParse(Name, Taken_Date, Taken_Time, Location, Creator, Title, Folder_Name, Username, Today_Date, Now_Time);
                string NewPath = ((AppViewModel)DataContext).Settings.OutputPath + "\\" +((AppViewModel)DataContext).Settings.FolderConventionParse(Name, Taken_Date, Taken_Time, Location, Creator, Title, Folder_Name);
                string NewFile = NewPath + "\\" + NewName;
                Directory.CreateDirectory(NewPath);
                f.CopyTo(NewFile);
                ((AppViewModel)DataContext).Log += "File moved to " + NewFile +"\n\n";
            }

        }
        private BitmapImage ImageFromGDIplus(string uriPath)

        {

            Bitmap badMetadataImage = new Bitmap(uriPath);

            ImageCodecInfo myImageCodecInfo;

            System.Drawing.Imaging.Encoder myEncoder;

            EncoderParameter myEncoderParameter;

            EncoderParameters myEncoderParameters;

            // get an ImageCodecInfo object that represents the JPEG codec

            myImageCodecInfo = GetEncoderInfo("image/jpeg");

            // Create an Encoder object based on the GUID for the Quality parameter category

            myEncoder = System.Drawing.Imaging.Encoder.Quality;

            // Create an EncoderParameters object

            // An EncoderParameters object has an array of EncoderParameter objects.

            // In this case, there is only one EncoderParameter object in the array.

            myEncoderParameters = new EncoderParameters(1);

            // Save the image as a JPEG file with quality level 75.

            myEncoderParameter = new EncoderParameter(myEncoder, 75L);

            myEncoderParameters.Param[0] = myEncoderParameter;

            badMetadataImage.Save(@"C:\Temp\foo.bmp", myImageCodecInfo, myEncoderParameters);

            // Create the source to use as the tb source

            BitmapImage bi = new BitmapImage();

            bi.BeginInit();

            bi.UriSource = new Uri(@"C:\Temp\foo.bmp", UriKind.Absolute);

            bi.EndInit();

            return bi;

        }





        private static ImageCodecInfo GetEncoderInfo(String mimeType)

        {

            int j;

            ImageCodecInfo[] encoders;

            encoders = ImageCodecInfo.GetImageEncoders();

            for (j = 0; j < encoders.Length; ++j)

            {

                if (encoders[j].MimeType == mimeType)

                    return encoders[j];

            }

            return null;

        }

        static double ConvertCoordinate(ulong[] coordinates)
        {
            if (coordinates == null)
                return 0;

            double degrees = ConvertToUnsignedRational(coordinates[0]);
            double minutes = ConvertToUnsignedRational(coordinates[1]);
            double seconds = ConvertToUnsignedRational(coordinates[2]);
            return degrees + (minutes / 60.0) + (seconds / 3600);

        }
        static double ConvertToUnsignedRational(ulong value)
        {
            //return value;
            return (value & 0xFFFFFFFFL) / (double)((value & 0xFFFFFFFF00000000L) >> 32);
        }
    }

}

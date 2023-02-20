using PhotoOrganizer.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PhotoOrganizer.ViewModels
{
    public class AppViewModel :BaseViewModel
    {
        private SettingsViewModel settings = new SettingsViewModel();
        public SettingsViewModel Settings
        {
            get { return settings; }
            set 
            { 
                settings = value;
                NotifyPropertyChanged();
            }
        }

        private string log;
        public string Log
        {
            get { return log; }
            set
            { 
                log = value;
                NotifyPropertyChanged();
            }
        }

        private static List<String> GetFilesFrom(String searchFolder, String[] filters, bool isRecursive)
        {
            List<String> filesFound = new List<String>();
            var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (var filter in filters)
            {
                filesFound.AddRange(Directory.GetFiles(searchFolder, String.Format("*.{0}", filter), searchOption));
            }
            return filesFound;
        }

        public List<String> GetImages()
        {
            List<String> filters = new List<String>();
            if(Settings.ImageFormatIN == Classes.ImageFormatIN.All)
            {
                filters.Add(ImageFormatIN.JPEG.ToString().ToLower());
                filters.Add("jpg");
                filters.Add(ImageFormatIN.BMP.ToString().ToLower());
                filters.Add(ImageFormatIN.PNG.ToString().ToLower());
            }
            else
            {
                if(Settings.ImageFormatIN == Classes.ImageFormatIN.JPEG)
                {
                    filters.Add(ImageFormatIN.JPEG.ToString().ToLower());
                    filters.Add("jpg");
                }
                else
                {
                    filters.Add(Settings.ImageFormatIN.ToString().ToLower());
                }                
            }
            List<String> Out = GetFilesFrom(Settings.SourcePath, filters.ToArray(), Settings.Recursive);
            return Out;
        }
    }
}

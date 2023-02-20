using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Newtonsoft.Json;
using PhotoOrganizer.Classes;

namespace PhotoOrganizer.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly string configfile = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\config.json";
        private readonly SettingsViewModel Backup;

        #region ViewModelProperties
        private string sourcePath = "test";
        public string SourcePath
        {
            get { return sourcePath; }
            set 
            { 
                sourcePath = value;
                NotifyPropertyChanged();
            }
        }

        private string outputPath = "test2";
        public string OutputPath
        {
            get { return outputPath; }
            set
            {
                outputPath = value;
                NotifyPropertyChanged();
            }
        }

        private bool useMetadata = true;
        public bool UseMetadata
        {
            get { return useMetadata; }
            set { useMetadata = value; NotifyPropertyChanged(); }
        }

        private bool createSubfolder = true;
        public bool CreateSubfolder
        {
            get { return createSubfolder; }
            set { createSubfolder = value; NotifyPropertyChanged(); }
        }

        private string folderConvention = "test";
        public string FolderConvention
        {
            get { return folderConvention; }
            set { folderConvention = value; NotifyPropertyChanged(); }
        }

        private bool recursive = true;
        public bool Recursive
        {
            get { return recursive; }
            set { recursive = value; NotifyPropertyChanged(); }
        }

        private SubfolderSettings subfolderName = SubfolderSettings.Photo_Name;

        public SubfolderSettings SubfolderName
        {
            get { return subfolderName; }
            set { subfolderName = value; NotifyPropertyChanged(); }
        }

        private ImageFormat imageFormat = ImageFormat.JPEG;
        public ImageFormat ImageFormat
        {
            get { return imageFormat; }
            set { imageFormat = value; NotifyPropertyChanged(); }
        }

        private ImageFormatIN imageFormatIN = ImageFormatIN.JPEG;
        public ImageFormatIN ImageFormatIN
        {
            get { return imageFormatIN; }
            set { imageFormatIN = value; NotifyPropertyChanged(); }
        }


        private string namingConvention = "test";
        public string NamingConvention
        {
            get { return namingConvention; }
            set { namingConvention = value; NotifyPropertyChanged(); }
        }
        #endregion

        


        public SettingsViewModel(string sourcePath, string outputPath, bool createSubfolder, SubfolderSettings subfolderName, ImageFormat imageFormat, ImageFormatIN imageFormatIN, string namingConvention, bool recursive, bool useMetadata, string folderConvention)
        {
            SourcePath = sourcePath;
            OutputPath = outputPath;
            CreateSubfolder = createSubfolder;
            SubfolderName = subfolderName;
            ImageFormat = imageFormat;
            NamingConvention = namingConvention;
            ImageFormatIN = imageFormatIN;
            Recursive = recursive;
            UseMetadata = useMetadata;
            FolderConvention = folderConvention;
        }

        public SettingsViewModel()
        {
            Backup = new SettingsViewModel(SourcePath, OutputPath, CreateSubfolder, SubfolderName, ImageFormat, ImageFormatIN, NamingConvention, Recursive, UseMetadata, FolderConvention);
            LoadSettings();
            BackupSettings();
        }

        #region SettingsSave/Load
        public void BackupSettings()
        {
            Backup.SourcePath = SourcePath;
            Backup.OutputPath = OutputPath;
            Backup.CreateSubfolder = CreateSubfolder;
            Backup.SubfolderName = SubfolderName;
            Backup.ImageFormat = ImageFormat;
            Backup.NamingConvention = NamingConvention;
            Backup.ImageFormatIN = ImageFormatIN;
            Backup.Recursive = Recursive;
            Backup.UseMetadata = UseMetadata;
            Backup.FolderConvention = FolderConvention;
        }

        public void RestoreViewModel ()
        {

            SourcePath = Backup.SourcePath;
            OutputPath = Backup.OutputPath;
            CreateSubfolder = Backup.CreateSubfolder;
            SubfolderName = Backup.SubfolderName;
            ImageFormat = Backup.ImageFormat;
            NamingConvention = Backup.NamingConvention;
            ImageFormatIN = Backup.ImageFormatIN;
            Recursive = Backup.Recursive;
            UseMetadata = Backup.UseMetadata;
            FolderConvention = Backup.FolderConvention;
        }

        public void SaveSettings()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                //writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();
                writer.WritePropertyName("SourcePath");
                writer.WriteValue(SourcePath);
                writer.WritePropertyName("OutputPath");
                writer.WriteValue(OutputPath);
                writer.WritePropertyName("CreateSubfolder");
                writer.WriteValue(CreateSubfolder);
                writer.WritePropertyName("SubfolderName");
                writer.WriteValue(SubfolderName);
                writer.WritePropertyName("ImageFormat");
                writer.WriteValue(ImageFormat);
                writer.WritePropertyName("ImageFormatIN");
                writer.WriteValue(ImageFormatIN);
                writer.WritePropertyName("NamingConvention");
                writer.WriteValue(NamingConvention);
                writer.WritePropertyName("Recursive");
                writer.WriteValue(Recursive);
                writer.WritePropertyName("UseMetadata");
                writer.WriteValue(UseMetadata);
                writer.WritePropertyName("FolderConvention");
                writer.WriteValue(FolderConvention);
                writer.WriteEndObject();
            }
            File.WriteAllTextAsync(configfile, sw.ToString());
        }

        public void LoadSettings()
        {
            string PropName = "";
            string json = File.ReadAllText(configfile);
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.TokenType.ToString() == "PropertyName")
                    {
                        PropName = reader.Value.ToString();
                    }
                    else
                    {
                        switch (PropName)
                        {
                            case "SourcePath":
                                SourcePath = reader.Value.ToString();
                                break;
                            case "OutputPath":
                                OutputPath = reader.Value.ToString();
                                break ;
                            case "CreateSubfolder":
                                CreateSubfolder = (bool)reader.Value;
                                break ;
                            case "SubfolderName":
                                Enum.TryParse(reader.Value.ToString(), out SubfolderSettings DirName);
                                SubfolderName = DirName;
                                break ; 
                            case "ImageFormat":
                                Enum.TryParse(reader.Value.ToString(), out ImageFormat ImgFormat);
                                ImageFormat = ImgFormat;
                                break;
                            case "ImageFormatIN":
                                Enum.TryParse(reader.Value.ToString(), out ImageFormatIN ImgFormatIN);
                                ImageFormatIN = ImgFormatIN;
                                break;
                            case "NamingConvention":
                                NamingConvention = reader.Value.ToString();
                                break;
                            case "Recursive":
                                Recursive = (bool)reader.Value;
                                break;
                            case "UseMetadata":
                                UseMetadata = (bool)reader.Value;
                                break;
                            case "FolderConvention":
                                FolderConvention = reader.Value.ToString();
                                break;
                            default: 
                                break;

                        }
                    }
                }
                else
                {

                }
            }

        }
        #endregion

        public void UpdateNamingConvention( string s)
        {
            NamingConvention += s;
        }

        public void UpdateFollderConvention(string s)
        {
            FolderConvention += s;
        }
        public string NamingConventionParse(string Name, string Taken_Date, string Taken_Time, string Location, string Creator, string Title, string Folder_Name, string Username, string Today_Date, string Now_Time)
        {
            string NewName = NamingConvention;
            NewName = NewName.Replace("$Name$", Name);
            NewName = NewName.Replace("$Taken_Date$", Taken_Date);
            NewName = NewName.Replace("$Taken_Time$", Taken_Time);
            NewName = NewName.Replace("$Location$", Location);
            NewName = NewName.Replace("$Creator$", Creator);
            NewName = NewName.Replace("$Title$", Title);
            NewName = NewName.Replace("$Folder_Name$", Folder_Name);
            NewName = NewName.Replace("$Username$", Username);
            NewName = NewName.Replace("$Today_Date$", Today_Date);
            NewName = NewName.Replace("$Now_Time$", Now_Time);
            return NewName + "." + imageFormat.ToString().ToLower();
        }

        public string FolderConventionParse(string Name, string Taken_Date, string Taken_Time, string Location, string Creator, string Title, string Folder_Name)
        {
            string NewName = FolderConvention;
            NewName = NewName.Replace("$Photo_Name$", Name);
            NewName = NewName.Replace("$Photo_Title$", Title);
            NewName = NewName.Replace("$Taken_Date$", Taken_Date);
            NewName = NewName.Replace("$Taken_DateTime$", Taken_Date+" "+Taken_Time);
            NewName = NewName.Replace("$Photo_Location$", Location);
            NewName = NewName.Replace("$Photo_Creator$", Creator);
            NewName = NewName.Replace("$OriginalFolderName$", Folder_Name);
            NewName = NewName.Replace("$Folder_Name$", Folder_Name);
            return NewName;
        }

        //public string GetFolder(string Name, string Taken_Date, string Taken_Time, string Location, string Creator, string Title, string Folder_Name)
        //{
        //    string Out = "";
        //    switch (subfolderName)
        //    {
        //        case SubfolderSettings.Photo_Name:
        //            Out = Name;
        //            break;
        //        case SubfolderSettings.Photo_Title:
        //            Out = Title;
        //            break;
        //        case SubfolderSettings.Taken_Date:
        //            Out = Taken_Date;
        //            break;
        //        case SubfolderSettings.Taken_DateTime:
        //            Out = Taken_Date +" "+ Taken_Time;
        //            break;
        //        case SubfolderSettings.Photo_Location:
        //            Out = Location;
        //            break;
        //        case SubfolderSettings.Photo_Creator:
        //            Out = Creator;
        //            break;
        //        case SubfolderSettings.OriginalFolderName:
        //            Out = Folder_Name;
        //            break;
        //    }

        //    return Out;
        //}
    }
}

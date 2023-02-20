using PhotoOrganizer.Classes;
using PhotoOrganizer.ViewModels;
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
using System.Windows.Shapes;

namespace PhotoOrganizer
{
    /// <summary>
    /// Logica di interazione per Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
        }
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            ((SettingsViewModel)DataContext).BackupSettings();
            this.Close();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            ((SettingsViewModel)DataContext).RestoreViewModel();
        }

        private void SelectMetadata_Click(object sender, RoutedEventArgs e)
        {
            switch (ImageMetadata.SelectedItem)
            {
                case Image_Metadata.Name:
                    ((SettingsViewModel)DataContext).UpdateNamingConvention("$Name$");
                    break;
                case Image_Metadata.Taken_Date:
                    ((SettingsViewModel)DataContext).UpdateNamingConvention("$Taken_Date$");
                    break;
                case Image_Metadata.Taken_Time:
                    ((SettingsViewModel)DataContext).UpdateNamingConvention("$Taken_Time$");
                    break;
                case Image_Metadata.Location:
                    ((SettingsViewModel)DataContext).UpdateNamingConvention("$Location$");
                    break;
                case Image_Metadata.Creator:
                    ((SettingsViewModel)DataContext).UpdateNamingConvention("$Creator$");
                    break;
                case Image_Metadata.Title:
                    ((SettingsViewModel)DataContext).UpdateNamingConvention("$Title$");
                    break;
            }                        
        }

        private void SelectInfo_Click(object sender, RoutedEventArgs e)
        {
            switch (OtherMetadata.SelectedItem)
            {
                case OtherSettings.Folder_Name:
                    ((SettingsViewModel)DataContext).UpdateNamingConvention("$Folder_Name$");
                    break;
                case OtherSettings.Username:
                    ((SettingsViewModel)DataContext).UpdateNamingConvention("$Username$");
                    break;
                case OtherSettings.Today_Date:
                    ((SettingsViewModel)DataContext).UpdateNamingConvention("$Today_Date$");
                    break;
                case OtherSettings.Now_Time:
                    ((SettingsViewModel)DataContext).UpdateNamingConvention("$Now_Time$");
                    break;
            }
        }

        private void SelectFolderOptions_Click(object sender, RoutedEventArgs e)
        {
            switch (FolderOption.SelectedItem)
            {
                case SubfolderSettings.Photo_Name:
                    ((SettingsViewModel)DataContext).UpdateFollderConvention("$Photo_Name$");
                    break;
                case SubfolderSettings.Photo_Title:
                    ((SettingsViewModel)DataContext).UpdateFollderConvention("$Photo_Title$");
                    break;
                case SubfolderSettings.Taken_Date:
                    ((SettingsViewModel)DataContext).UpdateFollderConvention("$Taken_Date$");
                    break;
                case SubfolderSettings.Taken_DateTime:
                    ((SettingsViewModel)DataContext).UpdateFollderConvention("$Taken_DateTime$");
                    break;
                case SubfolderSettings.Photo_Location:
                    ((SettingsViewModel)DataContext).UpdateFollderConvention("$Photo_Location$");
                    break;
                case SubfolderSettings.Photo_Creator:
                    ((SettingsViewModel)DataContext).UpdateFollderConvention("$Photo_Creator$");
                    break;
                case SubfolderSettings.OriginalFolderName:
                    ((SettingsViewModel)DataContext).UpdateFollderConvention("$OriginalFolderName$");
                    break;
            }
        }
    }
}

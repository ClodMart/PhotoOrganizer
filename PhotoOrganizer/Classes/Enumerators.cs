using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoOrganizer.Classes
{
    internal enum Image_Metadata
    {
        Name,
        Taken_Date,
        Taken_Time,
        Location,
        Creator,
        Title
    }
    public enum ImageFormat
    {
        JPEG,
        BMP,
        PNG
    }

    public enum ImageFormatIN
    {
        JPEG,
        BMP,
        PNG,
        All
    }

    internal enum OtherSettings
    {
        Folder_Name,
        Username,
        Today_Date,
        Now_Time
    }

    public enum SubfolderSettings
    {
        Photo_Name,
        Photo_Title,
        Taken_Date,
        Taken_DateTime,
        Photo_Location,
        Photo_Creator,
        OriginalFolderName
    }
}

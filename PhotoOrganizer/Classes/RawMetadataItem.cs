using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace PhotoOrganizer.Classes
{
    class RawMetadataItem
    {
        public String location;
        public Object value;

        public static List<RawMetadataItem> CaptureMetadata(ImageMetadata imageMetadata, string query)
        {
            List<RawMetadataItem> RawMetadataItems = new List<RawMetadataItem>();
            BitmapMetadata bitmapMetadata = imageMetadata as BitmapMetadata;

            if (bitmapMetadata != null)
            {
                foreach (string relativeQuery in bitmapMetadata)
                {
                    string fullQuery = query + relativeQuery;
                    object metadataQueryReader = bitmapMetadata.GetQuery(relativeQuery);
                    RawMetadataItem metadataItem = new RawMetadataItem();
                    metadataItem.location = fullQuery;
                    metadataItem.value = metadataQueryReader;
                    RawMetadataItems.Add(metadataItem);
                    BitmapMetadata innerBitmapMetadata = metadataQueryReader as BitmapMetadata;
                    if (innerBitmapMetadata != null)
                    {
                        CaptureMetadata(innerBitmapMetadata, fullQuery);
                    }
                    else
                    {
                        CaptureMetadata(null, fullQuery);
                    }
                }
            }
            return RawMetadataItems;
        }
    }
}

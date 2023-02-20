using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PhotoOrganizer.Classes
{
    public class ApiRequest
    {
        public string Url = "https://neutrinoapi.net/geocode-reverse";
        public string UserId = "Cloner666";
        public string key = "zASMsijNwBfsvffImkBSFBsMGXW7nPfQtlUTX01KqAfI8aI1";
        public double latitutde;
        public double longitude;
        public string lenguage;
        private static readonly HttpClient client = new HttpClient();

        public ApiRequest(double Latitude, double Longitude)
        {
            latitutde = Latitude;
            longitude = Longitude;
            client.BaseAddress = new Uri(Url);
        }
        
        public string GetCity()
        {
            string json = client.GetAsync(Url + "?User-ID=" + UserId + "&API-Key=" + key + "&latitude=" + latitutde + "&longitude=" + longitude + "language-code=" + lenguage + "&zoom=city").ToString();
            string Out = "";
            bool PropFound = false;
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.TokenType.ToString() == "PropertyName")
                    {
                        if (reader.Value == "city")
                        {
                            PropFound = true;
                        }
                    }
                    else if (PropFound)
                    {
                        Out = reader.Value.ToString();
                        break;
                    }
                }
            }
            if (Out == "" || Out == null)
            {
                Out = "GPS data not found";
            }
                    return Out;
        }
    }
}

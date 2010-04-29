using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SimpleAppReviews
{
    class AppStoreManager
    {
        static Dictionary<string, string[]> info = new Dictionary<string, string[]>();
        static bool isInit = false;

        static public void Initialize()
        {
            if (isInit)
                return;

            info.Add("143441", new string[] { "United States", "US" });
            info.Add("143460", new string[] { "Australia", "AU" });
            info.Add("143455", new string[] { "Canada", "CA" });
            info.Add("143443", new string[] { "Germany", "DE" });
            info.Add("143454", new string[] { "Spain", "ES" });
            info.Add("143442", new string[] { "France", "FR" });
            info.Add("143450", new string[] { "Italy", "IT" });
            info.Add("143452", new string[] { "Netherlands", "NL" });
            info.Add("143444", new string[] { "United Kingdom", "GB" });
            info.Add("143462", new string[] { "Japan", "JP" });
            // End of top countries

            info.Add("143505", new string[] { "Argentina", "AR" });
            info.Add("143446", new string[] { "Belgium", "BE" });
            info.Add("143503", new string[] { "Brazil", "BR" });
            info.Add("143483", new string[] { "Chile", "CL" });
            info.Add("143465", new string[] { "China", "CN" });
            info.Add("143501", new string[] { "Colombia", "CO" });
            info.Add("143495", new string[] { "Costa Rica", "CR" });
            info.Add("143489", new string[] { "Czech Republic", "CZ" });
            info.Add("143458", new string[] { "Denmark", "DK" });
            info.Add("143506", new string[] { "El Salvador", "SV" });
            info.Add("143447", new string[] { "Finland", "FI" });
            info.Add("143448", new string[] { "Greece", "GR" });
            info.Add("143504", new string[] { "Guatemala", "GT" });
            info.Add("143463", new string[] { "Hong Kong", "HK" });
            info.Add("143482", new string[] { "Hungary", "HU" });
            info.Add("143467", new string[] { "India", "IN" });
            info.Add("143476", new string[] { "Indonesia", "ID" });
            info.Add("143449", new string[] { "Ireland", "IE" });
            info.Add("143491", new string[] { "Israel", "IL" });
            info.Add("143466", new string[] { "Korea", "KR" });
            info.Add("143493", new string[] { "Kuwait", "KW" });
            info.Add("143497", new string[] { "Lebanon", "LB" });
            info.Add("143451", new string[] { "Luxemburg", "LU" });
            info.Add("143473", new string[] { "Malaysia", "MY" });
            info.Add("143468", new string[] { "Mexico", "MX" });
            info.Add("143461", new string[] { "New Zealand", "NZ" });
            info.Add("143457", new string[] { "Norway", "NO" });
            info.Add("143445", new string[] { "Austria", "AT" });
            info.Add("143477", new string[] { "Pakistan", "PK" });
            info.Add("143485", new string[] { "Panama", "PA" });
            info.Add("143507", new string[] { "Peru", "PE" });
            info.Add("143474", new string[] { "Phillipines", "PH" });
            info.Add("143478", new string[] { "Poland", "PL" });
            info.Add("143453", new string[] { "Portugal", "PT" });
            info.Add("143498", new string[] { "Qatar", "QA" });
            info.Add("143487", new string[] { "Romania", "RO" });
            info.Add("143469", new string[] { "Russia", "RU" });
            info.Add("143479", new string[] { "Saudi Arabia", "SA" });
            info.Add("143459", new string[] { "Switzerland", "CH" });
            info.Add("143464", new string[] { "Singapore", "SG" });
            info.Add("143496", new string[] { "Slovakia", "SK" });
            info.Add("143499", new string[] { "Slovenia", "SI" });
            info.Add("143472", new string[] { "South Africa", "ZA" });
            info.Add("143486", new string[] { "Sri Lanka", "LK" });
            info.Add("143456", new string[] { "Sweden", "SE" });
            info.Add("143470", new string[] { "Taiwan", "TW" });
            info.Add("143475", new string[] { "Thailand", "TH" });
            info.Add("143480", new string[] { "Turkey", "TR" });
            info.Add("143481", new string[] { "United Arab Emirates", "AE" });
            info.Add("143502", new string[] { "Venezuela", "VE" });
            info.Add("143471", new string[] { "Vietnam", "VN" });

            isInit = true;
        }

        static public Image GetImageFromID(string id)
        {
            AppStoreManager.Initialize();            
            string fileName = "ImagesFlags/" + info[id][1] + ".png";
            if (!System.IO.File.Exists(fileName))
                return null;

            return Image.FromFile(fileName);
        }

        static public string GetCountryFromID(string id)
        {
            AppStoreManager.Initialize();
            return info[id][0];
        }

        static public string[] getStoreIDs()
        {
            AppStoreManager.Initialize();
            return info.Keys.ToArray();
        }

        static public int count()
        {
            AppStoreManager.Initialize();
            return info.Keys.Count();
        }

    }
}
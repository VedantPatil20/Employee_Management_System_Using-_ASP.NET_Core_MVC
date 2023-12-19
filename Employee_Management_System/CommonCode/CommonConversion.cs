using Newtonsoft.Json;
using System.Globalization;

namespace Employee_Management_System.CommonCode
{
    public static class CommonConversion
    {
        static TextInfo myTextInfo = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo;

        public static string APIPath = "http://localhost/carecloudapi/api/";
        public static string ConvertDBNullToString(this object sValue, bool IsCasingRequired = false)
        {
            if (sValue == DBNull.Value)
            { return ""; }
            if (!IsCasingRequired)
            {
                return Convert.ToString(sValue);
            }
            else
            {
                return myTextInfo.ToTitleCase(sValue.ToString().ToLower());
            }
        }
        public static string ConvertDBNullToStringOrNA(this object sValue, bool IsCasingRequired = false)
        {
            if (sValue == DBNull.Value)
            { return "N/A"; }
            if (!IsCasingRequired)
            {
                return Convert.ToString(sValue);
            }
            else
            {
                return myTextInfo.ToTitleCase(sValue.ToString().ToLower());
            }
        }

        public static int ConvertDBNullToInt(this object sValue)
        {
            if (((sValue != null && string.IsNullOrEmpty(sValue.ToString())) || (sValue != null && string.IsNullOrWhiteSpace(sValue.ToString()))) || sValue == DBNull.Value)
            { return 0; }
            return Convert.ToInt32(sValue);
        }

        public static long ConvertDBNullToLong(this object sValue)
        {
            if (sValue == DBNull.Value)
            { return 0; }
            return Convert.ToInt64(sValue);
        }
        public static decimal ConvertDBNullToDecimal(this object sValue)
        {
            if (sValue == DBNull.Value)
            { return 0; }
            return Convert.ToDecimal(sValue);
        }
        public static string ConvertJSONNullToString(this object sValue)
        {
            if (sValue == null)
            { return ""; }
            return sValue.ToString();
        }
        internal static double ConvertDBNullToDouble(this object sValue)
        {
            if (sValue == DBNull.Value)
            { return 0; }
            return Convert.ToDouble(sValue);
        }

        public static bool ConvertDBNullToBool(this object sValue)
        {
            if (sValue == DBNull.Value)
            { return false; }
            return Convert.ToBoolean(sValue);
        }
        public static DateTime? ConvertDBNullToDate(this object sValue)
        {
            if (sValue == "" || sValue == DBNull.Value || sValue == null || string.Equals(sValue, "NULL") || string.Equals(sValue, "null"))
            { return null; }

            return Convert.ToDateTime(sValue, System.Globalization.CultureInfo.InvariantCulture);
        }
        public static object ConvertDateTimeToDBNull(this object sValue)
        {
            if (sValue == "" || sValue == DBNull.Value || sValue == null)
            { return DBNull.Value; }
            return sValue;
        }
        //public static void HandleDeserializationError(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs errorArgs)
        //{
        //    var currentError = errorArgs.ErrorContext.Error.Message;
        //    errorArgs.ErrorContext.Handled = true;
        //}


        public static string FormatNumber(this long num)
        {
            if (num >= 100000000 || num <= -100000000)
            {
                return (num / 1000000D).ToString("0.#M");
            }
            if (num >= 1000000 || num <= -1000000)
            {
                return (num / 1000000D).ToString("0.##M");
            }
            if (num >= 100000 || num <= -1000000)
            {
                return (num / 1000D).ToString("0.#K");
            }
            if (num >= 10000 || num <= -10000)
            {
                return (num / 1000D).ToString("0.##K");
            }
            if (num >= 1000 || num <= -1000)
            {
                return (num / 1000D).ToString("0.##K");
            }
            return num.ToString("#,0");
        }

        //public static HttpContent ToHttpJsonContent(this object o)
        //{
        //    string jsonInString = JsonConvert.SerializeObject(o, Newtonsoft.Json.Formatting.Indented);
        //    HttpContent httpContent = new StringContent(jsonInString);
        //    httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        //    return httpContent;
        //}

        public static HttpContent ToHttpJsonContent(this object o)
        {
            string jsonInString = JsonConvert.SerializeObject(o, Newtonsoft.Json.Formatting.Indented);
            HttpContent httpContent = new StringContent(jsonInString);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            return httpContent;
        }

        public static double GetPercentage(int iCounter, int iTotal)
        {
            return Convert.ToDouble(iCounter) / Convert.ToDouble(iTotal);
        }
        /// <summary>
        /// Convert Current UTC Date Time to EST
        /// </summary>
        /// <returns></returns>
        public static DateTime ConvertedDateTimeUTCToESTZone(this DateTime dateTime, bool isutc = true)
        {

            DateTime today = DateTime.UtcNow;
            TimeZoneInfo easternZone = null;
            try
            {
                easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                easternZone = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
            }
            if (isutc)
            {
                today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);
            }
            else
            {
                today = TimeZoneInfo.ConvertTime(DateTime.UtcNow, easternZone);
            }

            return today;
        }
        public static string ConvertDateToStringDateOrNull(this DateTime? datetime)
        {
            if (datetime == null)
            { return "NULL"; }
            else { return "'" + datetime.Value.ToString("yyyy-MM-dd") + "'"; }
        }

        public static string ConvertPhoneSpecialCharacterToBlank(this string value)
        {
            string sMerge = "";
            if (!string.IsNullOrEmpty(value))
            {
                foreach (var chr in value.ToCharArray())
                {
                    if (Char.IsNumber(chr))
                    {
                        if (sMerge == "")
                        { sMerge = chr.ToString(); }
                        else { sMerge = sMerge + chr.ToString(); }
                    }
                }
            }

            return sMerge;
        }
        public static long ConvertStringNullToLong(this object sValue)
        {
            try
            {
                if (sValue == null || sValue.ToString() == "NULL" || sValue.ToString() == "")
                { return 0; }
                return Convert.ToInt64(sValue);

            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static string ConvertStringNullToDBNULL(this object sValue)
        {
            if (sValue == null)
            {
                return "NULL";
            }
            else if (sValue != null && (sValue.ToString().ToLower() == "null" || sValue.ToString().ToLower() == ""))
            {
                return string.Format("'{0}'", sValue);
            }
            return string.Format("'{0}'", sValue.ToString().Replace("'", "''"));
        }

        public static List<string[]> ParseCsv(MemoryStream fileName, char seperator)
        {
            var result = new List<string[]>();

            using (var reader = new StreamReader(fileName))
                foreach (var row in ReadCsvLine(reader, seperator))
                    result.Add(row);

            return result;
        }

        public static IEnumerable<string[]> ReadCsvLine(StreamReader reader, char seperator)
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();

                var values = line.Split(seperator);

                yield return values;
            }
        }


        public static string ReadUrl(this object sValue, bool IsCasingRequired = false)
        {
            if (sValue == DBNull.Value)
            { return ""; }
            if (!IsCasingRequired)
            {
                return Convert.ToString(sValue);
            }
            else
            {
                return myTextInfo.ToTitleCase(sValue.ToString().ToLower());
            }
            // return S3ObjectURLPrefix+sValue;

        }
    }
}

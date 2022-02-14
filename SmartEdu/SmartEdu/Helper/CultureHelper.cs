using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace SmartEdu.Helper
{
    public class CultureHelper
    {

        // Include ONLY cultures you are implementing as views
        private static readonly Dictionary<String, bool> _cultures = new Dictionary<string, bool> {
            {"en-US", true},  // first culture is the DEFAULT
            {"es-CL", true},
            {"ar-JO", true}
        };


        /// <summary>
        /// Returns a valid culture name based on "name" parameter. If "name" is not valid, it returns the default culture "en-US"
        /// </summary>
        /// <param name="name">Culture's name (e.g. en-US)</param>
        public static string GetValidCulture(string name)
        {
            if (string.IsNullOrEmpty(name))
                return GetDefaultCulture(); // return Default culture

            if (_cultures.ContainsKey(name))
                return name;

            // Find a close match. For example, if you have "en-US" defined and the user requests "en-GB", 
            // the function will return closes match that is "en-US" because at least the language is the same (ie English)            
            foreach (var c in _cultures.Keys)
                if (c.StartsWith(name.Substring(0, 2)))
                    return c;


            // else             
            return GetDefaultCulture(); // return Default culture as no match found
        }


        /// <summary>
        /// Returns default culture name which is the first name decalared (e.g. en-US)
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultCulture()
        {
            return _cultures.Keys.ElementAt(0); // return Default culture

        }


        /// <summary>
        ///  Returns "true" if view is implemented separatley, and "false" if not.
        ///  For example, if "es-CL" is true, then separate views must exist e.g. Index.es-cl.cshtml, About.es-cl.cshtml
        /// </summary>
        /// <param name="name">Culture's name</param>
        /// <returns></returns>
        public static bool IsViewSeparate(string name)
        {
            return _cultures[name];
        }

        /// <summary>
        /// Return Date Separator according to the culture
        /// </summary>
        /// <param name="Culture">CultureInfo</param>
        /// <returns>String Date Separator</returns>
        public static string GetDateplaceholder(CultureInfo Culture)
        {
            // bug DateSeparator is private property
            // Workarround
            string aux = Culture.DateTimeFormat.ShortDatePattern;
            string result = aux.Substring(0, 1);
            for (int i = 0; i < aux.Length; i++)
            {
                if (result != aux.Substring(i, 1))
                {
                    result = aux.Substring(i, 1);
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Return Time Separator according to the culture
        /// </summary>
        /// <param name="Culture">CultureInfo</param>
        /// <returns>String Time Separator</returns>
        public static string GetTimeplaceholder(CultureInfo Culture)
        {
            // bug TimeSeparator is private property
            // Workarround 
            string aux = Culture.DateTimeFormat.ShortTimePattern;
            string result = aux.Substring(0, 1);
            for (int i = 0; i < aux.Length; i++)
            {
                if (result != aux.Substring(i, 1))
                {
                    result = aux.Substring(i, 1);
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Return AM/PM Indicator according to the culture
        /// </summary>
        /// <param name="Culture">CultureInfo</param>
        /// <returns>Array String with symbols AM/PM</returns>
        public static string[] GetAMPMplaceholders(CultureInfo Culture)
        {
            List<string> result = new List<string>();
            result.Add(Culture.DateTimeFormat.AMDesignator);
            result.Add(Culture.DateTimeFormat.PMDesignator);
            return result.ToArray();
        }

        /// <summary>
        /// Return AM Indicator according to the culture
        /// </summary>
        /// <param name="Culture">CultureInfo</param>
        /// <returns>Array String with symbols AM/PM</returns>
        public static string GetAMplaceholder(CultureInfo Culture)
        {
            return Culture.DateTimeFormat.AMDesignator;
        }

        /// <summary>
        /// Return PM Indicator according to the culture
        /// </summary>
        /// <param name="Culture">CultureInfo</param>
        /// <returns>Array String with symbols AM/PM</returns>
        public static string GetPMplaceholder(CultureInfo Culture)
        {
            return Culture.DateTimeFormat.PMDesignator;
        }

        /// <summary>
        /// Return Decimal Separator according to the culture
        /// </summary>
        /// <param name="Culture">CultureInfo</param>
        /// <returns>String decimal Separator</returns>
        public static string GetDecimalplaceholder(CultureInfo Culture)
        {
            return Culture.NumberFormat.NumberDecimalSeparator;
        }

        /// <summary>
        /// Return Thousands Separator according to the culture
        /// </summary>
        /// <param name="Culture">CultureInfo</param>
        /// <returns>String Thousand Separator</returns>
        public static string GetThousandsplaceholder(CultureInfo Culture)
        {
            return Culture.NumberFormat.NumberGroupSeparator;
        }

        /// <summary>
        /// Return Postive Symbol according to the culture
        /// </summary>
        /// <param name="Culture">CultureInfo</param>
        /// <returns>String Postive Symbol</returns>
        public static string GetPositiveSignal(CultureInfo Culture)
        {
            return Culture.NumberFormat.PositiveSign;
        }

        /// <summary>
        /// Return Negative Symbol according to the culture
        /// </summary>
        /// <param name="Culture">CultureInfo</param>
        /// <returns>String Negative Symbol</returns>
        public static string GetNegativeSignal(CultureInfo Culture)
        {
            return Culture.NumberFormat.NegativeSign;
        }

        /// <summary>
        /// Return Negative Symbol according to the culture
        /// </summary>
        /// <param name="Culture">CultureInfo</param>
        /// <returns>String Negative Symbol</returns>
        public static string GetPercentSymbol(CultureInfo Culture)
        {
            return Culture.NumberFormat.PercentSymbol;
        }

        /// <summary>
        /// Return Currency Symbol according to the culture
        /// </summary>
        /// <param name="Culture">CultureInfo</param>
        /// <returns>String Currency Symbol</returns>
        public static string GetCurrencySymbol(CultureInfo Culture)
        {
            return Culture.NumberFormat.CurrencySymbol;
        }
    }
}
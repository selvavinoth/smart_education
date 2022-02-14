using System;
using System.Collections.Generic;
using System.Web;
using SmartEdu.Data.Models;

namespace SmartEdu.SessionHolder
{
    public static class SessionPersistor
    {
        static string username = "UserName";
        static string userFullName = "UserFullName";
        static string userTitle = "UserTitle";
        static string userId = "UserId";
        static string collegeId = "CollegeId";
        static string DepartmentId = "DpId";
        static string selectedCollegeId = "SelectedCollegeId";
        static string selectedDepartmentId = "SelectedDpId";
        static string tempId = "TempId";
        static string positionId = "UserPositionId";
        static string tempKeyValuePair = "TempKeyValuePair";
        static string tempMapKeyValuePair = "TempMAPKeyValuePair";
        static string dateformat = "dd-MM-yyyy";
        static string timeformat = "hh:mm tt";
        static string theme = "default";
        static string themeId = "UI_THEME_ID";
        static string resdll = "Resources";  // default Resource
        static string collegeName = "FX";
        static string dpName = "IT";
        static string logoUrl = "ClientLogoUrl";
        static string positionlevelId = "PositionLevelId";
        static string FieldValidationModel = "FieldConfigModel";
        static string vmName = "ViewModelName";

        /// <summary>
        /// this property holds the current viewmodel name 
        /// </summary>
        public static string ViewModelName
        {

            get
            {
                if (HttpContext.Current == null) return null;
                if (HttpContext.Current.Session[vmName] != null)
                    return GetObjectFromSession(vmName) as string;
                return null;
            }
            set
            {
                if (value == null)
                    ClearItemFromSession(vmName);
                SetItemInSession(value, vmName);

            }
        }
        /// <summary>
        /// this property holds the current viewmodel related field level config record.
        /// </summary>
        public static Dictionary<string,Dictionary<string,string>> FieldLevelConfig
        {

            get
            {
                if (HttpContext.Current == null) return null;
                if (HttpContext.Current.Session[FieldValidationModel] != null)
                    return GetObjectFromSession(FieldValidationModel) as Dictionary<string, Dictionary<string, string>>;
                return null;
            }
            set
            {
                if (value == null)
                    ClearItemFromSession(FieldValidationModel);
                SetItemInSession(value, FieldValidationModel);

            }
        }

        /// <summary>
        /// this property holds the login username of the logged-in user 
        /// </summary>
        public static string UserName
        {

            get
            {
                if (HttpContext.Current == null) return null;
                if (HttpContext.Current.Session[username] != null)
                    return GetObjectFromSession(username) as string;
                return null;
            }
            set
            {
                if (value == null)
                    ClearItemFromSession(username);
                SetItemInSession(value, username);

            }
        }
        /// <summary>
        /// This Property holds the salutation (Mr/Ms/Mrs) of the logged-in user
        /// </summary>
        public static string UserTitle
        {
            get
            {
                if (HttpContext.Current == null) return null;
                if (HttpContext.Current.Session[userTitle] != null)
                    return GetObjectFromSession(userTitle) as string;
                return null;
            }
            set
            {
                if (value == null)
                    ClearItemFromSession(userTitle);
                SetItemInSession(value, userTitle);

            }
        }
        /// <summary>
        /// This property holds the UserId of the logged-in user.
        /// </summary>
        public static long UserId
        {
            get
            {


                if (HttpContext.Current == null) return 0;
                if (HttpContext.Current.Session[userId] != null)
                    return Convert.ToInt64(GetObjectFromSession(userId));
                return 0;
            }
            set
            {

                SetItemInSession(value, userId);

            }
        }

        /// <summary>
        /// This property holds Tenant Id (Company ID) of the logged-in User.
        /// </summary>
        public static int CollegeId
        {
            get
            {
                if (HttpContext.Current == null) return 0;
                if (HttpContext.Current.Session[collegeId] != null)
                    return Convert.ToInt32(GetObjectFromSession(collegeId));
                return 0;
            }
            set
            {

                SetItemInSession(value, collegeId);

            }
        }
        /// <summary>
        ///  This Property holds the Business Unit Id of the logged-in user.
        /// </summary>
        public static int DPID
        {
            get
            {
                if (HttpContext.Current == null) return 0;
                if (HttpContext.Current.Session[DepartmentId] != null)
                    return Convert.ToInt32(GetObjectFromSession(DepartmentId));
                return 0;
            }
            set
            {
                SetItemInSession(value, DepartmentId);
            }
        }
        public static int SelectedDPID
        {
            get
            {
                if (HttpContext.Current == null) return 0;
                if (HttpContext.Current.Session[selectedDepartmentId] != null)
                    return Convert.ToInt32(GetObjectFromSession(selectedDepartmentId));
                return 0;
            }
            set { SetItemInSession(value, selectedDepartmentId); }
        }
        public static int SelectedCollegeID
        {
            get
            {
                if (HttpContext.Current == null) return 0;
                if (HttpContext.Current.Session[selectedCollegeId] != null)
                    return Convert.ToInt32(GetObjectFromSession(selectedCollegeId));
                return 0;
            }
            set { SetItemInSession(value, selectedCollegeId); }
        }
        /// <summary>
        /// This Property holds the complete name of the logged-in user.
        /// </summary>
        public static string UserFullName
        {
            get
            {
                if (HttpContext.Current == null) return null;
                if (HttpContext.Current.Session[userFullName] != null)
                    return GetObjectFromSession(userFullName) as string;
                return null;
            }
            set
            {
                if (value == null)
                    ClearItemFromSession(userFullName);
                SetItemInSession(value, userFullName);

            }
        }
        /// <summary>
        /// The property holds the Position ID ex: MR Position Id, ABM Position Id of the logged-in user.
        /// </summary>
        public static long PositionId
        {
            get
            {
                if (HttpContext.Current == null) return 0;
                if (HttpContext.Current.Session[positionId] != null)
                    return Convert.ToInt64(GetObjectFromSession(positionId));
                return 0;
            }
            set
            {

                SetItemInSession(value, positionId);

            }
        }

        public static string TempId
        {
            get
            {
                if (HttpContext.Current == null) return null;
                if (HttpContext.Current.Session[tempId] != null)
                    return GetObjectFromSession(tempId) as string;
                return null;
            }
            set
            {
                if (value == null)
                    ClearItemFromSession(tempId);
                SetItemInSession(value, tempId);

            }
        }

        public static void setTempValue(string key, object value)
        {
            if (HttpContext.Current != null)
            {
                Dictionary<string, object> obj = new Dictionary<string, object>();
                if (HttpContext.Current.Session[tempKeyValuePair] != null)
                {
                    obj = GetObjectFromSession(tempKeyValuePair) as Dictionary<string, object>;
                    if (obj == null) { obj = new Dictionary<string, object>(); }
                    obj[key] = value;
                }
                else
                {
                    obj = new Dictionary<string, object>();
                    obj[key] = value;
                }
                SetItemInSession(obj, tempKeyValuePair);
            }
        }
        public static object getTempValue(string key)
        {
            object rvalue = null;
            if (HttpContext.Current == null) { return null; }
            Dictionary<string, object> obj = new Dictionary<string, object>();
            if (HttpContext.Current.Session[tempKeyValuePair] != null)
            {
                obj = GetObjectFromSession(tempKeyValuePair) as Dictionary<string, object>;
                if (obj != null && obj.ContainsKey(key))
                    rvalue = obj[key];

            }
            return rvalue;
        }
        public static void removeTempValue(string key)
        {
            if (HttpContext.Current != null)
            {
                Dictionary<string, object> obj = new Dictionary<string, object>();
                if (HttpContext.Current.Session[tempKeyValuePair] != null)
                {
                    obj = GetObjectFromSession(tempKeyValuePair) as Dictionary<string, object>;
                    if (obj != null && obj.ContainsKey(key))
                    {
                        obj.Remove(key);
                        SetItemInSession(obj, tempKeyValuePair);
                    }
                }
            }
        }
        public static void setTempMapNull()
        {
            ClearItemFromSession(tempKeyValuePair);
        }

        public static void setTempValue(string screen, string key, object value)
        {
            Dictionary<string, Dictionary<string, object>> obj = new Dictionary<string, Dictionary<string, object>>();
            Dictionary<string, object> childObj = new Dictionary<string, object>();
            try
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Session[tempMapKeyValuePair] != null)
                    {
                        obj = GetObjectFromSession(tempMapKeyValuePair) as Dictionary<string, Dictionary<string, object>>;
                        if (obj == null) { obj = new Dictionary<string, Dictionary<string, object>>(); }
                        childObj = (obj.ContainsKey(screen) ? obj[screen] : new Dictionary<string, object>());
                        childObj[key] = value;
                        obj[screen] = childObj;
                    }
                    else
                    {
                        obj = new Dictionary<string, Dictionary<string, object>>();
                        childObj[key] = value;
                        obj[screen] = childObj;
                    }
                    SetItemInSession(obj, tempMapKeyValuePair);
                }
            }
            finally
            {
                obj = null;
                childObj = null;
            }
        }
        public static object getTempValue(string screen, string key)
        {
            object rvalue = null;
            if (HttpContext.Current == null) { return null; }
            Dictionary<string, Dictionary<string, object>> obj = new Dictionary<string, Dictionary<string, object>>();
            Dictionary<string, object> childObj = new Dictionary<string, object>();
            try
            {
                if (HttpContext.Current.Session[tempMapKeyValuePair] != null)
                {
                    obj = GetObjectFromSession(tempMapKeyValuePair) as Dictionary<string, Dictionary<string, object>>;
                    if (obj != null && obj.ContainsKey(screen) && obj[screen].ContainsKey(key)) { rvalue = obj[screen][key]; }
                }
            }
            finally { obj = null; childObj = null; }
            return rvalue;
        }
        public static void removeTempValue(string screen, string key)
        {
            if (HttpContext.Current != null)
            {
                Dictionary<string, Dictionary<string, object>> obj = new Dictionary<string, Dictionary<string, object>>();
                Dictionary<string, object> childObj = new Dictionary<string, object>();
                try
                {
                    if (HttpContext.Current.Session[tempMapKeyValuePair] != null)
                    {
                        obj = GetObjectFromSession(tempMapKeyValuePair) as Dictionary<string, Dictionary<string, object>>;
                        if (obj != null && obj.ContainsKey(screen) && obj[screen].ContainsKey(key))
                        {
                            obj[screen].Remove(key);
                            SetItemInSession(obj, tempMapKeyValuePair);
                        }
                    }
                }
                finally { obj = null; childObj = null; }
            }
        }
        public static void setTempMapNull(string screen)
        {
            if (HttpContext.Current != null)
            {
                Dictionary<string, Dictionary<string, object>> obj = new Dictionary<string, Dictionary<string, object>>();
                try
                {
                    if (HttpContext.Current.Session[tempMapKeyValuePair] != null)
                    {
                        obj = GetObjectFromSession(tempMapKeyValuePair) as Dictionary<string, Dictionary<string, object>>;
                        if (obj != null && obj.ContainsKey(screen))
                        {
                            obj.Remove(screen);
                            SetItemInSession(obj, tempMapKeyValuePair);
                        }
                    }
                }
                finally { obj = null; }
            }
        }

        /// <summary>
        /// It removes all objects stored in session and nullify the http user identity.
        /// </summary>
        public static void invalidateSession()
        {
            HttpContext.Current.Session.RemoveAll();
            // HttpContext.Current.Session.Abandon();

            HttpContext.Current.User = null;
        }
        /// <summary>
        /// It retrieves the object stored in corresponding session key.
        /// </summary>
        /// <param name="key">it refers the session key</param>
        /// <returns> matched session object stored in the key. returns null if the key is invalid or not exist</returns>
        public static string GetStringFromSession(string key)
        {
            return GetObjectFromSession(key).ToString();
        }
        /// <summary>
        /// It retrieves the object stored in corresponding session key.
        /// </summary>
        /// <param name="key">it refers the session key</param>
        /// <returns> matched session object stored in the key. returns null if the key is invalid or not exist</returns>

        public static object GetObjectFromSession(string key)
        {
            return HttpContext.Current.Session[key];
        }
        /// <summary>
        /// It stores object in the session and will be refered under the given key.
        /// </summary>
        /// <param name="item"> the supposed object to be stored in the session</param>
        /// <param name="key"> the reference key of the stored session object</param>
        public static void SetItemInSession(object item, string key)
        {
            HttpContext.Current.Session.Add(key, item);
        }
        /// <summary>
        /// It removes the session entry of the object refered under the given key.
        /// </summary>
        /// <param name="key"></param>
        public static void ClearItemFromSession(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }
        /// <summary>
        /// It property holds the tenant/Business unit date format in the session
        /// </summary>
        public static string DateFormat
        {

            get
            {
                if (HttpContext.Current == null) return null;
                if (HttpContext.Current.Session[dateformat] != null)
                    return GetObjectFromSession(dateformat) as string;
                return null;
            }
            set
            {
                if (value == null)
                    ClearItemFromSession(dateformat);
                SetItemInSession(value, dateformat);

            }
        }
        /// <summary>
        /// This property holds the Tenant / Business Unit 's Time format  in the session.
        /// </summary>
        public static string TimeFormat
        {

            get
            {
                if (HttpContext.Current == null) return null;
                if (HttpContext.Current.Session[timeformat] != null)
                    return GetObjectFromSession(timeformat) as string;
                return null;
            }
            set
            {
                if (value == null)
                    ClearItemFromSession(timeformat);
                SetItemInSession(value, timeformat);

            }
        }
        /// <summary>
        /// This property hold current theme assigned to the business unit user.
        /// </summary>
        public static string Theme
        {

            get
            {
                if (HttpContext.Current == null) return null;
                if (HttpContext.Current.Session[theme] != null)
                    return GetObjectFromSession(theme) as string;
                return null;
            }
            set
            {
                if (value == null)
                    ClearItemFromSession(theme);
                SetItemInSession(value, theme);

            }
        }
        /// <summary>
        /// This property hold current theme assigned to the business unit user.
        /// </summary>
        public static int ThemeId
        {

            get
            {
                if (HttpContext.Current == null) return 0;
                if (HttpContext.Current.Session[themeId] != null)
                    return Convert.ToInt32(GetObjectFromSession(themeId));
                return 0;
            }
            set
            {
                if (value == null)
                    ClearItemFromSession(themeId);
                SetItemInSession(value, themeId);
            }
        }
        /// <summary>
        /// This property holds the resource dll name for Business Unit in the session. returns Ivyresources if resource not found/created.
        /// </summary>
        public static string ResourceDLL
        {

            get
            {
                if (HttpContext.Current == null) return resdll;
                if (HttpContext.Current.Session[resdll] != null)
                    return GetObjectFromSession(resdll) as string;
                return resdll;
            }
            set
            {
                if (value == null)
                    ClearItemFromSession(resdll);
                SetItemInSession(value, resdll);

            }
        }
        /// <summary>
        /// It holds the Tenant name (Company) of the logged-in user in the session.
        /// </summary>
        public static string CollegeName
        {

            get
            {
                if (HttpContext.Current == null) return collegeName;
                if (HttpContext.Current.Session[collegeName] != null)
                    return GetObjectFromSession(collegeName) as string;
                return collegeName;
            }
            set
            {
                if (value == null)
                    ClearItemFromSession(collegeName);
                SetItemInSession(value, collegeName);

            }
        }

        /// <summary>
        /// It holds the Business Unit Name of the logged-in user in the session.
        /// </summary>
        public static string DPName
        {

            get
            {
                if (HttpContext.Current == null) return dpName;
                if (HttpContext.Current.Session[dpName] != null)
                    return GetObjectFromSession(dpName) as string;
                return dpName;
            }
            set
            {
                if (value == null)
                    ClearItemFromSession(dpName);
                SetItemInSession(value, dpName);

            }
        }
        public static string ClientLogoUrl
        {
            get
            {
                if (HttpContext.Current == null) return logoUrl;
                if (HttpContext.Current.Session[logoUrl] != null)
                    return GetObjectFromSession(logoUrl) as string;
                return logoUrl;
            }
            set
            {
                if (value == null)
                    ClearItemFromSession(logoUrl);
                SetItemInSession(value, logoUrl);

            }
        }
        public static int PositionLevelId
        {
            get
            {
                if (HttpContext.Current == null) return 0;
                if (HttpContext.Current.Session[positionlevelId] != null)
                    return Convert.ToInt32(GetObjectFromSession(positionlevelId));
                return 0;
            }
            set
            {
                if (value == null)
                    ClearItemFromSession(positionlevelId);
                SetItemInSession(value, positionlevelId);

            }
        }
    }
}
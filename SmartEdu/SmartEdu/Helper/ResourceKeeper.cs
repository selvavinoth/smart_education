using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using SmartEdu.SessionHolder;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;

namespace SmartEdu.Helper
{
    /// <summary>
    /// It has static methods for miscellaneous operations being used in the application.
    /// </summary>
    public class ResourceKeeper
    {
        /// <summary>
        /// return hash code (MD5) for the input string
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>MD5 hash value</returns>
        public static string GetHash(string value)
        {
            return Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(new UTF8Encoding().GetBytes(value)));
        }

        /// <summary>
        /// returns random password of given length.
        /// </summary>
        /// <param name="length">password's length</param>
        /// <returns>dynamic password string</returns>
        //public static string GetRandomPassword(int length)
        //{
        //    char[] chars = DataKeeper.passwordSet.ToCharArray();
        //    string password = string.Empty;
        //    Random random = new Random();

        //    for (int i = 0; i < length; i++)
        //    {
        //        int x = random.Next(1, chars.Length);
        //        //Don't Allow Repetation of Characters
        //        if (!password.Contains(chars.GetValue(x).ToString()))
        //            password += chars.GetValue(x);
        //        else
        //            i--;
        //    }
        //    return password;
        //}

        /// <summary>
        /// returns value assigned to the given resourceName taken from resource file.
        /// </summary>
        /// <param name="resourceName">Resource string</param>
        /// <returns>value of the input resourceName</returns>
        public static string GetResource(string resourceName)
        {
            Assembly assembly = null;
            ResourceManager rm = null;
            Object obj = null;
            try
            {
                // Object obj =HttpContext.GetGlobalResourceObject(SessionPersister.ResourceDLL, resourceName);
                // obtain the resource dll reference from assembly
                var rName = WebConfigurationManager.AppSettings["ResourceName"].ToString();
                assembly = Assembly.Load(rName);
                rm = new ResourceManager(rName + ".Resources", assembly);
                // get the value assigned to the resourcename
                obj = rm.GetString(resourceName);
                return (obj == null ? "" : obj.ToString());
            }
            finally {
                assembly = null;
                rm = null;
                obj = null;
            }
        }

        /// <summary>
        /// returns audit related details extracted from session.
        /// </summary>
        /// <returns></returns>
        //public static AuditDetails getAuditDetails()
        //{
        //    AuditDetails auditDet = new AuditDetails();
        //    auditDet.BU_Id = SessionPersister.BuId;
        //    auditDet.Tenant_Id = SessionPersister.TenantId;
        //    auditDet.Position_Id = SessionPersister.PositionId;
        //    auditDet.UserName = SessionPersister.UserName;
        //    auditDet.User_Id = SessionPersister.UserId;
        //    return auditDet;
        //}

    }
}
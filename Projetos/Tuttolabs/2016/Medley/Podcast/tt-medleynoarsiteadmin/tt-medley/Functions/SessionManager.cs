using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tt_medley.Functions
{
    public static class SessionManager
    {
        public static void RegisterSession(string key, object obj)
        {
            System.Web.HttpContext.Current.Session[key] = obj;
        }

        public static void FreeSession(string key)
        {
            System.Web.HttpContext.Current.Session[key] = null;
        }
        
        public static bool CheckSession(string key)
        {
            if (System.Web.HttpContext.Current.Session[key] != null)
                return true;
            else
                return false;
        }

        public static bool CheckSession(string key, System.Web.HttpContextBase contexto)
        {
            if (contexto.Session[key] != null)
                return true;
            else
                return false;
        }

        public static object ReturnSessionObject(string key)
        {
            if (CheckSession(key))
                return System.Web.HttpContext.Current.Session[key];
            else
                return null;
        }

        public static object ReturnSessionObject(string key, System.Web.HttpContextBase contexto)
        {
            if (CheckSession(key, contexto))
                return contexto.Session[key];
            else
                return null;
        }

        public static bool SlowEquals(string a, string b)
        {
            var diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }

    }
}
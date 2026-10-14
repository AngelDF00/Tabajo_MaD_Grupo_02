using System;
using System.Web;
using System.Web.Security;


namespace Es.Udc.DotNet.MiniPortal.Web.HTTP.Util
{
    public class CookiesManager
    {
        private const string LOGIN_NAME_COOKIE = "loginName";
        private const string ENCRYPTED_PASSWORD_COOKIE = "encryptedPassword";

        private const int REMEMBER_MY_PASSWORD_AGE = 30 * 24 * 3600;
        private const int COOKIES_TIME_TO_LIVE_REMOVE = 0;

        public static void LeaveCookies(HttpContext context, String loginName,
                String encryptedPassword)
        {
            int timeToLive = REMEMBER_MY_PASSWORD_AGE;

            /* Create the loginName cookie. */
            HttpCookie loginNameCookie =
                new HttpCookie(LOGIN_NAME_COOKIE, loginName);

            /* Create the encryptedPassword cookie. */
            HttpCookie encryptedPasswordCookie =
                new HttpCookie(ENCRYPTED_PASSWORD_COOKIE, encryptedPassword);


            /* Set maximum age to cookies. */
            loginNameCookie.Expires = DateTime.Now.AddSeconds(timeToLive);
            encryptedPasswordCookie.Expires = DateTime.Now.AddSeconds(timeToLive);

            /* Add cookies to response (These values will be used in TouchSession method to create
               the authCookie when the session starts. */
            context.Response.Cookies.Add(loginNameCookie);
            context.Response.Cookies.Add(encryptedPasswordCookie);           

        }

        public static void RemoveCookies(HttpContext context)
        {
            int timeToLive = COOKIES_TIME_TO_LIVE_REMOVE;

            /* Create the loginName cookie. */
            HttpCookie loginNameCookie =
                new HttpCookie(LOGIN_NAME_COOKIE, "");

            /* Create the encryptedPassword cookie. */
            HttpCookie encryptedPasswordCookie =
                new HttpCookie(ENCRYPTED_PASSWORD_COOKIE, "");

            /* Set maximum age to cookies. */
            loginNameCookie.Expires = DateTime.Now.AddSeconds(timeToLive);
            encryptedPasswordCookie.Expires = DateTime.Now.AddSeconds(timeToLive);

            /* Add cookies to response. */
            context.Response.Cookies.Add(loginNameCookie);
            context.Response.Cookies.Add(encryptedPasswordCookie);
            
        }

        public static String GetLoginName(HttpContext context)
        {
            HttpCookie loginNameCookie =
                context.Request.Cookies.Get(LOGIN_NAME_COOKIE);

            if (loginNameCookie != null)
                return loginNameCookie.Value;
            else
                return null;

        }

        public static String GetEncryptedPassword(HttpContext context)
        {
            HttpCookie encryptedPasswordCookie =
                context.Request.Cookies.Get(ENCRYPTED_PASSWORD_COOKIE);

            if (encryptedPasswordCookie != null)
                return encryptedPasswordCookie.Value;
            else
                return null;
        }

    }
}

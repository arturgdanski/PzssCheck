using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

using PzssLib.Exceptions;

namespace PzssLib
{
    public class LicenseProvider
    {
        private string m_pzssLoginPage;
        private string m_pzssLicensesListPage;
        private string m_pzssCorrectSignIn;

        private CookieContainer m_cookieContainer;

        public LicenseProvider()
        {
            ServicePointManager.ServerCertificateValidationCallback =
                new System.Net.Security.RemoteCertificateValidationCallback(
                    (sender, cert, chain, ssl) =>
                        {
                            return true;
                        });

            m_pzssLoginPage = Persistent.PzssLoginPage;
            m_pzssLicensesListPage = Persistent.PzssLicenseListPage;
            m_pzssCorrectSignIn = Persistent.PzssPortalCorrectSignInAbsUrl;
        }

        public string GetAllLicenses(Credentials credentials)
        {
            string ret = "";
            using (var loginResponse = ProcessSyncHttpLoginRequest(credentials, m_pzssLoginPage))
            {
                if(loginResponse != null)
                    if (loginResponse.ResponseUri.AbsolutePath == m_pzssCorrectSignIn)
                    {
                        using(var licensesResponse = ProcessSyncHttpWebRequest(m_pzssLicensesListPage))
                        {
                            ret = ReadWebResponse(licensesResponse);
                        }
                    }
                    else
                    {
                        throw new LicenseProviderException("Unable to sign in");
                    }
            }
            return ret;
        }

        private WebResponse ProcessSyncHttpLoginRequest(Credentials credentials, string url)
        {
            string postData = "";
            try
            {
                postData = String.Format("Login={0}&Password={1}", credentials.Login, credentials.Password);
            }
            catch (CryptographicException ex)
            {
                throw new LicenseProviderException("Cryptographic exception", ex);
            }
            catch (Exception ex)
            {
                throw new LicenseProviderException("Unrecognized exception", ex);
            }

            byte[] byteData = Encoding.ASCII.GetBytes(postData);

            m_cookieContainer = new CookieContainer();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.CookieContainer = m_cookieContainer;
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteData.Length;
            
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteData, 0, byteData.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            
            return response;
        }

        private WebResponse ProcessSyncHttpWebRequest(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = 0;
            request.CookieContainer = m_cookieContainer;

            WebResponse response = request.GetResponse();
            
            return response;
        }

        private string ReadWebResponse(WebResponse response)
        {
            string ret = "";
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    ret = reader.ReadToEnd();
                }
            }
            return ret;
        }
        
    }
}

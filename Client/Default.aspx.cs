using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Client
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback((a, b, c, d) => true);

            // Create a request using a URL that can receive a post. 
            //WebRequest request = WebRequest.Create("https://localhost:44300/login");

            //RESTSHARP!

            var restclient = new RestClient("https://localhost:44300");
            restclient.Authenticator = new SimpleAuthenticator("userName", "mathieu", "password", "asd");

            var restrequest = new RestRequest("/login", Method.POST);
            restrequest.AddHeader("Accept", "*/*");

            var restresponse = restclient.Execute(restrequest);

            var responseCookies = restresponse.Cookies;



            restclient.AddDefaultHeader("Client", "RichClient");
            restclient.Authenticator = null;

            var secureRequest = new RestRequest("/secure");
            //secureRequest.AddCookie(responseCookies[0].Name, responseCookies[0].Value);

            var secureResponse = restclient.Execute(secureRequest);


            




            CookieContainer cookieJar = new CookieContainer();

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://localhost:44300/login");
            request.CookieContainer = cookieJar;
            request.Headers.Add("Client", "RichClient");
            // Set the Method property of the request to POST.
            request.Method = "POST";
            // Create POST data and convert it to a byte array.
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("userName", "mathieu");
            nvc.Add("password", "abc");
            var values = nvc.Cast<string>().Select(x => string.Format("{0}={1}", x, HttpUtility.UrlEncode(nvc[x]))).ToArray();
            var postValues = string.Join("&", values);
            byte[] byteArray = Encoding.UTF8.GetBytes(postValues);
            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            //WebResponse response = request.GetResponse();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            int cookieCount = cookieJar.Count;
            var cookies = cookieJar.GetCookies(new Uri("http://localhost"));
            var cookieValue = cookies.Cast<Cookie>().First().Value;
            

            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            Console.WriteLine(responseFromServer);
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();


            OtherRequest(cookieJar);
        }

        private void OtherRequest(CookieContainer cookieJar)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://localhost:44300/secure");
            request.AllowAutoRedirect = true;
            //request.CookieContainer = cookieJar;
            request.Headers.Add("Client", "RichClient");

            // Set the Method property of the request to POST.
            request.Method = "GET";
            
            // Set the ContentType property of the WebRequest.
            request.ContentType = "text/html";

            
            // Get the response.
            //WebResponse response = request.GetResponse();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            var dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            Console.WriteLine(responseFromServer);
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();
        }
    }
}
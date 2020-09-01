using System;
using System.Net;
using System.Net.Http;
using System.Resources;

namespace MiniWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] test = new string[1];
            test[0] = "http://localhost:8080/";

            SimpleListenerExample(test);
        }

        public static string ReturnIndexHTML()
        {
            string path = @"C:\Users\willi\Dev\webserver2-grupp4\Content\index.html";
            string text = System.IO.File.ReadAllText(path);

            return text;
        }

        public static void SimpleListenerExample(string[] prefixes)
        {
            CookieCollection cookies = new CookieCollection();
            Cookie cookieTime = new Cookie();

            cookieTime.Expires.Add(new TimeSpan(0, 1, 0));
            cookieTime.Value = DateTime.Now.ToString();
            
            cookies.Add(cookieTime);
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");

                return;
            }

            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            HttpListener listener = new HttpListener();
            foreach (var s in prefixes)
            {
                listener.Prefixes.Add(s);
            }
            listener.Start();
            Console.WriteLine("Listening...");
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            response.Cookies.Add(cookieTime);
            cookies.Add(response.Cookies);
            
            string responseString = ReturnIndexHTML() + cookies.Count;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

            
            response.ContentLength64 = buffer.Length;
            
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);

            output.Close();
            listener.Stop();


        }
    }
}

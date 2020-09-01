using System;
using System.IO;
using System.Net;

namespace MiniWebServer
{
    class Program
    {
        public string Value { get; set; }
        static void Main(string[] args)
        {
            string[] test = new string[1];
            test[0] = "http://localhost:8080/";

            SimpleListenerExample(test);
        }

        public static string NextCustomerID()
{
    // A real-world application would do something more robust
    // to ensure uniqueness.
    return DateTime.Now.ToString();
}
        public static string ReturnIndexHTML()
        {
            string path = @"C:\Users\willi\Dev\webserver2-grupp4\Content\index.html";
            string text = System.IO.File.ReadAllText(path);

            return text;
        }

        public static void SimpleListenerExample(string[] prefixes)
        {
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
            string customerID = null;

            Cookie cookie = request.Cookies["ID"];
            if (cookie != null)
                customerID = cookie.Value;
            if (customerID != null)
                Console.WriteLine("Found the Cookie!");

            HttpListenerResponse response = context.Response;

            if(customerID == null)
            {
                customerID = NextCustomerID();
                Cookie cook = new Cookie("ID", customerID);
                response.AppendCookie(cook);
            }

            string responseString = ReturnIndexHTML() + customerID;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);

            output.Close();
            listener.Stop();
        }
    }
}

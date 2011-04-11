using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    class EventArgs<T> : EventArgs
    {
        public T Result { get; set; }
    }

    class FooClient
    {
        public event EventHandler<EventArgs<string>> GetBarCompleted;
        protected void OnGetBarCompleted(EventArgs<string> args)
        {
            if (GetBarCompleted != null)
                GetBarCompleted(this, args);
        }

        public void GetBarAsync()
        {
            var client = new WebClient {Encoding = Encoding.GetEncoding("utf-8")};
            client.DownloadStringCompleted += (s, e) => 
                OnGetBarCompleted(new EventArgs<string> {Result = e.Result});

            client.DownloadStringAsync(new Uri("http://www.google.com"));
        }
    }
}

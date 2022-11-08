using System;
using System.IO;
using System.Net;
using static Agregator.GeneralQueue;

namespace Agregator
{
    public class Server
    {
        public int countd = 0;
        public int countc = 0;
        public int Port = 8100;

        private HttpListener _listener;

        public void Start()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://localhost:" + Port.ToString() + "/");
            _listener.Start();
            Receive();
        }

        public void Stop()
        {
            _listener.Stop();
        }

        private void Receive()
        {
            _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
        }

        private void ListenerCallback(IAsyncResult result)
        {
            if (_listener.IsListening)
            {
                var context = _listener.EndGetContext(result);
                var request = context.Request;
                Console.WriteLine($"{request.HttpMethod} {request.RawUrl}");
                

                // do something with the request
                
                if (request.HasEntityBody)
                {
                    var body = request.InputStream;
                    var encoding = request.ContentEncoding;
                    var reader = new StreamReader(body, encoding);
                    string s = reader.ReadToEnd();
                    if (request.RawUrl == "/dinninghall" && (s.Length != null || s.Length != 0))
                    {
                        _queue_dinning_hall.Enqueue(s);
                        countd++;
                        Console.WriteLine("Success" + " " + countd + " dinning hall");
                    }

                    else if(request.RawUrl == "/kitchen" && (s.Length != null || s.Length != 0))
                    {
                        _queue_kitchen.Enqueue(s);
                        countc++;
                        Console.WriteLine("Success" + " " + countc + " kitchen");
                    }
                        
                }

                Receive();
            }
        }
    }
}

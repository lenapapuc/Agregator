using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace Agregator
{
    
    public class Threads
    
    {
        private static Mutex mut = new Mutex();
        public void Extractor()
        {
          
           //Thread.Sleep(1000);
           string str = string.Empty;
            
            while (true)
            {
                mut.WaitOne();
                if (GeneralQueue._queue_dinning_hall.Count > 0)
                {
                    
                    str = GeneralQueue._queue_dinning_hall.Dequeue();
                    //if (string.IsNullOrEmpty(str)) continue ;
                    
                }
                mut.ReleaseMutex();
                using var client = new HttpClient();
                //var json = JsonConvert.SerializeObject(str);
                if (str != null)
                {
                    var data = new StringContent(str, Encoding.UTF8, "application/json");
                    client.PostAsync("http://localhost:8090/", data);
                }

                Thread.Sleep(1000);
                
            }
        }
        public void Receiver()
        {
            //Thread.Sleep(1000);
            string str = string.Empty;
            
            while (true)
            {
                mut.WaitOne();
                if (GeneralQueue._queue_kitchen.Count > 0)
                {
                    
                    str = GeneralQueue._queue_kitchen.Dequeue();

                }
                mut.ReleaseMutex();
                using var client = new HttpClient();
               
                if (str != null)
                {
                    var data = new StringContent(str, Encoding.UTF8, "application/json");
                    client.PostAsync("http://localhost:8080/", data);
                }

                Thread.Sleep(1000);
                
            }
        }

        public List<Thread> ExtractThreads()
        {
            int thread_extractors = 3;
            List<Thread> list = new List<Thread>();
            for (int i = 0; i < thread_extractors; i++)
            {
                Thread thread = new Thread(Extractor);
                list.Add(thread);
            }
            return list;
        }

        public List<Thread> ReceiverThreads()
        {
            int thread_receivers = 3;
            List<Thread> list = new List<Thread>();
            for (int i = 0; i < thread_receivers; i++)
            {
                Thread thread = new Thread(Receiver);
                list.Add(thread);
            }
            return list;
        }
    }
}
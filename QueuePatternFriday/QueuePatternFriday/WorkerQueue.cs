using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QueuePatternFriday
{
    class WorkerQueue
    {
        //System.Collections.Concurrent.ConcurrentQueue<Action> m_queue = 
        //new System.Collections.Concurrent.ConcurrentQueue<Action>();

        private Queue<Action> m_queue = new Queue<Action>();

        List<Thread> m_workers;

        private object key = new object();

        public int Count
        {
            get
            {
                return m_queue.Count;
            }
        }

        public WorkerQueue(int num_workers)
        {
            m_workers = new List<Thread>(num_workers);
            for (int i = 0; i < num_workers; i++)
            {
                Thread one_thread = new Thread(PendingConsume);
                m_workers.Add(one_thread);
                m_workers[i].Start();
            }
        }

        private void PendingConsume()
        {
            while (true)
            {
                Action action = null;
                while (m_queue.Count == 0)
                {
                    Thread.Sleep(10);
                }
                lock (key)
                {
                    //if (m_queue.TryDequeue(out Action action))
                    //action.Invoke();
                    if (m_queue.Count > 0)
                    {
                        action = m_queue.Dequeue();
                    }
                }
                if (action != null)
                {
                    action.Invoke();
                }
            }
        }

        public void Produce(Action work)
        {
            try
            {
                Monitor.Enter(key);
                // critical section

                m_queue.Enqueue(work);
            }
            finally 
            {
                Monitor.Exit(key);
            }
        }
    }
}

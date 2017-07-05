//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//14 variant
namespace Queue
{
    class Program
    {
        static void Main(string[] args)
        {
            Queue<int> q = new Queue<int>();
            for (int i = 0; i < 10; i++)
                q.Enqueue(i * 4 + 2);
            for (int i = 0; i < 10; i++)
                if (i % 2 == 0)
                    q.Enqueue(q.Dequeue());
                else
                    q.Dequeue();
            for (int i =0;i<5;i++)
                Console.WriteLine(q.Dequeue());
        }
    }
}

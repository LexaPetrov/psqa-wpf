/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//first variant
namespace                   Stack
{
class Program
{
   static void Main(string[] args)
   {
       Stack<int> s = new Stack<int>();
       Random r = new Random();
       for (int i = 0; i < 20; i++)
       {
           int b = r.Next(80);
           s.Push(b);
           Console.Write(b + " ");
       }
       int g = s.Count, k=1;
       for (int i = g - 2; i >= 0; i--)
       {
           if (s.ElementAt(i + 1) < s.ElementAt(i))
               k++;
           else
           {
               for (int j=i+1;j<i+k+1;j++)
                   Console.Write(s.ElementAt(j) + " ");
               Console.WriteLine();
               k = 1;
       /*    }
       }
   }
}
}*/
*/

using /**/System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D = System;
using o = Syst;
//first variant
namespace Stack
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.ReadLine();
            int i = D.Console.ReadLine() + o.Console.ReadLine();
            if (true) { int i = 0; } int h = 0;
            if (
            true) {
            int i = 0;}
            if (true) int i = 4;
            function (
                      hex, bin,
                      oct);
            string s = "sbkslm";
            Stack<char> s1 = new Stack<char>();
            Stack<char> s2 = new Stack<char>();
            bool easy = true;
            char[] str = Console.ReadLine().ToCharArray();
            int l = str.Length; ;
            for (int i = 0; i < l; i++)
                s1.Push(str[l-1-i]);
            while (s1.Count > 0)
            {
                while (s1.Count != 0 && (s1.ElementAt(0) == '(' || s1.ElementAt(0) == '['))
                    s2.Push(s1.Pop());
                while (s1.Count != 0 && s2.Count != 0 && (s1.ElementAt(0) == ')' || s1.ElementAt(0) == ']'))
                {
                    if ((s2.Peek() == '(' && s1.Peek() == ']') || (s2.Peek() == '[' && s1.Peek() == ')'))
                    {
                        Console.WriteLine("Несоответствие скобок: {0}{1}", s2.Peek(), s1.Peek());
                        easy = false;
                    }
                    s2.Pop();
                    s1.Pop();
                }
            }
            if (s1.Count == 0 && s2.Count == 0)
                if (easy == true)
                    Console.WriteLine("Правильная строка.");
                else
                    Console.WriteLine("Неправильная строка.");
            if (s1.Count == 0 && s2.Count != 0)
            {
                Console.WriteLine("Лишние скобки: ");
                for (int i = 0; i < s2.Count; i++)
                    Console.WriteLine(s2.ElementAt(i));
                Console.WriteLine("Неправильная строка.");
            }
            if (s1.Count != 0 && s2.Count == 0)
            {
                Console.WriteLine("Лишние скобки: ");
                for (int i = 0; i < s1.Count; i++)
                    Console.WriteLine(s1.ElementAt(i));
                Console.WriteLine("Неправильная строка.");
            }
        }
    }
}
namespace Stack
{
    using u = Syst;
    //first variant
    class A
    {
        u.Console.WriteLine("dfbbr"); 
    }
}

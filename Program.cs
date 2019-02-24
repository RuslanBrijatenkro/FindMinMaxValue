using System;
using System.IO;
using System.Collections;
using System.Threading;
using System.Collections.Generic;

namespace FindMinMax
{
    class FindMinMaxClass
    {
        static int countofnumbers=1000;
        static List<int> threadspositions= new List<int>();
        static Mutex maxmutex=new Mutex();
        static Mutex minmutex=new Mutex();
        static Mutex mutex1 = new Mutex();
        static int count=0;
        static int min = -1000;
        static int max = 1000;
        static Thread thread;
        static List<int> numbers = new List<int>();
        static Random random = new Random();
        static void Main(string[] args)
        {
            for(int i=0;i<countofnumbers;i++)
            {
                numbers.Add(random.Next(min,max));
            }
            for(int i=0;i<4;i++)
            {
               thread = new Thread(FindMinMax);
               thread.IsBackground=false;
               thread.Name=Convert.ToString(i);
               threadspositions.Add(i*(countofnumbers/4));
               thread.Start();
            }
            foreach(var number in numbers)
            {
                Console.Write(number+" ");
            }
            System.Console.WriteLine();
            System.Console.WriteLine(max);
            System.Console.WriteLine(min);
            Console.ReadKey();
        }
        static void FindMinMax()
        {
            while(true)
            { 
                if(numbers[threadspositions[Convert.ToInt32(Thread.CurrentThread.Name)]]>max)
                {
                    maxmutex.WaitOne();
                    max=numbers[threadspositions[Convert.ToInt32(Thread.CurrentThread.Name)]];
                    maxmutex.ReleaseMutex();
                }
                    
                if(numbers[threadspositions[Convert.ToInt32(Thread.CurrentThread.Name)]]<min)
                {
                    minmutex.WaitOne();
                    min=numbers[threadspositions[Convert.ToInt32(Thread.CurrentThread.Name)]];
                    minmutex.ReleaseMutex();
                }
                if(threadspositions[Convert.ToInt32(Thread.CurrentThread.Name)]==(Convert.ToInt32(Thread.CurrentThread.Name)/(countofnumbers/4)+1)*(countofnumbers/4)-1)
                {
                    break;
                }
                threadspositions[Convert.ToInt32(Thread.CurrentThread.Name)]++;
            }
        }

    }
}

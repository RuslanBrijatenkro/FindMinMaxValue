using System;
using System.IO;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;

namespace FindMinMax
{
    class FindMinMaxClass
    {
        static Stopwatch stopwatch;
        static long onethreadtime;
        static long multithreadtime;
        static int count=0;
        static Mutex countmutex=new Mutex();
        static int countofnumbers=10000;
        static List<int> threadspositions= new List<int>();
        static AutoResetEvent event1=new AutoResetEvent(false);
        static Mutex maxmutex=new Mutex();
        static Mutex minmutex=new Mutex();
        static int minmultithread=1000;
        static int maxmultithread=-1000;
        static int minonethread=1000;
        static int maxonethread=-1000;
        static Thread thread;
        static List<int> numbers = new List<int>();
        static Random random = new Random();
        static void Main(string[] args)
        {
            for(int i=0;i<countofnumbers;i++)
            {
                numbers.Add(random.Next(-10*minmultithread,-10*maxmultithread));
            }

            stopwatch=new Stopwatch();
            stopwatch.Start();
            FindMinMax();
            stopwatch.Stop();
            onethreadtime=stopwatch.ElapsedMilliseconds;

            stopwatch=new Stopwatch();
            stopwatch.Start();
            for(int i=0;i<4;i++)
            {
                thread = new Thread(FindMinMaxMultiThread);
               thread.Name=Convert.ToString(i);
               threadspositions.Add(i*(countofnumbers/4));
               thread.Start();
            }
            event1.WaitOne();
            stopwatch.Stop();
            multithreadtime=stopwatch.ElapsedMilliseconds;

            System.Console.WriteLine("4 threads: ");
            System.Console.WriteLine(multithreadtime);
            System.Console.WriteLine(maxmultithread);
            System.Console.WriteLine(minmultithread);
            System.Console.WriteLine("1 thread: ");
            System.Console.WriteLine(onethreadtime);
            System.Console.WriteLine(maxonethread);
            System.Console.WriteLine(minonethread);

            Console.ReadKey();
        }
        static void FindMinMaxMultiThread()
        {
            while(true)
            { 
                if(numbers[threadspositions[Convert.ToInt32(Thread.CurrentThread.Name)]]>=maxmultithread)
                {
                    maxmutex.WaitOne();
                    maxmultithread=numbers[threadspositions[Convert.ToInt32(Thread.CurrentThread.Name)]];
                    maxmutex.ReleaseMutex();
                }
                else if(numbers[threadspositions[Convert.ToInt32(Thread.CurrentThread.Name)]]<=minmultithread)
                {
                    minmutex.WaitOne();
                    minmultithread=numbers[threadspositions[Convert.ToInt32(Thread.CurrentThread.Name)]];
                    minmutex.ReleaseMutex();
                }
                if(threadspositions[Convert.ToInt32(Thread.CurrentThread.Name)]==(Convert.ToInt32(Thread.CurrentThread.Name)+1)*(countofnumbers/4)-1)
                {
                    countmutex.WaitOne();
                    count++;
                    countmutex.ReleaseMutex();
                    if(count==4)
                        event1.Set();
                    break;         
                }
                threadspositions[Convert.ToInt32(Thread.CurrentThread.Name)]++;
            }
        }
        static void FindMinMax()
        {
            for(int i=0;i<countofnumbers;i++)
            {
                if(numbers[i]<minonethread)
                    minonethread=numbers[i];
                else if(numbers[i]>maxonethread)
                    maxonethread=numbers[i];
            }
        }

    }
}

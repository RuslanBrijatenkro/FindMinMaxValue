using System;
using System.IO;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using static System.Console;

namespace FindMinMax
{
    class FindMinMaxClass
    {
        static Stopwatch stopwatch;
        static long time;
        static int countofnumbers = 10_000_000;
        static int min;
        static int max;
        static object maxLock = new object();
        static object minLock = new object();
        static object randLock = new object();
        static List<int> numbers = new List<int>();
        static Random random = new Random();
        static void Main(string[] args)
        {
            Parallel.For(0, countofnumbers, i =>
            {
                lock(randLock)
                    numbers.Add(random.Next(-10000000, 10000000));
            });

            max=numbers.Min();
            min=numbers.Max();

            stopwatch = new Stopwatch();
            stopwatch.Start();
            Parallel.For(0, countofnumbers, i =>
            {
                if (numbers[i] < min)
                    lock(minLock) 
                        min = numbers[i];
                else if (numbers[i] > max)
                    lock (maxLock) 
                        max = numbers[i];
            });
            stopwatch.Stop();
            time = stopwatch.ElapsedMilliseconds;
            WriteLine("4 threads: ");
            WriteLine(time);
            WriteLine(max);
            WriteLine(min);
            stopwatch.Reset();

            max=numbers.Min();
            min=numbers.Max();

            stopwatch.Start();
            FindMinMax();
            stopwatch.Stop();
            time = stopwatch.ElapsedMilliseconds;
            WriteLine("1 thread: ");
            WriteLine(time);
            WriteLine(max);
            WriteLine(min);

            Console.ReadKey();
        }
        static void FindMinMax()
        {
            min=numbers.Min();
            max=numbers.Max();
        }
    }
}
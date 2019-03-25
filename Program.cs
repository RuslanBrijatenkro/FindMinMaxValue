using System;
using System.IO;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Console;
using System.Linq;

namespace FindMinMax
{
	class FindMinMaxClass
	{
		static int countofnumbers = 11;
		static int count = 0;
		int runThreads;
		static AutoResetEvent event1 = new AutoResetEvent(false);
		//static public int processorCount = Convert.ToInt32(Environment.ProcessorCount);
		static object newobject = new object();
		static public int processorCount = 2;
		static int abc = countofnumbers / processorCount;
		public int[] min = new int[processorCount];
		public int[] max = new int[processorCount];
		static void Main(string[] args)
		{
			FindMinMaxClass minMax = new FindMinMaxClass();
			minMax.Begin();
			Console.ReadKey();
		}
		public void Begin()
		{
			List<Thread> threads = new List<Thread>();
			Stopwatch stopwatch = new Stopwatch(); ;
			long time;
			Thread thread;
			List<int> numbers = new List<int>();
			Random random = new Random();

			for (int i = 0; i < countofnumbers; i++)
			{
				numbers.Add(random.Next(-10_000, 10_000));
			}
			foreach(var ele in numbers)
			{
				Console.Write(ele+" ");
			}
			Console.WriteLine();
			foreach (var item in min)
			{
				min[item] = 10000;
			}
			foreach (var item in max)
			{
				max[item] = -10000;
			}

			stopwatch.Start();
			FindMinMax(numbers, countofnumbers, min, max);
			stopwatch.Stop();
			time = stopwatch.ElapsedMilliseconds;
			WriteLine("1 thread: ");
			WriteLine(time);
			WriteLine(max[0]);
			WriteLine(min[0]);

			min[0] = 10000;
			max[0] = -10000;

			stopwatch.Reset();

			stopwatch.Start();
			runThreads = processorCount;
			for (int i = 0; i < processorCount; i++)
			{
				thread = new Thread(delegate () { FindMinMaxMultiThread(count++, numbers); });
				threads.Add(thread);
				//thread.IsBackground = false;
				thread.Start();
			}
			event1.WaitOne();
			foreach (var item in min)
			{
				if (min[0] > item)
					min[0] = item;
			}
			foreach (var item in max)
			{
				if (max[0] < item)
					max[0] = item;
			}
			stopwatch.Stop();
			time = stopwatch.ElapsedMilliseconds;

			WriteLine(processorCount + " threads:");
			WriteLine(time);
			WriteLine(max[0]);
			WriteLine(min[0]);

			Console.ReadKey();
		}
		public void FindMinMaxMultiThread(int countz, List<int> numbers)
		{
			int startIndex = countz * abc;
			int endIndex;
			if (countz == processorCount - 1)
				endIndex = countofnumbers;
			else
				endIndex = startIndex + abc;
			Console.WriteLine("Index "+endIndex);
			for (int k = startIndex; k < endIndex; k++)
			{
				if (numbers[k] < min[countz])
					min[countz] = numbers[k];
				if (numbers[k] > max[countz])
					max[countz] = numbers[k];
			}
			lock (newobject)
				runThreads--;
			if (runThreads == 0)
				event1.Set();
		}
		void FindMinMax(List<int> numbers, int countofnumbers, int[] min, int[] max)
		{
			for (int i = 0; i < countofnumbers; i++)
			{
				if (numbers[i] < min[0])
					min[0] = numbers[i];
				else if (numbers[i] > max[0])
					max[0] = numbers[i];
			}
		}

	}
}

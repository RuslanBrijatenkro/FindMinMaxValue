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
		static int countofnumbers = 100_000_000;
		static int count = 0;
		static AutoResetEvent event1 = new AutoResetEvent(false);
		//static public int processorCount = Convert.ToInt32(Environment.ProcessorCount);
		static object newobject = new object();
		static public int processorCount = 4;
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
			for (int i = 0; i < processorCount; i++)
			{
				thread = new Thread(delegate () { FindMinMaxMultiThread(count++, numbers); });
				threads.Add(thread);
				thread.IsBackground = false;
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
		public void FindMinMaxMultiThread(int z, List<int> numbers)
		{
			int i = z * abc;
			int numberofminormax=i/abc;
			for (int k = i; k < i + abc; k++)
			{
				if (numbers[k] < min[numberofminormax])
					min[numberofminormax] = numbers[k];
				else if (numbers[k] > max[numberofminormax])
					max[numberofminormax] = numbers[k];
			}
			lock (newobject)
				count--;
			if (count == 0)
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

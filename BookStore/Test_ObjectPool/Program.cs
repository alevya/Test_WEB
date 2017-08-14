using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test_ObjectPool
{
    class Program
    {
        static void Main(string[] args)
        {
            //ObjectPool<MyClass> pool = new ObjectPool<MyClass>(() => new MyClass(), 10);
            //Parallel.For(0, 10000, (i, loopState) =>
            //{
            //    MyClass mc = pool.GetObject();

            //    Console.CursorLeft = 0;
            //    Console.WriteLine(mc.GetValue(i));

            //    pool.PutObject(mc);
            //});

            //var w = new Wrap();
            //var wraps = new Wrap[3];
            //for (int i = 0; i < wraps.Length; i++)
            //{
            //    wraps[i] = w;
            //}

            //var values = wraps.Select(x => x.Value);
            //var results = Square(values);
            //int sum = 0;
            //int count = 0;
            //foreach (var r in results)
            //{
            //    count++;
            //    Console.WriteLine("result: {0}", r);
            //    sum += r;
            //}
            //Console.WriteLine("Count {0}", count);
            //Console.WriteLine("Sum {0}", sum);

            //Console.WriteLine("Count {0}", results.Count());
            //Console.WriteLine("Sum {0}", results.Sum());

            //object sync = new object();
            //var thread = new Thread(() =>
            //{
            //    try
            //    {
            //        Work();
            //    }
            //    finally
            //    {
            //        lock (sync)
            //        {
            //            Monitor.PulseAll(sync);
            //        }
            //    }
            //});
            //thread.Start();
            //lock (sync)
            //{
            //    Monitor.Wait(sync);
            //}
            //Console.WriteLine("test");

            try
            {
                Calc();
            }
            catch (MyCustomException e)
            {
                Console.WriteLine("Catch MyCustomException");
                //throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("Catch Exception");
            }


            Console.WriteLine("Press the Enter key to exit.");
            Console.ReadLine();

        }

        static IEnumerable<int> Square(IEnumerable<int> a)
        {
            foreach (var r in a)
            {
                Console.WriteLine(r * r);
                yield return r * r;
                Console.WriteLine("iter");
            }
        }
        class Wrap
        {
            private static int init = 0;
            public int Value
            {
                get { return ++init; }
            }
        }


        private static void Work()
        {
            Thread.Sleep(1000);
        }


        private static void Calc()
        {
            int result = 0;
            var x = 5;
            int y = 0;
            try
            {
                result = x / y;
            }
            catch (MyCustomException e)
            {
                Console.WriteLine("Catch DivideByZeroException");
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("Catch Exception");

                throw new MyCustomException(e);
            }
            finally
            {
                
            }
        }

        class MyCustomException : DivideByZeroException
        {
            internal MyCustomException(Exception e) : base(string.Empty, e)
            {
                
            }
        }

    }

   
    class MyClass
    {
        public int[] Nums { get; set; }
        public long GetValue(long i)
        {
            return Nums[i];
        }
        public MyClass()
        {
            Nums = new int[10000];
            Random rand = new Random();
            for (int i = 0; i < Nums.Length; i++)
                Nums[i] = rand.Next();
        }
    }

    public class ObjectPool<T>
    {
        private readonly ConcurrentBag<T> _objects;
        private readonly Func<T> _objectGenerator;
        private int _time;

        public ObjectPool(Func<T> objectGenerator, int time)
        {
            _objects = new ConcurrentBag<T>();
            _objectGenerator = objectGenerator;
            _time = time;
        }


        public T GetObject()
        {
            if (_objects.TryTake(out T item)) return item;

            return _objectGenerator();
        }

        public void PutObject(T item)
        {
            _objects.Add(item);
        }
    }
}

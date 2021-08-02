using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace TestMonitel
{
    class Program : IDisposable
    {
        public static Dictionary<int, string> Dictionary;
        private static int _n;
        private static int _keyStream1;
        private static int _keyStream2;
        private static Thread _readStream;
        private static Thread _writeStream2;
        private static Thread _writeStream1;
        private static object _object;

        static void Main(string[] args)
        {
            Dictionary = new Dictionary<int, string>();
            _object = new object();

            _writeStream1 = new Thread(WriteStream1);
            _writeStream2 = new Thread(WriteStream2);
            _readStream = new Thread(ReadStream);

            _writeStream1.Start();
            _writeStream2.Start();

            _readStream.Start();
            _readStream.Priority = ThreadPriority.Normal;

            Console.ReadKey();

            foreach (var item in Dictionary)
            {
                Console.WriteLine(item);
            }
        }

        private static async void ReadStream()
        {
            while (_n <= 100)
            {
                if (Dictionary.Count % 5 == 0 && Dictionary.Count != 0)
                {
                    await Task.Run(MainStream);
                }
            }
        }

        private static void MainStream()
        {
            if (Dictionary.Count % 5 == 0 && Dictionary.Count != 0)
            {
                Monitor.Enter(_object);
                //Console.WriteLine(@"MainStream");

                Dictionary.TryGetValue(_n, out var result);
                Dictionary.TryGetValue(_n - 1, out var result1);

                var item = _keyStream1 += _keyStream2;

                var key = item + Dictionary.Count;

                Dictionary.Add(key, $"MS — {Dictionary.Count} + {item}");
                Monitor.Exit(_object);
            }
        }

        private static void WriteStream1()
        {
            try
            {
                while (_n <= 100)
                {
                    if (IsKey())
                    {
                        Write("WS1");
                        _keyStream1 = Dictionary.Count;
                        //Console.WriteLine(@"WriteStream1 ++  " + _n);
                    }
                }
            }
            finally
            {
            }
        }

        private static void WriteStream2()
        {
            try
            {
                while (_n <= 100)
                {

                    if (_n % 2 != 0 || Dictionary.Count == 0) continue;
                    if (IsKey())
                    {
                        Write("WS2");
                        _keyStream2 = Dictionary.Count;
                        //Console.WriteLine(@"WriteStream2 ++  " + _n);
                    }
                }
            }
            finally
            {
            }
        }

        private static void Write(string ws)
        {
            Monitor.Enter(_object);
            Dictionary.Add(_n, $"{ws} — {DateTime.Now}");
            IncInt(ref _n);

            Thread.Sleep(1000);
            Monitor.Exit(_object);
        }

        private static void IncInt(ref int w)
        {
            w++;
        }

        private static bool IsKey()
        {
            if (Dictionary.ContainsKey(_n))
            {
                //Console.WriteLine(@"WriteStream1 ---- ContainsKey ---" + _n);
                Monitor.Enter(_object);
                IncInt(ref _n);
                Monitor.Exit(_object);
                return false;

            }

            return true;
        }


        public void Dispose()
        {
            _writeStream1.Abort();
            _writeStream2.Abort();
            _readStream.Abort();
        }
    }
}
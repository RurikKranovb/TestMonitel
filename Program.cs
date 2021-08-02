using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace TestMonitel
{
    class Program : IDisposable
    {
        public static Dictionary<int, string> Dictionary;
        private static int _n;
        private static Timer _timer;
        private static int _keyStream1;
        private static int _keyStream2;


        static void Main(string[] args)
        {
            Dictionary = new Dictionary<int, string>();
            _n = 1;
            _keyStream1 = 0;
            _keyStream2 = 0;
            _timer = new Timer() {Interval = 1000};

            _timer.Elapsed += _timer_Elapsed;

            _timer.Start();

            Console.ReadKey();
        }

        private static void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_n >= 100)
            {
                _timer.Stop();

                foreach (var item in Dictionary)
                {
                    Console.WriteLine(item);
                }

                return;
            }

            Task.Factory.StartNew(WriteStream1);
            Task.Factory.StartNew(WriteStream2);

            Task.Factory.StartNew(Read);
        }

        private static void IncInt(ref int n)
        {
            n++;
        }

        private static void WriteStream2()
        {
            if (Dictionary.ContainsKey(_n))
            {
                IncInt(ref _n);
            }
            else
            {
                if (_n % 2 == 0) return;

                Dictionary.Add(_n, $"WS2 — {DateTime.Now}");

                //Task.Factory.StartNew(Read);

                _keyStream2 = Dictionary.Count - 1;
                IncInt(ref _n);
            }
        }

        private static void WriteStream1()
        {
            if (_n % 2 != 0) return;

            if (Dictionary.ContainsKey(_n))
            {
                IncInt(ref _n);
            }
            else
            {
                Dictionary.Add(_n, $"WS1 — {DateTime.Now}");

                //Task.Factory.StartNew(Read);

                _keyStream1 = Dictionary.Count - 1;
                IncInt(ref _n);
            }
        }

        private static async void Read()
        {
            if (Dictionary.Count % 5 == 0)
            {
                await Task.Run(MainStream);
            }
        }

        private static void MainStream()
        {
            var item = _keyStream1 += _keyStream2;

            var key = item + Dictionary.Count;

            Dictionary.Add(key, $"MS — {Dictionary.Count} + {item}");
        }

        public void Dispose()
        {

        }
    }
}

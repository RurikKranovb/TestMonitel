using System;
using System.Collections.Generic;
using System.Timers;

namespace TestMonitel
{
    class Program
    {
        public static Dictionary<int, string> Dictionary;
        private static int _n;
        private static string _writeStream1;
        private static string _writeStream2;
        private static Timer _timer;

        static void Main(string[] args)
        {
            Dictionary = new Dictionary<int, string>();

            _n = 1;

            _writeStream1 = $"WS1 — {DateTime.Now}";
            _writeStream2 = $"WS2 — {DateTime.Now}";

            _timer = new Timer() {Interval = 1000};

            _timer.Elapsed += Timer_Elapsed;

            _timer.Start();


            Console.ReadKey();
            //do
            //{


            //    if (n % 2 == 0)
            //    {
            //        dictionary.Add(n, $"WS2 — {DateTime.Now}");
            //        //var writeStream2 = Task.Factory.StartNew(() =>
            //        //{

            //        //});
            //    }
            //    else
            //    {
            //        dictionary.Add(n, $"WS1 — {DateTime.Now}");
            //        //var writeStream1 = Task.Factory.StartNew(() =>
            //        //{

            //        //});
            //    }


            //} while (n < 100);





        }

        private static void IncInt(ref int n)
        {
            n++;
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {

            if (_n >= 100)
            {
                _timer.Stop();
                return;
            }


            IncInt(ref _n);
            Dictionary.Add(_n, _n % 2 == 0 ? _writeStream2 : _writeStream1);
        }
    }
}

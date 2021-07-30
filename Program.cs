using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private static string _fileName = @"./text.txt";
        public static event EventHandler _eventHandler;


        static void Main(string[] args)
        {
            Dictionary = new Dictionary<int, string>();
            _n = 1;

            _writeStream1 = $"WS1 — {DateTime.Now}";
            _writeStream2 = $"WS2 — {DateTime.Now}";

            _timer = new Timer() {Interval = 1000};

            _timer.Elapsed += Timer_Elapsed;

            _timer.Start();
            _eventHandler += Program__eventHandler;

            Console.ReadKey();
        }

        private static void Program__eventHandler(object sender, EventArgs e)
        {


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

            using (var fileStream = new FileStream(_fileName, FileMode.Append, FileAccess.Write))
            {
                using var streamWriter = new StreamWriter(fileStream);

                streamWriter.WriteLine(_n % 2 == 0 ? _writeStream2 : _writeStream1);
                streamWriter.Flush();
                streamWriter.Close();
            }


            var file = File.OpenRead(_fileName);
            var bytes = new byte[file.Length];
            file.Read(bytes, 0, bytes.Length);
            CheckItems(bytes);
            file.Close();

            IncInt(ref _n);


            //Dictionary.Add(_n,  ? _writeStream2 : _writeStream1);
        }

        private static void CheckItems(byte[] bytes)
        {
            Dictionary.Clear();

            using var memory = new MemoryStream(bytes);
            using (var streamReader = new StreamReader(memory))
            {
                var text = streamReader.ReadToEnd();
                var items = text.Split('\n');//.ToDictionary(s => s[0], s =>s[1]);
                for (int i = 0; i < items.Length - 2; i++)
                {
                    var item = items[i];
                    Dictionary.Add(i, item);

                }

                if (Dictionary.Count % 5 == 0 && Dictionary.Count != 0)
                {
                   _eventHandler?.Invoke(items, EventArgs.Empty);
                }

                streamReader.Close();
            }
            memory.Close();
        }
    }
}

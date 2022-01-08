using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading;

namespace WinWavePlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            var shouldListFiles = false;
            var delayMs = 0;
            var initialDelayMs = 0;
            var multiplier = 1;

            if (args.Length == 0)
            {
                Console.WriteLine("Usage: <filename> --repeat 10 --delay 100");
                return;
            }

            for (var i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                if (arg == "--ls")
                {
                    shouldListFiles = true;
                }

                if (arg == "--delay")
                {
                    delayMs = int.Parse(args[i + 1]);
                    i++;
                }

                if (arg == "--initialdelay")
                {
                    initialDelayMs = int.Parse(args[i + 1]);
                    i++;
                }

                if (arg == "--repeat")
                {
                    multiplier = int.Parse(args[i + 1]);
                    i++;
                }
            }

            if (args[0] != "--ls")
            {
                Play(args[0], multiplier, delayMs, initialDelayMs);
            }

            if (shouldListFiles)
            {
                ListWaveFiles();
            }
        }

        private static void Play(string path, int multiplier, int delayMs, int initialDelayMs)
        {
            var soundPlayer = new SoundPlayer(path);
            soundPlayer.Load();

            if (initialDelayMs > 0)
            {
                Thread.Sleep(initialDelayMs);
            }

            for (int i = 0; i < multiplier; i++)
            {
                var isLast = i == multiplier - 1;
                
                if (isLast || delayMs == 0)
                {
                    soundPlayer.PlaySync();
                }
                else
                {
                    soundPlayer.Play();
                    Thread.Sleep(delayMs);
                }
            }
        }

        private static void ListWaveFiles()
        {
            var directoryInfo = new DirectoryInfo(Environment.CurrentDirectory);

            var waveFiles = directoryInfo.GetFiles("*.wav")
                .OrderBy(x => x.Name)
                .ToArray();

            foreach (var file in waveFiles)
            {
                Console.WriteLine($"{file.Name}");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading;

namespace WinWavePlayer
{
    class Program
    {
        private static DirectoryInfo _directoryInfo;

        static void Main(string[] args)
        {
            var directoryPath = Environment.CurrentDirectory;

            if (args.Length > 0)
            {
                directoryPath = args[0];
            }

            _directoryInfo = new DirectoryInfo(directoryPath);

            while (true)
            {
                Console.WriteLine("Type something like f123, f123*10@100ms, d123, ls or ..");

                var input = Console.ReadLine();

                try
                {
                    var command = CommandParser.Parse(input);
                    
                    HandleInput(command);
                    
                    ListAll();
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid command, try again...");
                }
            }
        }

        private static void HandleInput(Command command)
        {
            if (command.CommandName == "..")
            {
                _directoryInfo = _directoryInfo.Parent;
            }
            else if (command.CommandName == "ls")
            {
            }
            else if (command.CommandName == "d")
            {
                var directories = GetDirectoryInfos();
                _directoryInfo = directories[command.CommandNumber - 1];
            }
            else if (command.CommandName == "f")
            {
                var files = GetWaveFiles();
                var file = files[command.CommandNumber - 1];

                for (int i = 0; i < command.Multiplicator; i++)
                {
                    new SoundPlayer(file.FullName).Play();

                    Thread.Sleep(command.DelayMs);
                }
            }
            else
            {
                Console.WriteLine("Unknown command");
            }
        }

        private static void ListAll()
        {
            Console.Clear();

            Console.WriteLine("Directory: " + _directoryInfo.FullName);

            var directories = GetDirectoryInfos();
            var waveFiles = GetWaveFiles();

            Console.WriteLine("..\t..");
            for (var i = 0; i < directories.Length; i++)
            {
                var directory = directories[i];
                Console.WriteLine($"d{i + 1}.\t{directory.Name}");
            }

            for (var i = 0; i < waveFiles.Length; i++)
            {
                var file = waveFiles[i];
                Console.WriteLine($"f{i + 1}.\t{file.Name}");
            }
        }

        private static FileInfo[] GetWaveFiles()
        {
            var waveFiles = _directoryInfo.GetFiles("*.wav");
            return waveFiles.OrderBy(x => x.Name).ToArray();
        }

        private static DirectoryInfo[] GetDirectoryInfos()
        {
            var directories = _directoryInfo.GetDirectories();

            return directories.OrderBy(x => x.Name).ToArray();
        }
    }
}
using System;
using System.Text.RegularExpressions;

namespace WinWavePlayer
{
    public class CommandParser
    {
        public static Command Parse(string input)
        {
            var regexPattern = @"(?<name>[a-z\.]+)(?<number>\d+)?(\*(?<multiplicator>\d+))?(\@(?<delay>\d+)(ms)?)?(?<rest>.*)";

            var match = Regex.Match(input, regexPattern);

            var nameGroup = match.Groups["name"];
            var numberGroup = match.Groups["number"];
            var multiplicatorGroup = match.Groups["multiplicator"];
            var delayGroup = match.Groups["delay"];
            
            return new Command()
            {
                CommandName = nameGroup.Value,
                CommandNumber = numberGroup.Success ? int.Parse(numberGroup.Value) : 0,
                Multiplicator = multiplicatorGroup.Success ? int.Parse(multiplicatorGroup.Value) : 1,
                DelayMs = delayGroup.Success ? int.Parse(delayGroup.Value) : 0,
            };
        }
    }

    public class Command
    {
        public string CommandName { get; set; }
        public int CommandNumber { get; set; }
        public int Multiplicator { get; set; }
        public int DelayMs { get; set; }
    }
}
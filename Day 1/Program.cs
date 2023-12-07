using System.Text.RegularExpressions;

namespace Day_1;

class Program
{
    private static int CalculateSumPart1(IEnumerable<string> data)
    {
        int sum = default;

        var regex = new Regex("\\d");
        var ints = data.Select(line => regex.Matches(line).Select(match => int.Parse(match.Value)).ToList()).ToList();

        foreach (var line in ints)
        {
            var partialSum = string.Join("", line[0].ToString(), line[^1].ToString());

            sum += int.Parse(partialSum);
        }

        return sum;
    }

    private static int CalculateSumPart2(IEnumerable<string> data)
    {
        int sum = default;
        var numbers = new Dictionary<string, int>
        {
            { "oneight", 18 },
            { "twone", 21 },
            { "threeight", 38 },
            { "fiveight", 58 },
            { "sevenine", 79 },
            { "eightwo", 82 },
            { "eighthree", 83 },
            { "nineight", 98 },
            { "one", 1 },
            { "two", 2 },
            { "three", 3 },
            { "four", 4 },
            { "five", 5 },
            { "six", 6 },
            { "seven", 7 },
            { "eight", 8 },
            { "nine", 9 }
        };

        foreach (var line in data)
        {
            var modifiedLine = line;

            foreach (var (key, value) in numbers)
            {
                modifiedLine = Regex.Replace(modifiedLine, key, value.ToString(), RegexOptions.IgnoreCase);
            }

            // Remove remaining non-digit characters
            modifiedLine = Regex.Replace(modifiedLine, @"[\D]", "");

            var firstDigit = modifiedLine[0].ToString();
            var lastDigit = modifiedLine[^1].ToString();
            sum += int.Parse(firstDigit + lastDigit);
        }

        return sum;
    }

    private static void Main()
    {
        var data = File.ReadLines("../../../input.txt");
        var enumerable = data as string[] ?? data.ToArray();
        Console.WriteLine($"Part 1: {CalculateSumPart1(enumerable)}");
        Console.WriteLine($"Part 2: {CalculateSumPart2(enumerable)}");
    }
}
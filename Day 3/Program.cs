using System.Text.RegularExpressions;

namespace Day_3;

class Program
{
    // The engine schematic (your puzzle input) consists of a visual representation of the engine.
    // There are lots of numbers and symbols you don't really understand, but apparently any number adjacent to a symbol,
    // even diagonally, is a "part number" and should be included in your sum. (Periods (.) do not count as a symbol.)

    private static int CalculateEngineSumPart1(IEnumerable<string> data)
    {
        //We will need to check the 8 surrounding tiles if we hit a digit and
        //add the number of which the digit it a part of to the sum if that digit adjacent to a symbol

        //Regex for the special characters/symbols
        var regex = new Regex(@"[!@#$%^&*()_+\-=\[\]{};':,<>/?]");
        int engineSum = default;
        int currentNumber = default;

        //Double loop to go through the engine schematic left to right, top to bottom
        var enumerable = data as string[] ?? data.ToArray();
        for (var i = 0; i < enumerable.Count(); i++)
        {
            for (var j = 0; j < enumerable.ElementAt(i).Length; j++)
            {
                //If we hit a digit, go right until we hit a symbol or . and add the number to the current number
                if (char.IsDigit(enumerable.ElementAt(i)[j]))
                {
                    var right = j;
                    while (right < enumerable.ElementAt(i).Length &&
                           char.IsDigit(enumerable.ElementAt(i)[right]) && enumerable.ElementAt(i)[right] != '.')
                    {
                        currentNumber *= 10;
                        currentNumber += int.Parse(enumerable.ElementAt(i)[right].ToString());
                        right++;
                    }
                }

                if (currentNumber == 0) continue;
                //For each number found (length of current number to the right as well,
                //we check the 8 surrounding tiles, if we hit a symbol,
                //we set a flag to true, so we can add the end number to the sum
                var foundAdjacentSymbol = false;

                for (var length = 0; length < currentNumber.ToString().Length; length++)
                {
                    for (var k = -1; k <= 1; k++)
                    {
                        for (var l = -1; l <= 1; l++)
                        {
                            // Skip the center tile (the current digit itself)
                            if (k == 0 && l == 0)
                                continue;

                            // Calculate the position of the surrounding tile
                            var newRow = i + k;
                            var newCol = j + l + length; // Adjusted position for each iteration
                            if (newRow >= 0 && newRow < enumerable.Count() && newCol >= 0 &&
                                newCol < enumerable.ElementAt(newRow).Length)
                            {
                                // We check if the tile is a symbol
                                if (regex.IsMatch(enumerable.ElementAt(newRow)[newCol].ToString()))
                                {
                                    foundAdjacentSymbol = true;
                                }
                            }
                        }
                    }
                }

                //If we found a symbol, we add the current number to the sum
                if (foundAdjacentSymbol)
                {
                    engineSum += currentNumber;
                    // Console.WriteLine("Added number: " + currentNumber);
                }

                //Reset the current number
                currentNumber = 0;

                //Skip to the next symbol or .
                while (j < enumerable.ElementAt(i).Length && !regex.IsMatch(enumerable.ElementAt(i)[j].ToString()) &&
                       enumerable.ElementAt(i)[j] != '.')
                {
                    j++;
                }
            }
        }

        return engineSum;
    }

    private static int CalculateGearRatioSumPart2(IEnumerable<string> data)
    {
        //We will need to check if two numbers are adjacent to one *, if they are, we multiply them and add them to the Gear Ratio sum
        int gearRatioSum = default;

        //Double loop to go through the engine schematic left to right, top to bottom

        var enumerable = data as string[] ?? data.ToArray();
        for (var i = 0; i < enumerable.Count(); i++)
        {
            for (var j = 0; j < enumerable.ElementAt(i).Length; j++)
            {
                //If we hit a * we check the 8 surrounding tiles for numbers
                if (enumerable.ElementAt(i)[j] == '*')
                {
                    var hits = 0; //Needed to check if we hit two numbers
                    var numbers1 = new LinkedList<int>();
                    var numbers2 = new LinkedList<int>();
                    for (var k = -1; k <= 1; k++)
                    {
                        for (var l = -1; l <= 1; l++)
                        {
                            // Skip the center tile (the current digit itself)
                            if (k == 0 && l == 0)
                                continue;

                            // Calculate the position of the surrounding tile
                            var newRow = i + k;
                            var newCol = j + l;
                            if (newRow < 0 || newRow >= enumerable.Count() || newCol < 0 ||
                                newCol >= enumerable.ElementAt(newRow).Length) continue;
                            // We check if the tile is a number
                            if (char.IsDigit(enumerable.ElementAt(newRow)[newCol]))
                            {
                                var currentNumbers = (hits == 0) ? numbers1 : numbers2;
                                currentNumbers.Clear();
                                if (hits == 0)
                                {
                                    // Reset the linked lists for the first set of adjacent digits
                                    numbers1.Clear();
                                    var left = newCol;
                                    while (left >= 0 && char.IsDigit(enumerable.ElementAt(newRow)[left]))
                                    {
                                        numbers1.AddFirst(int.Parse(enumerable.ElementAt(newRow)[left].ToString()));
                                        left--;
                                    }

                                    // Skip the current digit when adding to the second set
                                    var right = newCol + 1;
                                    while (right < enumerable.ElementAt(newRow).Length &&
                                           char.IsDigit(enumerable.ElementAt(newRow)[right]))
                                    {
                                        numbers1.AddLast(int.Parse(enumerable.ElementAt(newRow)[right].ToString()));
                                        right++;
                                    }

                                    hits++;
                                }

                                else if (hits == 1)
                                {
                                    // Reset the linked lists for the second set of adjacent digits
                                    numbers2.Clear();
                                    var left = newCol - 1;
                                    while (left >= 0 && char.IsDigit(enumerable.ElementAt(newRow)[left]))
                                    {
                                        numbers2.AddFirst(int.Parse(enumerable.ElementAt(newRow)[left].ToString()));
                                        left--;
                                    }

                                    // Skip the current digit when adding to the first set
                                    var right = newCol;
                                    while (right < enumerable.ElementAt(newRow).Length &&
                                           char.IsDigit(enumerable.ElementAt(newRow)[right]))
                                    {
                                        numbers2.AddLast(int.Parse(enumerable.ElementAt(newRow)[right].ToString()));
                                        right++;
                                    }

                                    if (numbers1.SequenceEqual(numbers2)) continue;
                                    // We concat the numbers and add them to the sum
                                    var number1 = numbers1.Aggregate(0, (current, number) => current * 10 + number);
                                    var number2 = numbers2.Aggregate(0, (current, number) => current * 10 + number);
                                    gearRatioSum += number1 * number2;

                                    // Console.WriteLine($"Added number: {number1} * {number2}");

                                    hits = 0;
                                }
                            }
                        }
                    }

                    // ReSharper disable once RedundantAssignment, it's not redundant
                    hits = 0;
                }
            }
        }


        return gearRatioSum;
    }

    private static void Main()
    {
        var data = File.ReadLines("../../../input.txt");
        var enumerable = data as string[] ?? data.ToArray();
        Console.WriteLine($"Part 1: {CalculateEngineSumPart1(enumerable)}");
        Console.WriteLine($"Part 2: {CalculateGearRatioSumPart2(enumerable)}");
    }
}
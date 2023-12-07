namespace Day_2;

class Program
{
    private static int CalculateGamesPart1(IEnumerable<string> data)
    {
        // 12 red cubes, 13 green cubes, and 14 blue cubes.
        var maxDict = new Dictionary<string, int>
        {
            { "red", 12 },
            { "green", 13 },
            { "blue", 14 }
        };

        int gameIdSum = default;

        //We check each game in the input and compare it to the maxDict, if it's valid we add it to the sum
        // You play several games and record the information from each game (your puzzle input).
        // Each game is listed with its ID number (like the 11 in Game 11: ...) followed by a semicolon-separated
        // list of subsets of cubes that were revealed from the bag (like 3 red, 5 green, 4 blue).
        foreach (var game in data)
        {
            // We split the game into the game id and the cubes
            var splitGame = game.Split(":");
            var gameId = splitGame[0].Split(" ")[1].Trim();
            var cubes = splitGame[1].Split(";");
            // var cubesDict = new Dictionary<string, int>();

            // // We add the cubes to a dictionary
            // foreach (var bag in cubes)
            // {
            //     foreach (var cube in bag.Split(","))
            //     {
            //         var splitCube = cube.Trim().Split(" ");
            //         var color = splitCube[1];
            //         var amount = int.Parse(splitCube[0]);
            //
            //         if (cubesDict.ContainsKey(color))
            //         {
            //             cubesDict[color] += amount;
            //         }
            //         else
            //         {
            //             cubesDict.Add(color, amount);
            //         }
            //     }
            // }
            //
            // // We check if the cubes are valid
            // var isValid = true;
            // foreach (var (key, value) in cubesDict)
            // {
            //     if (value <= maxDict[key]) continue;
            //     isValid = false;
            //     break;
            // }

            //We add the cubes to a dictionary, if one of the cubes is invalid we set isValid to false (not the sum of bags)
            var isValid = true;
            foreach (var bag in cubes)
            {
                foreach (var cube in bag.Split(","))
                {
                    var splitCube = cube.Trim().Split(" ");
                    var color = splitCube[1];
                    var amount = int.Parse(splitCube[0]);

                    if (amount > maxDict[color])
                    {
                        isValid = false;
                        break;
                    }
                }
            }

            // If the cubes are valid we add the game id to the sum
            if (isValid)
            {
                gameIdSum += int.Parse(gameId);
            }
        }

        return gameIdSum;
    }

    private static int CalculateGamesPart2(IEnumerable<string> data)
    {
        int gamePower = default;

        // As you continue your walk, the Elf poses a second question:
        // in each game you played, what is the fewest number of cubes of each color
        // that could have been in the bag to make the game possible?
        foreach (var game in data)
        {
            var splitGame = game.Split(":");
            var cubes = splitGame[1].Split(";");
            var maxFromBag = new Dictionary<string, int>();

            // We add the cubes to a dictionary
            foreach (var bag in cubes)
            {
                foreach (var cube in bag.Split(","))
                {
                    var splitCube = cube.Trim().Split(" ");
                    var color = splitCube[1];
                    var amount = int.Parse(splitCube[0]);

                    //If the dict holds the color, check if the amount is less than the current amount, if it's not we set it to the current amount
                    if (maxFromBag.ContainsKey(color))
                    {
                        if (amount > maxFromBag[color])
                        {
                            maxFromBag[color] = amount;
                        }
                    }
                    else
                    {
                        maxFromBag.Add(color, amount);
                    }
                }
            }

            // We take the power of the cubes and add it to the sum
            var power = maxFromBag.Values.Aggregate(1, (current, value) => current * value);
            gamePower += power;
        }

        // The power of a set of cubes is equal to the numbers of red, green, and blue cubes multiplied together.
        return gamePower;
    }

    private static void Main()
    {
        var data = File.ReadLines("../../../input.txt");
        var enumerable = data as string[] ?? data.ToArray();
        Console.WriteLine($"Part 1: {CalculateGamesPart1(enumerable)}");
        Console.WriteLine($"Part 2: {CalculateGamesPart2(enumerable)}");
    }
}
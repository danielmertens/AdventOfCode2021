global using System;
global using System.Linq;
using AdventOfCode2021.Days;

var art =
@"          .     .  .      +     .      .          
     .       .      .     #       .           .   |\   __  \|\   ___ \|\  \    /  /|\  ___ \ |\   ___  \|\___   ___\
        .      .         ###            .      .  \ \  \|\  \ \  \_|\ \ \  \  /  / | \   __/|\ \  \\ \  \|___ \  \_|
      .      .   ""#:. .:##""##:. .:#""  .      .     \ \   __  \ \  \ \\ \ \  \/  / / \ \  \_|/_\ \  \\ \  \   \ \  \
          .      . ""####""###""####""  .               \ \  \ \  \ \  \_\\ \ \    / /   \ \  \_|\ \ \  \\ \  \   \ \  \
       .     ""#:.    .:#""###""#:.    .:#""  .          \ \__\ \__\ \_______\ \__/ /     \ \_______\ \__\\ \__\   \ \__\
  .             ""#########""#########""        .        \|__|\|__|\|_______|\|__|/       \|_______|\|__| \|__|    \|__|
        .    ""#:.  ""####""###""####""  .:#""   .             
     .     .  ""#######""""##""##""""#######""               |\   __  \|\  _____\    |\   ____\|\   __  \|\   ___ \|\  ___ \     
                .""##""#####""#####""##""           .      \ \  \|\  \ \  \__/     \ \  \___|\ \  \|\  \ \  \_|\ \ \   __/|    
    .   ""#:. ...  .:##""###""###""##:.  ... .:#""          \ \  \\\  \ \   __\     \ \  \    \ \  \\\  \ \  \ \\ \ \  \_|/__  
      .     ""#######""##""#####""##""#######""      .        \ \  \\\  \ \  \_|      \ \  \____\ \  \\\  \ \  \_\\ \ \  \_|\ \ 
    .    .     ""#####""""#######""""#####""    .      .       \ \_______\ \__\        \ \_______\ \_______\ \_______\ \_______\
            .     ""      000      ""    .     .            \|_______|\|__|         \|_______|\|_______|\|_______|\|_______|
       .         .   .   000     .        .              
.. .. ..................O000O........................ ...... ...

";
Console.SetWindowSize(140, 40);
Console.WriteLine(art);

var abstractDayTyps = typeof(AbstractDay);
var days = AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(t => t.GetTypes())
    .Where(t => t.IsAssignableTo(abstractDayTyps) && t != abstractDayTyps);

// Print days for reasons?
for (int i = 0; i <= days.Count() / 3; i ++)
{
    var line = "";
    for (int j = 0; j < 3 && i * 3 + j < days.Count(); j++)
    {
        line += String.Format("{0, -24}", days.ElementAt(i * 3 + j).Name);
    }
    Console.WriteLine(line);
}

Console.Write($"\nSelect day to run ({DateTime.Now.Day}): ");

// Get day to run.
var input = Console.ReadLine();

if (string.IsNullOrWhiteSpace(input)) input = DateTime.Now.Day.ToString();
else if (!int.TryParse(input, out int parsed))
{
    Console.WriteLine("Input must be a number.");
    return;
}
if (input.Length == 1) input = "0" + input;

// Run selected type
var selectedType = days.First(d => d.Name == "Day" + input);
AbstractDay abstractDay = Activator.CreateInstance(selectedType) as AbstractDay;
abstractDay!.SolveProblem();

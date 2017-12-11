using Fclp;
using System;

namespace TestElevator {
	/// <summary>
	/// Elevator test
	/// </summary>
	class Program {
		static void Main(string[] args) {
			//arguments setup
			var p = new FluentCommandLineParser<Options>();
			p.Setup(o => o.Floors)
			 .As('f', "floors")
			 .WithDescription("The amount of floors.")
			 .Required();
			p.Setup(o => o.Height)
			 .As('h', "height")
			 .WithDescription("The height of each floor in meters.")
			 .Required();
			p.Setup(o => o.Speed)
			 .As('s', "speed")
			 .WithDescription("The elevator speed in m/s.")
			 .Required();
			p.Setup(o => o.Delay)
			 .As('d', "delay")
			 .WithDescription("The time span between opening and closing the door in seconds.")
			 .Required();
			p.SetupHelp("?", "help")
			  .Callback(text => Console.WriteLine(text));

			ICommandLineParserResult result = null;
			try {
				result = p.Parse(args);
				if(result.HasErrors)
					Console.Write(result.ErrorText + "\nType --help or -? for help");
				else if(!result.HelpCalled) {
					//creates the building
					Building building = new Building((uint)p.Object.Floors, p.Object.Height);
					//adds an elevator
					Elevator elevator = building.AddElevator(p.Object.Speed, p.Object.Delay);
					//setup events
					elevator.OnDoorOpen += (floor) => {
						Console.WriteLine("##### DOOR OPENED ON FLOOR " + floor);
					};
					elevator.OnDoorClose += (floor) => {
						Console.WriteLine("##### DOOR CLOSED ON FLOOR " + floor);
					};
					elevator.OnPassFloor += (floor) => {
						Console.WriteLine("##### PASSED ON THE FLOOR " + floor);
					};
					for(; ; ) {
						try {
							Console.WriteLine("Type the floor number (between 0 and " + (building.Floors - 1) + ")");
							elevator.Visit(uint.Parse(Console.ReadLine()));
						}
						catch(ArgumentException e) {
							Console.WriteLine(e.Message);
						}
						catch {
							Console.WriteLine("An unexpected error has occurred");
						}
					}
				}
			}
			catch(Exception e) {
				Console.Write(e.InnerException is ArgumentException ? e.InnerException.Message : "An unexpected error has happened");
			}
		}
	}
}
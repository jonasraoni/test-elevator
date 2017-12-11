using System;
using System.Collections.Generic;

namespace TestElevator {
	/// <summary>
	/// Represents a building
	/// </summary>
	public class Building {
		/// <summary>
		/// Creates a new building
		/// </summary>
		/// <param name="floors">amount of floors</param>
		/// <param name="floorHeight">height of each floor in meters</param>
		public Building(uint floors, double floorHeight) {
			Floors = floors;
			FloorHeight = floorHeight;
		}
		/// <summary>
		/// Adds an elevator
		/// </summary>
		/// <param name="speed">Speed of the elevator in m/s</param>
		/// <param name="delay">Delay between opening and closing the doors in seconds</param>
		public Elevator AddElevator(double speed, double delay) {
			Elevators.Add(new Elevator(this, speed, delay));
			return Elevators[Elevators.Count - 1];
		}

		/// <summary>
		/// Elevators of the building
		/// </summary>
		public List<Elevator> Elevators { get; set; } = new List<Elevator>();

		/// <summary>
		/// Amount of floors
		/// </summary>
		public uint Floors
		{
			get
			{
				return floors;
			}
			private set
			{
				floors = value;
			}
		}

		/// <summary>
		/// Height of each floor in meters
		/// </summary>
		public double FloorHeight
		{
			get
			{
				return floorHeight;
			}
			private set
			{
				if(value <= 0)
					throw new ArgumentOutOfRangeException("Height must be bigger than 0");
				floorHeight = value;
			}
		}
		private uint floors;
		private double floorHeight;
	}
}
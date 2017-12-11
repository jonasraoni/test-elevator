using System;

namespace TestElevator {
	/// <summary>
	/// Stores the application options
	/// </summary>
	public class Options {
		int floors;
		double height, speed, delay;
		/// <summary>
		/// Amount of floors
		/// </summary>
		public int Floors
		{
			get
			{
				return floors;
			}
			set
			{
				if(value < 5 || value > 20)
					throw new ArgumentOutOfRangeException("Floors must be between 5 and 20");
				floors = value;
			}
		}

		/// <summary>
		/// Height of each floor
		/// </summary>
		public double Height
		{
			get
			{
				return height;
			}
			set
			{
				if(value <= 0)
					throw new ArgumentOutOfRangeException("Height must be bigger than 0");
				height = value;
			}
		}

		/// <summary>
		/// Speed of the elevator
		/// </summary>
		public double Speed
		{
			get
			{
				return speed;
			}
			set
			{
				if(value <= 0)
					throw new ArgumentOutOfRangeException("Speed must be bigger than 0");
				speed = value;
			}
		}

		/// <summary>
		/// Delay between opening and closing the doors
		/// </summary>
		public double Delay
		{
			get
			{
				return delay;
			}
			set
			{
				if(value < 0)
					throw new ArgumentOutOfRangeException("Delay cannot be negative");
				delay = value;
			}
		}
	}
}
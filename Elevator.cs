using System;
using System.Collections.Generic;
using System.Threading;

/// <summary>
/// Namepsace
/// </summary>
namespace TestElevator {
	/// <summary>
	/// Direction of the elevator
	/// </summary>
	public enum DirectionType {
		/// <summary>
		/// The elevator is idle
		/// </summary>
		IDLE = 0,
		/// <summary>
		/// The elevator is going up
		/// </summary>
		UP = 1,
		/// <summary>
		/// The elevator is going down
		/// </summary>
		DOWN = 2,
	}

	/// <summary>
	/// Simulates an elevator
	/// </summary>
	public class Elevator {
		/// <summary>
		/// Creates a new elevator
		/// </summary>
		/// <param name="building">Building where it belongs</param>
		/// <param name="speed">Speed of the elevator in m/s</param>
		/// <param name="delay">Delay between opening and closing the doors</param>
		public Elevator(Building building, double speed, double delay) {
			Speed = speed;
			DoorDelay = delay;
			this.building = building;
		}

		/// <summary>
		/// Schedule an elevator visit
		/// </summary>
		/// <param name="floor">The floor that must be visited</param>
		public void Visit(uint floor) {
			if(floor > building.Floors - 1)
				throw new ArgumentException("Invalid floor");
			lock(requestedFloors)
				requestedFloors.Add(floor);
			if(task == null || !task.IsAlive) {
				(task = new Thread(Run) {
					IsBackground = true
				}).Start();
			}
		}

		/// <summary>
		/// Runs the elevator
		/// </summary>
		private void Run() {
			var timer = DateTime.Now;
			uint? destiny = null;
			for(; ; ) {
				//if there's a floor request available, check if it's needed to update the current destiny
				if(requestedFloors.Count > 0) {
					//check if a floor on the way up/down to its destiny was called
					if(Direction == DirectionType.UP)
						destiny = requestedFloors.GetViewBetween(Floor, building.Floors - 1).Min;
					else if(Direction == DirectionType.DOWN)
						destiny = requestedFloors.GetViewBetween(0, Floor).Max;
					//if nothing was found, look for closest request to the current floor
					if(!destiny.HasValue) {
						Direction = DirectionType.IDLE;
						var a = requestedFloors.GetViewBetween(0, Floor).Max;
						var b = requestedFloors.GetViewBetween(Floor, building.Floors - 1).Min;
						destiny = a > b ? a : b ?? a;
					}
				}
				//if idle, decide where to go
				if(Direction == DirectionType.IDLE) {
					if((destiny ?? Floor) != Floor) {
						Direction = destiny > Floor ? DirectionType.UP : DirectionType.DOWN;
						timer = DateTime.Now;
					}
				}
				//otherwise, keep moving
				else if(destiny.HasValue) {
					if((DateTime.Now - timer).Seconds >= building.FloorHeight / Speed) {
						Floor = Direction == DirectionType.UP ? Floor + 1 : Floor - 1;
						timer = DateTime.Now;
					}
				}
				//once the destiny is reached, remove the floor from the request list and open the door
				if(destiny == Floor) {
					destiny = null;
					lock(requestedFloors) {
						requestedFloors.Remove(Floor);
						if(requestedFloors.Count == 0)
							Direction = DirectionType.IDLE;
					}
					IsDoorOpened = true;
					timer = DateTime.Now;
				}
				//once the door timeout is reached, close the door
				if(IsDoorOpened && (DateTime.Now - timer).Seconds >= DoorDelay)
					IsDoorOpened = false;
				if(Direction == DirectionType.IDLE && !IsDoorOpened && requestedFloors.Count < 1)
					break;
			}
		}

		/// <summary>
		/// The elevators' speed in m/s
		/// </summary>
		public double Speed { get; set; }

		/// <summary>
		/// The delay between opening and closing the doors in seconds
		/// </summary>
		public double DoorDelay { get; set; }

		/// <summary>
		/// Elevator events
		/// </summary>
		public event Action<uint> OnDoorOpen, OnDoorClose, OnPassFloor;

		/// <summary>
		/// The elevator status
		/// </summary>
		private DirectionType Direction { get; set; } = DirectionType.IDLE;

		/// <summary>
		/// Whether the door is opened
		/// </summary>
		private bool IsDoorOpened
		{
			get
			{
				return isDoorOpened;
			}
			set
			{
				((isDoorOpened = value) ? OnDoorOpen : OnDoorClose)?.Invoke(Floor);
			}
		}

		/// <summary>
		/// Current floor
		/// </summary>
		private uint Floor
		{
			get
			{
				return floor;
			}
			set
			{
				floor = value;
				OnPassFloor?.Invoke(Floor);
			}
		}

		/// <summary>
		/// Ordered set of requested floors
		/// </summary>
		private SortedSet<uint?> requestedFloors = new SortedSet<uint?>();

		/// <summary>
		/// The thread which manages the elevator
		/// </summary>
		private Thread task;

		/// <summary>
		/// The building reference
		/// </summary>
		private Building building;

		private uint floor = 0;
		private bool isDoorOpened = false;
	}
}
using System.Collections.Generic;

namespace Assets.Scripts.Level
{
	public class TileGroup
	{
		public string name;
		public List<Coordinate> tileLocations = new List<Coordinate>();
		public enum GroupState
		{
			READY,
			ACTIVE,
			INACTIVE
		}
		public GroupState CurrentState = GroupState.READY;

		public TileGroup(string name)
		{
			this.name = name;
		}
	}
}
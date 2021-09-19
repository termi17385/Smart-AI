namespace SmartAI.Doors
{
	public class LockedDoor : BaseDoor
	{
		public bool locked = true;
		protected override void Awake()
		{
			locked = true;
			base.Awake();
		}
		public override void OpenDoor(bool _open) { if(!locked) base.OpenDoor(_open); }
	}
}
namespace AOPTest.DuckTyping
{
	public class Vector3
	{
		public Vector3(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }
	}
}
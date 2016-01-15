namespace Mixins.Method2
{
	public class Human : Animal, MAgeProvider
	{
		public string Name;
		public Human(string name)
		{
			Name = name;
		}

		// Nothing needed in here to implement MAgeProvider.
	}
}
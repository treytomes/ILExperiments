namespace AOPTest.Aspect02
{
	public interface ITestCatcher
	{
		[LogBefore]
		[LogAfter]
		[LogError]
		void Go();
	}
}
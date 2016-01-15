namespace AOPTest.Aspect03
{
	public interface ITestCatcher
	{
		[Logger]
		Test2 Go(int i);
	}
}
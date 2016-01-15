namespace AOPTest.Aspect01
{
	public class AspectControllerInfo
	{
		#region Constructors

		public AspectControllerInfo(AspectBase controller, IMessageMatcher matcher)
		{
			Controller = controller;
			Matcher = matcher;
		}

		#endregion

		#region Properties

		public AspectBase Controller { get; private set; }
		public IMessageMatcher Matcher { get; private set; }

		#endregion
	}
}
using System.Runtime.Remoting.Messaging;

namespace AOPTest.Aspect01
{
	public interface IMessageMatcher
	{
		bool IsMatch(IMessage msg);
	}
}
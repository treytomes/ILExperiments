using System;
using System.Runtime.Remoting.Messaging;

namespace AOPTest.Aspect01
{
	public class AspectBase
	{
		public virtual void Begin(object o, IMessage msg)
		{
		}

		public virtual void End(object o, IMessage msg)
		{
		}

		/// <returns>True if the error has been handled.</returns>
		public virtual bool Error(Exception ex, object o, IMessage msg)
		{
			return false;
		}

		public virtual IMessage SyncProcessMessage(object o, IMessageSink sink, IMessage msg)
		{
			Begin(o, msg);
			IMethodReturnMessage returnedMessage = (IMethodReturnMessage)sink.SyncProcessMessage(msg);
			if (returnedMessage.Exception != null)
			{
				if (Error(returnedMessage.Exception, o, msg))
				{
					returnedMessage = new MethodReturnMessageWrapper(returnedMessage)
					{
						Exception = null
					};
				}
			}
			End(o, msg);
			return returnedMessage;
		}
	}
}
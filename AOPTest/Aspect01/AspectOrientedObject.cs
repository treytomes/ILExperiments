using System;
using System.Runtime.Remoting.Messaging;

namespace AOPTest.Aspect01
{
	[AspectObject]
	public class AspectOrientedObject : ContextBoundObject
	{
		public AspectOrientedObject()
		{
		}

		public virtual IMessage SyncProcessMessage(IMessageSink sink, IMessage msg)
		{
			AspectControllerInfo cinfo = AspectConfiguration.Instance.GetAssociation(GetType());
			if ((cinfo != null) && (cinfo.Controller != null))
			{
				if ((cinfo.Matcher == null) || cinfo.Matcher.IsMatch(msg))
				{
					return cinfo.Controller.SyncProcessMessage(this, sink, msg);
				}
			}

			return sink.SyncProcessMessage(msg);
		}
	}
}
using System.Diagnostics;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;

namespace AOPTest.Aspect01
{
	public class AspectProperty : IContextProperty, IContributeServerContextSink
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AOPProperty"/> class.
		/// </summary>
		public AspectProperty()
		{
		}

		/// <summary>
		/// Takes the first sink in the chain of sinks composed so far, and then chains its message sink in front of the chain already formed.
		/// </summary>
		/// <param name="nextSink">The chain of sinks composed so far.</param>
		/// <returns>The composite sink chain.</returns>
		public IMessageSink GetServerContextSink(IMessageSink nextSink)
		{
			IMessageSink logSink = new AspectMessageSink(nextSink);
			return logSink;
		}

		/// <summary>
		/// Gets the name of the property under which it will be added to the context.
		/// </summary>
		/// <value></value>
		/// <returns>
		/// The name of the property.
		/// </returns>
		/// <PermissionSet>
		/// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure"/>
		/// </PermissionSet>
		public string Name
		{
			get
			{
				return "AOP";
			}
		}

		/// <summary>
		/// Determines whether [is new context OK] [the specified CTX].
		/// </summary>
		/// <param name="ctx">The CTX.</param>
		/// <returns>
		/// 	<c>true</c> if [is new context OK] [the specified CTX]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsNewContextOK(Context ctx)
		{
			AspectProperty newContextLogProperty = ctx.GetProperty("AOP") as AspectProperty;
			if (newContextLogProperty == null)
			{
				Debug.Assert(false);
				return false;
			}
			return true;
		}

		/// <summary>
		/// Freezes the specified CTX.
		/// </summary>
		/// <param name="ctx">The CTX.</param>
		public void Freeze(Context ctx)
		{
		}
	}
}
using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;

namespace AOPTest.Aspect01
{
	public class AspectMessageSink : IMessageSink
	{
		private IMessageSink _nextSink;
		private ObjRef _realObject = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="AOPSink"/> class.
		/// </summary>
		/// <param name="nextSink">The next sink.</param>
		public AspectMessageSink(IMessageSink nextSink)
		{
			_nextSink = nextSink;
		}

		/// <summary>
		/// Gets the next message sink in the sink chain.
		/// </summary>
		/// <value></value>
		/// <returns>
		/// The next message sink in the sink chain.
		/// </returns>
		/// <exception cref="T:System.Security.SecurityException">
		/// The immediate caller makes the call through a reference to the interface and does not have infrastructure permission.
		/// </exception>
		/// <PermissionSet>
		/// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure"/>
		/// </PermissionSet>
		public IMessageSink NextSink
		{
			get
			{
				return _nextSink;
			}
		}

		/// <summary>
		/// Synchronously processes the given message.
		/// </summary>
		/// <param name="msg">The message to process.</param>
		/// <returns>
		/// A reply message in response to the request.
		/// </returns>
		/// <exception cref="T:System.Security.SecurityException">
		/// The immediate caller makes the call through a reference to the interface and does not have infrastructure permission.
		/// </exception>
		public IMessage SyncProcessMessage(IMessage msg)
		{
			IMethodMessage methodMessage = (IMethodMessage)msg;
			if (methodMessage.MethodName == ".ctor")
			{
				IMethodReturnMessage ret = (IMethodReturnMessage)_nextSink.SyncProcessMessage(msg);
				_realObject = (ObjRef)ret.ReturnValue;
				return ret;
			}
			else
			{
				AspectOrientedObject obj = (AspectOrientedObject)RemotingServices.Unmarshal(_realObject);
				return obj.SyncProcessMessage(_nextSink, msg);
			}
		}

		/// <summary>
		/// Asynchronously processes the given message.
		/// </summary>
		/// <param name="msg">The message to process.</param>
		/// <param name="replySink">The reply sink for the reply message.</param>
		/// <returns>
		/// Returns an <see cref="T:System.Runtime.Remoting.Messaging.IMessageCtrl"/> interface that provides a way to control asynchronous messages after they have been dispatched.
		/// </returns>
		/// <exception cref="T:System.Security.SecurityException">
		/// The immediate caller makes the call through a reference to the interface and does not have infrastructure permission.
		/// </exception>
		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			throw new NotSupportedException();
		}
	}
}
using System;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;

namespace AOPTest.Aspect01
{
	[AttributeUsage(AttributeTargets.Class)]
	public class AspectObjectAttribute : ContextAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AOPAttribute"/> class.
		/// </summary>
		public AspectObjectAttribute()
			: base("AOPAttribute")
		{
		}

		/// <summary>
		/// Adds the current context property to the given message.
		/// </summary>
		/// <param name="ctorMsg">The <see cref="T:System.Runtime.Remoting.Activation.IConstructionCallMessage"/> to which to add the context property.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="ctorMsg"/> parameter is null.
		/// </exception>
		/// <PermissionSet>
		/// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure"/>
		/// </PermissionSet>
		public override void GetPropertiesForNewContext(IConstructionCallMessage ctorMsg)
		{
			if (AspectConfiguration.Instance.Enabled)
			{
				ctorMsg.ContextProperties.Add(new AspectProperty());
			}
		}

		/// <summary>
		/// Returns a Boolean value indicating whether the context parameter meets the context attribute's requirements.
		/// </summary>
		/// <param name="ctx">The context in which to check.</param>
		/// <param name="ctorMsg">The <see cref="T:System.Runtime.Remoting.Activation.IConstructionCallMessage"/> to which to add the context property.</param>
		/// <returns>
		/// true if the passed in context is okay; otherwise, false.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// Either <paramref name="ctx"/> or <paramref name="ctorMsg"/> is null.
		/// </exception>
		/// <PermissionSet>
		/// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure"/>
		/// </PermissionSet>
		public override bool IsContextOK(Context ctx, IConstructionCallMessage ctorMsg)
		{
			return false;
		}
	}
}
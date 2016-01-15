using System;
using System.Collections.Generic;

namespace AOPTest.Aspect01
{
	public class AspectConfiguration
	{
		#region Variables

		private Dictionary<Type, AspectControllerInfo> _associations;

		#endregion

		#region Constructors

		static AspectConfiguration()
		{
			Instance = new AspectConfiguration();
		}

		private AspectConfiguration()
		{
			Enabled = true;
			_associations = new Dictionary<Type, AspectControllerInfo>();
		}

		#endregion

		#region Properties

		public static AspectConfiguration Instance { get; private set; }

		public bool Enabled { get; set; }

		#endregion

		#region Methods

		public void SetAssociation<TClass, TAspect>(IMessageMatcher matcher)
			where TAspect : AspectBase
		{
			var ci = typeof(TAspect).GetConstructor(Type.EmptyTypes);
			AspectBase controller = Activator.CreateInstance<TAspect>() as AspectBase;
			_associations[typeof(TClass)] = new AspectControllerInfo(controller, matcher);
		}

		public void SetAssociation<TClass, TAspect>()
			where TAspect : AspectBase
		{
			SetAssociation<TClass, TAspect>(null);
		}

		public AspectControllerInfo GetAssociation(Type classType)
		{
			if (_associations.ContainsKey(classType))
			{
				return _associations[classType] as AspectControllerInfo;
			}
			return null;
		}

		#endregion
	}
}
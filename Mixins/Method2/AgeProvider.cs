using System;
using System.Runtime.CompilerServices;

namespace Mixins.Method2
{
	/// <summary>
	/// Implement the mixin using extensions methods.
	/// </summary>
	/// <remarks>
	/// I might move this class into the same file as MAgeProvider, to make it easier to read.
	/// </remarks>
	public static class AgeProvider
	{
		static ConditionalWeakTable<MAgeProvider, Fields> table;

		static AgeProvider()
		{
			table = new ConditionalWeakTable<MAgeProvider, Fields>();
		}

		/// <summary>
		/// Mixin's fields held in private nested class.
		/// </summary>
		private sealed class Fields
		{
			internal DateTime BirthDate = DateTime.UtcNow;
		}

		public static int GetAge(this MAgeProvider map)
		{
			DateTime dtNow = DateTime.UtcNow;
			DateTime dtBorn = table.GetOrCreateValue(map).BirthDate;
			int age = ((dtNow.Year - dtBorn.Year) * 372
					   + (dtNow.Month - dtBorn.Month) * 31
					   + (dtNow.Day - dtBorn.Day)) / 372;
			return age;
		}

		public static void SetBirthDate(this MAgeProvider map, DateTime birthDate)
		{
			table.GetOrCreateValue(map).BirthDate = birthDate;
		}
	}
}
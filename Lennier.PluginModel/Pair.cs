using System;
using System.Collections.Generic;

namespace BW.Lennier.PluginModel
{
	public sealed class Pair<T1, T2> : IEquatable<Pair<T1, T2>>
	{
		public T1 First  { get; }
		public T2 Second { get; }

		public Pair(T1 first, T2 second)
		{
			First  = first;
			Second = second;
		}

		public bool Equals(Pair<T1, T2> other)
		{
			if (other == null) return false;
			if (ReferenceEquals(this, other)) return true;
			return EqualityComparer<T1>.Default.Equals(First, other.First) && EqualityComparer<T2>.Default.Equals(Second, other.Second);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj is Pair<T1, T2> pair && Equals(pair);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (EqualityComparer<T1>.Default.GetHashCode(First) * 397) ^ EqualityComparer<T2>.Default.GetHashCode(Second);
			}
		}
	}
}

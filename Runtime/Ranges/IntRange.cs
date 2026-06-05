using UnityEngine;

namespace BasicUtilities
{
	/// <summary>
	/// This is a struct that holds a minimum and maximum int value and all features related to that.
	/// </summary>
	[System.Serializable]
	public struct IntRange
	{
        [SerializeField] private int _min;
        [SerializeField] private int _max;
		private int _dif;

		/// <summary>
		/// Minimum value of the range. If set higher than <see cref="Max"/>, the two values will swap.
		/// </summary>
		public int Min
		{
			get => _min;
			set
			{
				if (_max < value)
					(_max, _min) = (value, _max);
				else
					_min = value;
				_dif = _max - _min;
			}
		}

		/// <summary>
		/// Maximum value of the range. If set lower than <see cref="Min"/>, the two values will swap.
		/// </summary>
		public int Max
		{
			get => _max;
			set
			{
				if (_min > value)
					(_max, _min) = (_min, value);
				else
					_max = value;
				_dif = _max - _min;
			}
		}

		/// <summary>
		/// The difference between the maximum and minimum values of the range.
		/// </summary>
		public readonly int Difference => _dif;

		/// <summary>
		/// Create a new IntRange with given min and max values.
		/// </summary>
		/// <param name="min">Minimum value of the range.</param>
		/// <param name="max">Maximum value of the range.</param>
		public IntRange(int min, int max)
		{
			if (min > max)
			{
				(_max, _min) = (min, max);
			}
			else
			{
				_min = min;
				_max = max;
			}
			_dif = _max - _min;
		}

		/// <summary>
		/// Use to create a IntRange from zero to one.
		/// </summary>
		public static IntRange One => new(0, 1);

		#region Methods
		/// <summary>
		/// Gets a random value between the minimum and maximum values of the range.
		/// </summary>
		/// <returns>Returns a int inclusively in the range [Min, Max].</returns>
		public int Get() => Random.Range(Min, Max);

		/// <summary>
		/// Checks if a given value is within the range inclusively.
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <returns>true if the value is within the range; otherwise, false.</returns>
		public bool Contains(int value) => value >= Min && value <= Max;
		#endregion

		#region Overrides
		/// <summary>
		/// Returns a string representation of the IntRange.
		/// </summary>
		/// <returns>A string in the format "[Min, Max]".</returns>
		public override string ToString() => $"[{Min}, {Max}]";

		/// <summary>
		/// Determines whether the specified object is equal to the current IntRange.
		/// </summary>
		/// <param name="obj">The object to compare with the current IntRange.</param>
		/// <returns>true if the specified object is equal to the current IntRange; otherwise, false.</returns>
		public override bool Equals(object obj) => obj is IntRange range && Min == range.Min && Max == range.Max;
		
		/// <summary>
		/// Returns a hash code for the current IntRange.
		/// </summary>
		/// <returns>A hash code for the current IntRange.</returns>
		public override int GetHashCode() => (Min, Max).GetHashCode();
		#endregion

		#region Operators
		public static implicit operator Vector2Int(IntRange range) => new(range.Min, range.Max);
		public static implicit operator IntRange(Vector2Int vector) => new(vector.x, vector.y);
		public static bool operator ==(IntRange left, IntRange right) => left.Equals(right);
		public static bool operator !=(IntRange left, IntRange right) => !left.Equals(right);
		#endregion
	}
}

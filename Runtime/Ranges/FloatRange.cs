using UnityEngine;

namespace BasicUtilities
{
	/// <summary>
	/// This is a struct that holds a minimum and maximum float value and all features related to that.
	/// </summary>
	[System.Serializable]
	public struct FloatRange
	{
        [SerializeField] private float _min;
        [SerializeField] private float _max;
		private float _dif;

		/// <summary>
		/// Minimum value of the range. If set higher than <see cref="Max"/>, the two values will swap.
		/// </summary>
		public float Min
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
		public float Max
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
		public readonly float Difference => _dif;

		/// <summary>
		/// Create a new FloatRange with given min and max values.
		/// </summary>
		/// <param name="min">Minimum value of the range.</param>
		/// <param name="max">Maximum value of the range.</param>
		public FloatRange(float min, float max)
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
		/// Use to create a FloatRange from zero to one.
		/// </summary>
		public static FloatRange One => new(0, 1);

		#region Methods
		/// <summary>
		/// Gets a random value between the minimum and maximum values of the range.
		/// </summary>
		/// <returns>Returns a float inclusively in the range [Min, Max].</returns>
		public float Get() => Random.Range(Min, Max);

		/// <summary>
		/// Checks if a given value is within the range inclusively.
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <returns>true if the value is within the range; otherwise, false.</returns>
		public bool Contains(float value) => value >= Min && value <= Max;
		#endregion

		#region Overrides
		/// <summary>
		/// Returns a string representation of the FloatRange.
		/// </summary>
		/// <returns>A string in the format "[Min, Max]".</returns>
		public override string ToString() => $"[{Min}, {Max}]";

		/// <summary>
		/// Determines whether the specified object is equal to the current FloatRange.
		/// </summary>
		/// <param name="obj">The object to compare with the current FloatRange.</param>
		/// <returns>true if the specified object is equal to the current FloatRange; otherwise, false.</returns>
		public override bool Equals(object obj) => obj is FloatRange range && Min == range.Min && Max == range.Max;
		
		/// <summary>
		/// Returns a hash code for the current FloatRange.
		/// </summary>
		/// <returns>A hash code for the current FloatRange.</returns>
		public override int GetHashCode() => (Min, Max).GetHashCode();
		#endregion

		#region Operators
		public static implicit operator Vector2(FloatRange range) => new(range.Min, range.Max);
		public static implicit operator FloatRange(Vector2 vector) => new(vector.x, vector.y);
		public static bool operator ==(FloatRange left, FloatRange right) => left.Equals(right);
		public static bool operator !=(FloatRange left, FloatRange right) => !left.Equals(right);
		#endregion
	}
}

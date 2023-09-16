namespace Toolbox.Extensions
{
	public static class FloatExtentions
	{
		/// <summary>
		/// Remaps float value from one range of another
		/// </summary>
		/// <param name="value"></param>
		/// <param name="from1"></param>
		/// <param name="to1"></param>
		/// <param name="from2"></param>
		/// <param name="to2"></param>
		/// <returns>float</returns>
		public static float Remap(this float value, float from1, float to1, float from2, float to2)
		{
			return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
		}
	}
}
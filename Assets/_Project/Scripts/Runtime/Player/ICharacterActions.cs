namespace Project
{
	public interface ICharacterActions
	{
		public void Jump(float jumpForce, float jumpDuration);
		public void Dash(float dashForce, float dashDuration);
	}
}
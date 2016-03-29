using System;
namespace ClientSuperSearch
{
	internal class And
	{
		private string _LHS;
		private string _RHS;
		public string LHS
		{
			get
			{
				return this._LHS;
			}
		}
		public string RHS
		{
			get
			{
				return this._RHS;
			}
		}
		public And(string LHS, string RHS)
		{
			this._LHS = LHS;
			this._RHS = RHS;
		}
	}
}

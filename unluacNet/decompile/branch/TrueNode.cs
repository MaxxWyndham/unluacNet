namespace unluac.decompile.branch
{

	using Constant = unluac.decompile.Constant;
	using Registers = unluac.decompile.Registers;
	using ConstantExpression = unluac.decompile.expression.ConstantExpression;
	using Expression = unluac.decompile.expression.Expression;
	using LBoolean = unluac.parse.LBoolean;

	public class TrueNode : Branch
	{

	  public readonly int register;
	  public readonly bool _invert;

	  public TrueNode(int register, bool _invert, int line, int begin, int end) : base(line, begin, end)
	  {
		this.register = register;
		this._invert = _invert;
		this.setTarget = register;
	//isTest = true;
	  }

	  public override Branch invert()
	  {
		return new TrueNode(register, !_invert, line, end, begin);
	  }

	  public override int getRegister()
	  {
		return register;
	  }

	  public override Expression asExpression(Registers r)
	  {
		return new ConstantExpression(new Constant(_invert ? LBoolean.LTRUE : LBoolean.LFALSE), -1);
	  }

	  public override void useExpression(Expression expression)
	  {
	// Do nothing 
	  }

	  public override string ToString()
	  {
		return "TrueNode[invert=" + _invert + ";line=" + line + ";begin=" + begin + ";end=" + end + "]";
	  }

	}
}
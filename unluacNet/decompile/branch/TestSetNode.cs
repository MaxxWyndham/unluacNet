namespace unluac.decompile.branch
{

	using Registers = unluac.decompile.Registers;
	using Expression = unluac.decompile.expression.Expression;

	public class TestSetNode : Branch
	{

	  public readonly int test;
	  public readonly bool _invert;

	  public TestSetNode(int target, int test, bool invert, int line, int begin, int end) : base(line, begin, end)
	  {
		this.test = test;
		this._invert = invert;
		this.setTarget = target;
	  }

	  public override Branch invert()
	  {
		return new TestSetNode(setTarget, test, !_invert, line, end, begin);
	  }

	  public override int getRegister()
	  {
		return setTarget;
	  }

	  public override Expression asExpression(Registers r)
	  {
		return r.getExpression(test, line);
	  }

	  public override void useExpression(Expression expression)
	  {
	// Do nothing 
	  }

	  public override string ToString()
	  {
		return "TestSetNode[target=" + setTarget + ";test=" + test + ";invert=" + _invert + ";line=" + line + ";begin=" + begin + ";end=" + end + "]";
	  }

	}
}
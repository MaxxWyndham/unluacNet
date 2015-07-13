namespace unluac.decompile.branch
{

	using Registers = unluac.decompile.Registers;
	using Expression = unluac.decompile.expression.Expression;

	public class TestNode : Branch
	{

	  public readonly int test;
	  public readonly bool _invert;

	  public TestNode(int test, bool _invert, int line, int begin, int end) : base(line, begin, end)
	  {
		this.test = test;
		this._invert = _invert;
		isTest = true;
	  }

	  public override Branch invert()
	  {
		return new TestNode(test, !_invert, line, end, begin);
	  }

	  public override int getRegister()
	  {
		return test;
	  }

	  public override Expression asExpression(Registers r)
	  {
		if(_invert)
		{
		  return (new NotBranch(this.invert())).asExpression(r);
		}
		else
		{
		  return r.getExpression(test, line);
		}
	  }

	  public override void useExpression(Expression expression)
	  {
	// Do nothing 
	  }

	  public override string ToString()
	  {
		return "TestNode[test=" + test + ";invert=" + _invert + ";line=" + line + ";begin=" + begin + ";end=" + end + "]";
	  }

	}
}
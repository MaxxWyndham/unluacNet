namespace unluac.decompile.branch
{

	using Registers = unluac.decompile.Registers;
	using Expression = unluac.decompile.expression.Expression;

	abstract public class Branch
	{

	  public readonly int line;
	  public int begin;
	  public int end; //Might be modified to undo redirect

	  public bool isSet = false;
	  public bool isCompareSet = false;
	  public bool isTest = false;
	  public int setTarget = -1;

	  public Branch(int line, int begin, int end)
	  {
		this.line = line;
		this.begin = begin;
		this.end = end;
	  }

	  abstract public Branch invert();

	  abstract public int getRegister();

	  abstract public Expression asExpression(Registers r);

	  abstract public void useExpression(Expression expression);

	}

}
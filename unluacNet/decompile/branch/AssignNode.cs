using System;
namespace unluac.decompile.branch
{

	using Registers = unluac.decompile.Registers;
	using Expression = unluac.decompile.expression.Expression;
	using Assignment = unluac.decompile.statement.Assignment;

	public class AssignNode : Branch
	{

	  private Expression expression;

	  public AssignNode(int line, int begin, int end) : base(line, begin, end)
	  {
	  }

	  public override Branch invert()
	  {
		throw new Exception();
	  }

	  public override int getRegister()
	  {
		throw new Exception();
	  }

	  public override Expression asExpression(Registers r)
	  {
		return expression;
	  }

	  public override void useExpression(Expression expression)
	  {
		this.expression = expression;
	  }

	}

}
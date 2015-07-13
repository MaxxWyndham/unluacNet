namespace unluac.decompile.branch
{

	using Registers = unluac.decompile.Registers;
	using Expression = unluac.decompile.expression.Expression;
	using UnaryExpression = unluac.decompile.expression.UnaryExpression;

	public class NotBranch : Branch
	{

	  private readonly Branch branch;

	  public NotBranch(Branch branch) : base(branch.line, branch.begin, branch.end)
	  {
		this.branch = branch;
	  }

	  public override Branch invert()
	  {
		return branch;
	  }

	  public override int getRegister()
	  {
		return branch.getRegister();
	  }

	  public override Expression asExpression(Registers r)
	  {
		return new UnaryExpression("not ", branch.asExpression(r), Expression.PRECEDENCE_UNARY);
	  }

	  public override void useExpression(Expression expression)
	  {
	// Do nothing 
	  }

	}

}
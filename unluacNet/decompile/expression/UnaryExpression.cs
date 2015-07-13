namespace unluac.decompile.expression
{

	using Output = unluac.decompile.Output;

	public class UnaryExpression : Expression
	{

	  private readonly string op;
	  private readonly Expression expression;

	  public UnaryExpression(string op, Expression expression, int precedence) : base(precedence)
	  {
		this.op = op;
		this.expression = expression;
	  }

	  public override int getConstantIndex()
	  {
		return expression.getConstantIndex();
	  }

	  public override void print(Output @out)
	  {
		@out.print(op);
		if(precedence > expression.precedence)
			@out.print("(");
		expression.print(@out);
		if(precedence > expression.precedence)
			@out.print(")");
	  }

	}

}
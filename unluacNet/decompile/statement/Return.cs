using System.Collections.Generic;

namespace unluac.decompile.statement
{


	using Output = unluac.decompile.Output;
	using Expression = unluac.decompile.expression.Expression;

	public class Return : Statement
	{

	  private Expression[] values;

	  public Return()
	  {
		values = new Expression[0];
	  }

	  public Return(Expression @value)
	  {
		values = new Expression[1];
		values[0] = @value;
	  }

	  public Return(Expression[] values)
	  {
		this.values = values;
	  }

	  public override void print(Output @out)
	  {
		@out.print("do ");
		printTail(@out);
		@out.print(" end");
	  }

	  public override void printTail(Output @out)
	  {
		@out.print("return");
		if(values.Length > 0)
		{
		  @out.print(" ");
		  List<Expression> rtns = new List<Expression>(values.Length);
		  foreach(Expression value in values)
		  {
			rtns.Add(value);
		  }
		  Expression.printSequence(@out, rtns, false, true);
		}
	  }

	}

}
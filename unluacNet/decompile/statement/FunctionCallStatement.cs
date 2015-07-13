namespace unluac.decompile.statement
{

	using Output = unluac.decompile.Output;
	using FunctionCall = unluac.decompile.expression.FunctionCall;

	public class FunctionCallStatement : Statement
	{

	  private FunctionCall call;

	  public FunctionCallStatement(FunctionCall call)
	  {
		this.call = call;
	  }

	  public override void print(Output @out)
	  {
		call.print(@out);
	  }

	  public override bool beginsWithParen()
	  {
		return call.beginsWithParen();
	  }

	}

}
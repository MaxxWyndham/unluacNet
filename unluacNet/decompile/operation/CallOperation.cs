namespace unluac.decompile.operation
{

	using Registers = unluac.decompile.Registers;
	using Block = unluac.decompile.block.Block;
	using FunctionCall = unluac.decompile.expression.FunctionCall;
	using FunctionCallStatement = unluac.decompile.statement.FunctionCallStatement;
	using Statement = unluac.decompile.statement.Statement;

	public class CallOperation : Operation
	{

	  private FunctionCall call;

	  public CallOperation(int line, FunctionCall call) : base(line)
	  {
		this.call = call;
	  }

	  public override Statement process(Registers r, Block block)
	  {
		return new FunctionCallStatement(call);
	  }

	}

}
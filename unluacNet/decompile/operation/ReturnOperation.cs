namespace unluac.decompile.operation
{

	using Registers = unluac.decompile.Registers;
	using Block = unluac.decompile.block.Block;
	using Expression = unluac.decompile.expression.Expression;
	using Return = unluac.decompile.statement.Return;
	using Statement = unluac.decompile.statement.Statement;

	public class ReturnOperation : Operation
	{

	  private Expression[] values;

	  public ReturnOperation(int line, Expression @value) : base(line)
	  {
		values = new Expression[1];
		values[0] = @value;
	  }

	  public ReturnOperation(int line, Expression[] values) : base(line)
	  {
		this.values = values;
	  }

	  public override Statement process(Registers r, Block block)
	  {
		return new Return(values);
	  }

	}

}
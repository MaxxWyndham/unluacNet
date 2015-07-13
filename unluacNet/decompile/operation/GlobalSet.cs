namespace unluac.decompile.operation
{

	using Registers = unluac.decompile.Registers;
	using Block = unluac.decompile.block.Block;
	using Expression = unluac.decompile.expression.Expression;
	using Assignment = unluac.decompile.statement.Assignment;
	using Statement = unluac.decompile.statement.Statement;
	using GlobalTarget = unluac.decompile.target.GlobalTarget;

	public class GlobalSet : Operation
	{

	  private string global;
	  private Expression @value;

	  public GlobalSet(int line, string global, Expression @value) : base(line)
	  {
		this.global = global;
		this.value = @value;
	  }

	  public override Statement process(Registers r, Block block)
	  {
		return new Assignment(new GlobalTarget(global), @value);
	  }

	}

}
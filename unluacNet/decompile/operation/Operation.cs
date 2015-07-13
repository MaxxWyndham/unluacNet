namespace unluac.decompile.operation
{

	using Registers = unluac.decompile.Registers;
	using Block = unluac.decompile.block.Block;
	using Statement = unluac.decompile.statement.Statement;

	abstract public class Operation
	{

	  public readonly int line;

	  public Operation(int line)
	  {
		this.line = line;
	  }

	  abstract public Statement process(Registers r, Block block);

	}

}
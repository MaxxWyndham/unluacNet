namespace unluac.decompile.operation
{

	using Registers = unluac.decompile.Registers;
	using Block = unluac.decompile.block.Block;
	using Expression = unluac.decompile.expression.Expression;
	using Assignment = unluac.decompile.statement.Assignment;
	using Statement = unluac.decompile.statement.Statement;
	using UpvalueTarget = unluac.decompile.target.UpvalueTarget;

	public class UpvalueSet : Operation
	{

	  private UpvalueTarget target;
	  private Expression @value;

	  public UpvalueSet(int line, string upvalue, Expression @value) : base(line)
	  {
		target = new UpvalueTarget(upvalue);
		this.value = @value;
	  }

	  public override Statement process(Registers r, Block block)
	  {
		return new Assignment(target, @value);
	  }

	}

}
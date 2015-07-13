namespace unluac.decompile.operation
{

	using Registers = unluac.decompile.Registers;
	using Block = unluac.decompile.block.Block;
	using Expression = unluac.decompile.expression.Expression;
	using Assignment = unluac.decompile.statement.Assignment;
	using Statement = unluac.decompile.statement.Statement;

	public class RegisterSet : Operation
	{

	  public readonly int register;
	  public readonly Expression @value;

	  public RegisterSet(int line, int register, Expression @value) : base(line)
	  {
		this.register = register;
		this.value = @value;
//    
//    if(value.isMultiple()) {
//      System.out.println("-- multiple @" + register);
//    }
//    
	  }

	  public override Statement process(Registers r, Block block)
	  {
	//System.out.println("-- processing register set " + register + "@" + line);
		r.setValue(register, line, @value);
//    
//    if(value.isMultiple()) {
//      System.out.println("-- process multiple @" + register);
//    }
//    
		if(r.isAssignable(register, line))
		{
	  //System.out.println("-- assignment!");
		  return new Assignment(r.getTarget(register, line), @value);
		}
		else
		{
		  return null;
		}
	  }

	}

}
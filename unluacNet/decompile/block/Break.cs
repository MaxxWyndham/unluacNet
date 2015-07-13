using System;
namespace unluac.decompile.block
{

	using Output = unluac.decompile.Output;
	using Statement = unluac.decompile.statement.Statement;
	using LFunction = unluac.parse.LFunction;

	public class Break : Block
	{

	  public readonly int target;

	  public Break(LFunction function, int line, int target) : base(function, line, line)
	  {
		this.target = target;
	  }

	  public override void addStatement(Statement statement)
	  {
		throw new Exception();
	  }

	  public override bool isContainer()
	  {
		return false;
	  }

	  public override bool breakable()
	  {
		return false;
	  }

	  public override bool isUnprotected()
	  {
	//Actually, it is unprotected, but not really a block
		return false;
	  }

	  public override int getLoopback()
	  {
          throw new Exception();
	  }

	  public override void print(Output @out)
	  {
		@out.print("do break end");
	  }

	  public override void printTail(Output @out)
	  {
		@out.print("break");
	  }

	}

}
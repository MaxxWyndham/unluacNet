using System;
namespace unluac.decompile.block
{

	using Output = unluac.decompile.Output;
	using Statement = unluac.decompile.statement.Statement;
	using LFunction = unluac.parse.LFunction;

	public class BooleanIndicator : Block
	{

	  public BooleanIndicator(LFunction function, int line) : base(function, line, line)
	  {
	  }

	  public override void addStatement(Statement statement)
	  {

	  }

	  public override bool isContainer()
	  {
		return false;
	  }

	  public override bool isUnprotected()
	  {
		return false;
	  }

	  public override bool breakable()
	  {
		return false;
	  }

	  public override int getLoopback()
	  {
          throw new Exception();
	  }

	  public override void print(Output @out)
	  {
		@out.print("-- unhandled boolean indicator");
	  }

	}

}
using System;
using System.Collections.Generic;

namespace unluac.decompile.block
{


	using Output = unluac.decompile.Output;
	using Statement = unluac.decompile.statement.Statement;
	using LFunction = unluac.parse.LFunction;

	public class DoEndBlock : Block
	{

	  private readonly List<Statement> statements;

	  public DoEndBlock(LFunction function, int begin, int end) : base(function, begin, end)
	  {
		statements = new List<Statement>(end - begin + 1);
	  }

	  public override void addStatement(Statement statement)
	  {
		statements.Add(statement);
	  }

	  public override bool breakable()
	  {
		return false;
	  }

	  public override bool isContainer()
	  {
		return true;
	  }

	  public override bool isUnprotected()
	  {
		return false;
	  }

	  public override int getLoopback()
	  {
          throw new Exception();
	  }

	  public override void print(Output @out)
	  {
		@out.println("do");
		@out.indent();
		Statement.printSequence(@out, statements);
		@out.dedent();
		@out.print("end");
	  }

	}

}
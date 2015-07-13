using System;
using System.Collections.Generic;

namespace unluac.decompile.block
{


	using Output = unluac.decompile.Output;
	using Return = unluac.decompile.statement.Return;
	using Statement = unluac.decompile.statement.Statement;
	using LFunction = unluac.parse.LFunction;

	public class OuterBlock : Block
	{

	  private readonly List<Statement> statements;

	  public OuterBlock(LFunction function, int length) : base(function, 0, length + 1)
	  {
		statements = new List<Statement>(length);
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

	  public override int scopeEnd()
	  {
		return (end - 1) + function.header.version.getOuterBlockScopeAdjustment();
	  }

	  public override void print(Output @out)
	  {
	// extra return statement 
		int last = statements.Count - 1;
        if (last < 0) throw new Exception("OuterBlock has no statements!");
		if(/*last < 0 ||*/ !(statements[last] is Return))
		{
            throw new Exception(statements[last].ToString());
		}
		statements.RemoveAt(last);
		Statement.printSequence(@out, statements);
	  }

	}

}
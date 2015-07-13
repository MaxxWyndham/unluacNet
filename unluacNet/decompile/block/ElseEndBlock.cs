using System;
using System.Collections.Generic;

namespace unluac.decompile.block
{


	using Output = unluac.decompile.Output;
	using Statement = unluac.decompile.statement.Statement;
	using LFunction = unluac.parse.LFunction;

	public class ElseEndBlock : Block
	{

	  private readonly List<Statement> statements;
	  public IfThenElseBlock partner;

	  public ElseEndBlock(LFunction function, int begin, int end) : base(function, begin, end)
	  {
		statements = new List<Statement>(end - begin + 1);
	  }

	  public override int CompareTo(Block block)
	  {
		if(block == partner)
		{
		  return 1;
		}
		else
		{
		  int result = base.CompareTo(block);
		  if(result == 0 && block is ElseEndBlock)
		  {
			Console.WriteLine("HEY HEY HEY");
		  }
		  return result;
		}
	  }

	  public override bool breakable()
	  {
		return false;
	  }

	  public override bool isContainer()
	  {
		return true;
	  }

	  public override void addStatement(Statement statement)
	  {
		statements.Add(statement);
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
		if(statements.Count == 1 && statements[0] is IfThenEndBlock)
		{
		  @out.print("else");
		  statements[0].print(@out);
		}
		else if(statements.Count == 2 && statements[0] is IfThenElseBlock && statements[1] is ElseEndBlock)
		{
		  @out.print("else");
		  statements[0].print(@out);
		  statements[1].print(@out);
		}
		else
		{
		  @out.print("else");
		  @out.println();
		  @out.indent();
		  Statement.printSequence(@out, statements);
		  @out.dedent();
		  @out.print("end");
		}
	  }

	}
}
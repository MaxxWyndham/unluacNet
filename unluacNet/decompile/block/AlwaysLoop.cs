using System.Collections.Generic;

namespace unluac.decompile.block
{


	using Output = unluac.decompile.Output;
	using Statement = unluac.decompile.statement.Statement;
	using LFunction = unluac.parse.LFunction;

	public class AlwaysLoop : Block
	{

	  private readonly List<Statement> statements;

	  public AlwaysLoop(LFunction function, int begin, int end) : base(function, begin, end)
	  {
		statements = new List<Statement>();
	  }

	  public override int scopeEnd()
	  {
		return end - 2;
	  }

	  public override bool breakable()
	  {
		return true;
	  }

	  public override bool isContainer()
	  {
		return true;
	  }

	  public override bool isUnprotected()
	  {
		return true;
	  }

	  public override int getLoopback()
	  {
		return begin;
	  }

	  public override void print(Output @out)
	  {
		@out.println("while true do");
		@out.indent();
		Statement.printSequence(@out, statements);
		@out.dedent();
		@out.print("end");
	  }

	  public override void addStatement(Statement statement)
	  {
		statements.Add(statement);
	  }
	}
}
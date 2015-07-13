using System;
using System.Collections.Generic;

namespace unluac.decompile.block
{


	using Output = unluac.decompile.Output;
	using Registers = unluac.decompile.Registers;
	using Branch = unluac.decompile.branch.Branch;
	using Statement = unluac.decompile.statement.Statement;
	using LFunction = unluac.parse.LFunction;

	public class RepeatBlock : Block
	{

	  private readonly Branch branch;
	  private readonly Registers r;
	  private readonly List<Statement> statements;

	  public RepeatBlock(LFunction function, Branch branch, Registers r) : base(function, branch.end, branch.begin)
	  {
	//System.out.println("-- creating repeat block " + branch.end + " .. " + branch.begin);
		this.branch = branch;
		this.r = r;
		statements = new List<Statement>(branch.begin - branch.end + 1);
	  }

	  public override bool breakable()
	  {
		return true;
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
		@out.print("repeat");
		@out.println();
		@out.indent();
		Statement.printSequence(@out, statements);
		@out.dedent();
		@out.print("until ");
		branch.asExpression(r).print(@out);
	  }

	}

}
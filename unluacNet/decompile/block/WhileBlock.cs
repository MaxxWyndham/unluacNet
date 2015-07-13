using System.Collections.Generic;

namespace unluac.decompile.block
{


	using Output = unluac.decompile.Output;
	using Registers = unluac.decompile.Registers;
	using Branch = unluac.decompile.branch.Branch;
	using Statement = unluac.decompile.statement.Statement;
	using LFunction = unluac.parse.LFunction;

	public class WhileBlock : Block
	{

	  private readonly Branch branch;
	  private readonly int loopback;
	  private readonly Registers r;
	  private readonly List<Statement> statements;

	  public WhileBlock(LFunction function, Branch branch, int loopback, Registers r) : base(function, branch.begin, branch.end)
	  {
		this.branch = branch;
		this.loopback = loopback;
		this.r = r;
		statements = new List<Statement>(branch.end - branch.begin + 1);
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

	  public override void addStatement(Statement statement)
	  {
		statements.Add(statement);
	  }

	  public override bool isUnprotected()
	  {
		return true;
	  }

	  public override int getLoopback()
	  {
		return loopback;
	  }

	  public override void print(Output @out)
	  {
		@out.print("while ");
		branch.asExpression(r).print(@out);
		@out.print(" do");
		@out.println();
		@out.indent();
		Statement.printSequence(@out, statements);
		@out.dedent();
		@out.print("end");
	  }

	}
}
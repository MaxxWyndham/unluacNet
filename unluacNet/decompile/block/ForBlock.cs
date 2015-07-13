using System;
using System.Collections.Generic;

namespace unluac.decompile.block
{


	using Output = unluac.decompile.Output;
	using Registers = unluac.decompile.Registers;
	using Expression = unluac.decompile.expression.Expression;
	using Statement = unluac.decompile.statement.Statement;
	using LFunction = unluac.parse.LFunction;

	public class ForBlock : Block
	{

	  private readonly int register;
	  private readonly Registers r;
	  private readonly List<Statement> statements;

	  public ForBlock(LFunction function, int begin, int end, int register, Registers r) : base(function, begin, end)
	  {
		this.register = register;
		this.r = r;
		statements = new List<Statement>(end - begin + 1);
	  }

	  public override int scopeEnd()
	  {
		return end - 2;
	  }

	  public override void addStatement(Statement statement)
	  {
		statements.Add(statement);
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
		return false;
	  }

	  public override int getLoopback()
	  {
          throw new Exception();
	  }

	  public override void print(Output @out)
	  {
		@out.print("for ");
		r.getTarget(register + 3, begin - 1).print(@out);
		@out.print(" = ");
		r.getValue(register, begin - 1).print(@out);
		@out.print(", ");
		r.getValue(register + 1, begin - 1).print(@out);
		Expression step = r.getValue(register + 2, begin - 1);
		if(!step.isInteger() || step.asInteger() != 1)
		{
		  @out.print(", ");
		  step.print(@out);
		}
		@out.print(" do");
		@out.println();
		@out.indent();
		Statement.printSequence(@out, statements);
		@out.dedent();
		@out.print("end");
	  }

	}

}
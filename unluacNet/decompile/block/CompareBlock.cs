using System;
using unluac.decompile.operation;
using  unluac.decompile;
using unluac.decompile.statement;
using  unluac.decompile.branch;

namespace unluac.decompile.block
{

	using Decompiler = unluac.decompile.Decompiler;
	using Output = unluac.decompile.Output;
	using Registers = unluac.decompile.Registers;
	using Branch = unluac.decompile.branch.Branch;
	using Expression = unluac.decompile.expression.Expression;
	using Operation = unluac.decompile.operation.Operation;
	using RegisterSet = unluac.decompile.operation.RegisterSet;
	using Statement = unluac.decompile.statement.Statement;
	using LFunction = unluac.parse.LFunction;

	public class CompareBlock : Block
	{

	  public int target;
	  public Branch branch;

	  public CompareBlock(LFunction function, int begin, int end, int target, Branch branch) : base(function, begin, end)
	  {
		this.target = target;
		this.branch = branch;
	  }

	  public override bool isContainer()
	  {
		return false;
	  }

	  public override bool breakable()
	  {
		return false;
	  }

	  public override void addStatement(Statement statement)
	  {
	// Do nothing 
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
		@out.print("-- unhandled compare assign");
	  }

	  public override Operation process(Decompiler d)
	  {
          return new CompareBlockOperation(end - 1, end, target,branch);
//JAVA TO VB & C# CONVERTER TODO TASK: Anonymous inner classes are not converted to .NET:
		
	  }

	}
    public class CompareBlockOperation : Operation
    {
        int _end, _target;
        Branch _branch;

        public CompareBlockOperation(int line, int end, int target, Branch branch) : base(line)
        {
            _branch = branch;
            _target = target;
            _end = end;
        }
		  public override Statement process(Registers r, Block block)
		  {
			return new RegisterSet(_end - 1, _target, _branch.asExpression(r)).process(r, block);
		  }

    }

}
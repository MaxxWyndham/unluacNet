using System;

namespace unluac.decompile.block
{

	using Decompiler = unluac.decompile.Decompiler;
	using Op = unluac.decompile.Op;
	using Output = unluac.decompile.Output;
	using Registers = unluac.decompile.Registers;
	using Branch = unluac.decompile.branch.Branch;
	using Expression = unluac.decompile.expression.Expression;
	using Operation = unluac.decompile.operation.Operation;
	using RegisterSet = unluac.decompile.operation.RegisterSet;
	using Assignment = unluac.decompile.statement.Assignment;
	using Statement = unluac.decompile.statement.Statement;
	using Target = unluac.decompile.target.Target;
	using LFunction = unluac.parse.LFunction;

	public class SetBlock : Block
	{

	  public readonly int target;
	  private Assignment assign;
	  public readonly Branch branch;
	  private Registers r;
	  private bool empty;
	  private bool finalize = false;

	  public SetBlock(LFunction function, Branch branch, int target, int line, int begin, int end, bool empty, Registers r) : base(function, begin, end)
	  {
		this.empty = empty;
		if(begin == end)
			this.begin -= 1;
		this.target = target;
		this.branch = branch;
		this.r = r;
	//System.out.println("-- set block " + begin + " .. " + end);
	  }

	  public override void addStatement(Statement statement)
	  {
		if(!finalize && statement is Assignment)
		{
		  this.assign = (Assignment) statement;
		}
		else if(statement is BooleanIndicator)
		{
		  finalize = true;
		}
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
		if(assign != null && assign.getFirstTarget() != null)
		{
		  Assignment assignOut = new Assignment(assign.getFirstTarget(), Value);
		  assignOut.print(@out);
		}
		else
		{
		  @out.print("-- unhandled set block");
		  @out.println();
		}
	  }

	  public override bool breakable()
	  {
		return false;
	  }

	  public override bool isContainer()
	  {
		return false;
	  }

	  public virtual void useAssignment(Assignment assign)
	  {
		this.assign = assign;
		branch.useExpression(assign.getFirstValue());
	  }

	  public virtual Expression Value
	  {
		  get
		  {
			return branch.asExpression(r);
		  }
	  }

//JAVA TO VB & C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public Operation process(final Decompiler d)

        public class SetBlockOperation : Operation
        {
            Target _target;
            Expression _value;
            public SetBlockOperation(int line, Target target, Expression value) : base(line)
            {
                _target = target;
                _value = value;
            }

            public override Statement process(Registers r, Block block)
            {
                return new Assignment(_target, _value);
            }
        }
        public class SetBlockOperationOther : Operation
        {
            Branch branch;
            Decompiler d;
            int target;
            public SetBlockOperationOther(int line, Branch _branch, Decompiler _d, int _target) : base(line)
            {
                branch = _branch;
                d = _d;
                target = _target;
            }
            public override Statement process(Registers r, Block block)
			{
		  //System.out.println("-- block " + begin + " .. " + end);
			  Expression expr = null;
			  int register = 0;
			  for(; register < r.registers; register++)
			  {
				if(r.getUpdated(register, branch.end - 1) == branch.end - 1)
				{
				  expr = r.getValue(register, branch.end);
				  break;
				}
			  }
			  if(d.code.op(branch.end - 2) == Op.LOADBOOL && d.code.C(branch.end - 2) != 0)
			  {
				int target1 = d.code.A(branch.end - 2);
				if(d.code.op(branch.end - 3) == Op.JMP && d.code.sBx(branch.end - 3) == 2)
				{
			  //System.out.println("-- Dropped boolean expression operand");
				  expr = r.getValue(target1, branch.end - 2);
				}
				else
				{
				  expr = r.getValue(target1, branch.begin);
				}
				branch.useExpression(expr);
				if(r.isLocal(target1, branch.end - 1))
				{
				  return new Assignment(r.getTarget(target1, branch.end - 1), branch.asExpression(r));
				}
				r.setValue(target1, branch.end - 1, branch.asExpression(r));
				}
			  else if(expr != null && target >= 0)
			  {
				branch.useExpression(expr);
				if(r.isLocal(target, branch.end - 1))
				{
				  return new Assignment(r.getTarget(target, branch.end - 1), branch.asExpression(r));
				}
				r.setValue(target, branch.end - 1, branch.asExpression(r));
			//System.out.println("-- target = " + target + "@" + (branch.end - 1));
			//.print(new Output());
			//System.out.println();
				}
			  else
			  {
				Console.WriteLine("-- fail " + (branch.end - 1));
				Console.WriteLine(expr);
				Console.WriteLine(target);
			  }
			  return null;
			}
        }
	  public override Operation process(Decompiler d)
	  {
		if(empty)
		{
          
		  Expression expression = r.getExpression(branch.setTarget, end);
		  branch.useExpression(expression);
		  return new RegisterSet(end - 1, branch.setTarget, branch.asExpression(r));
		}
		else if(assign != null)
		{
		  branch.useExpression(assign.getFirstValue());
		  Target target1 = assign.getFirstTarget();
		  Expression @value = Value;
		  return new SetBlockOperation(end - 1, target1, @value);
			}
		else
		{
		  return new SetBlockOperationOther(end - 1,branch,d,target);
//JAVA TO VB & C# CONVERTER TODO TASK: Anonymous inner classes are not converted to .NET:
		  
	  //return super.process();
		}
//    
//    if(sblock.branch.begin == sblock.branch.end && r.isLocal(sblock.target, line)) {
//      sblock.useAssignment(new Assignment(r.getTarget(sblock.target, line), r.getExpression(sblock.target, line)));
//    } else if(sblock.branch.begin == sblock.branch.end) {
//      sblock.useAssignment(new Assignment(null, r.getExpression(sblock.target, sblock.begin)));
//      r.setValue(sblock.target, line, sblock.getValue());
//    } else {
//      //System.out.println("--sblock");
//      Expression expr = null;
//      int register = 0;
//      for(; register < registers; register++) {
//        if(updated[register] == line) {
//          expr = r.getValue(register, line + 1);
//          break;
//        }
//      }
//      if(expr == null) {
//        //System.out.println("-- null/null");
//        expr = r.getExpression(sblock.target, line);
//      }
//      sblock.useAssignment(new Assignment(null, expr));
//      r.setValue(sblock.target, line, sblock.getValue());
//    }
//  }
//  
	  }

	}

}
using System.Collections.Generic;
using System;
using unluac.util;

namespace unluac.decompile.block
{


	using Declaration = unluac.decompile.Declaration;
	using Decompiler = unluac.decompile.Decompiler;
	using Output = unluac.decompile.Output;
	using Registers = unluac.decompile.Registers;
	using Branch = unluac.decompile.branch.Branch;
	using TestNode = unluac.decompile.branch.TestNode;
	using BinaryExpression = unluac.decompile.expression.BinaryExpression;
	using Expression = unluac.decompile.expression.Expression;
	using LocalVariable = unluac.decompile.expression.LocalVariable;
	using Operation = unluac.decompile.operation.Operation;
	using Assignment = unluac.decompile.statement.Assignment;
	using Statement = unluac.decompile.statement.Statement;
	using LFunction = unluac.parse.LFunction;
	//using Stack = .Stack;

	public class IfThenEndBlock : Block
	{

	  private readonly Branch branch;
	  private readonly unluac.util.Stack<Branch> stack;
	  private readonly Registers r;
	  private readonly List<Statement> statements;

	  public IfThenEndBlock(LFunction function, Branch branch, Registers r) : this(function, branch, null, r)
	  {
	  }

	  public IfThenEndBlock(LFunction function, Branch branch, unluac.util.Stack<Branch> stack, Registers r) : base(function, branch.begin == branch.end ? branch.begin - 1 : branch.begin, branch.begin == branch.end ? branch.begin - 1 : branch.end)
	  {
		this.branch = branch;
		this.stack = stack;
		this.r = r;
		statements = new List<Statement>(branch.end - branch.begin + 1);
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
		@out.print("if ");
		branch.asExpression(r).print(@out);
		@out.print(" then");
		@out.println();
		@out.indent();
		Statement.printSequence(@out, statements);
		@out.dedent();
		@out.print("end");
	  }

        public class IfThenEndBlockOperation : Operation
        {
            Assignment assign;
            Expression expr;
            public IfThenEndBlockOperation(int line, Assignment _assign, Expression _expr) : base(line)
            {
                assign = _assign;
                expr = _expr;
            }
            public override Statement process(Registers r, Block block)
            {
                return new Assignment(assign.getFirstTarget(), expr);
            }
        }
	  public override Operation process(Decompiler d)
	  {
		if(statements.Count == 1)
		{
		  Statement stmt = statements[0];
		  if(stmt is Assignment)
		  {
			Assignment assign = (Assignment) stmt;
			if(assign.getArity() == 1)
			{
			  if(branch is TestNode)
			  {
				TestNode node = (TestNode) branch;
				Declaration decl = r.getDeclaration(node.test, node.line);
				if(assign.getFirstTarget().isDeclaration(decl))
				{
				  Expression expr;
				  if(node._invert)
				  {
					expr = new BinaryExpression("or", new LocalVariable(decl), assign.getFirstValue(), Expression.PRECEDENCE_OR, Expression.ASSOCIATIVITY_NONE);
				  }
				  else
				  {
					expr = new BinaryExpression("and", new LocalVariable(decl), assign.getFirstValue(), Expression.PRECEDENCE_AND, Expression.ASSOCIATIVITY_NONE);
				  }
                  return new IfThenEndBlockOperation(end - 1, assign, expr);
//JAVA TO VB & C# CONVERTER TODO TASK: Anonymous inner classes are not converted to .NET:
				  /*{

					public Statement process(Registers r, Block block)
					{
					  return new Assignment(assign.getFirstTarget(), expr);
					}

				  }*/
				}
			  }
			}
		  }
					}
		else if(statements.Count == 0 && stack != null)
		{
		  int test = branch.getRegister();
		  if(test < 0)
		  {
			for(int reg = 0; reg < r.registers; reg++)
			{
			  if(r.getUpdated(reg, branch.end - 1) >= branch.begin)
			  {
				if(test >= 0)
				{
				  test = -1;
				  break;
				}
				test = reg;
			  }
			}
		  }
		  if(test >= 0)
		  {
			if(r.getUpdated(test, branch.end - 1) >= branch.begin)
			{
			  Expression right = r.getValue(test, branch.end);
			  Branch setb = d.popSetCondition(stack, stack.peek().end);
			  setb.useExpression(right);
			  int testreg = test;
              return new IfThenEndBlockOperation2(end - 1, testreg, branch, setb);
//JAVA TO VB & C# CONVERTER TODO TASK: Anonymous inner classes are not converted to .NET:
			  /*{

				public Statement process(Registers r, Block block)
				{
				  r.setValue(testreg, branch.end - 1, setb.asExpression(r));
				  return null;
				}

			  }*/
			}
		  }
		}
		return base.process(d);
	  }
        public class IfThenEndBlockOperation2 : Operation
        {
            int testreg;
            Branch branch;
            Branch setb;

            public IfThenEndBlockOperation2(int line, int _testreg, Branch _branch, Branch _setb) : base(line)
            {
                testreg = _testreg;
                branch = _branch;
                setb = _setb;
            }

            public override Statement process(Registers r, Block block)
            {
                r.setValue(testreg, branch.end - 1, setb.asExpression(r));
                return null;
            }
        }

	}
}
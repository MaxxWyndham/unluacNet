using System;
using System.Collections.Generic;

namespace unluac.decompile
{


	using AlwaysLoop = unluac.decompile.block.AlwaysLoop;
	using Block = unluac.decompile.block.Block;
	using BooleanIndicator = unluac.decompile.block.BooleanIndicator;
	using Break = unluac.decompile.block.Break;
	using CompareBlock = unluac.decompile.block.CompareBlock;
	using DoEndBlock = unluac.decompile.block.DoEndBlock;
	using ElseEndBlock = unluac.decompile.block.ElseEndBlock;
	using ForBlock = unluac.decompile.block.ForBlock;
	using IfThenElseBlock = unluac.decompile.block.IfThenElseBlock;
	using IfThenEndBlock = unluac.decompile.block.IfThenEndBlock;
	using OuterBlock = unluac.decompile.block.OuterBlock;
	using RepeatBlock = unluac.decompile.block.RepeatBlock;
	using SetBlock = unluac.decompile.block.SetBlock;
	using TForBlock = unluac.decompile.block.TForBlock;
	using WhileBlock = unluac.decompile.block.WhileBlock;
	using AndBranch = unluac.decompile.branch.AndBranch;
	using AssignNode = unluac.decompile.branch.AssignNode;
	using Branch = unluac.decompile.branch.Branch;
	using EQNode = unluac.decompile.branch.EQNode;
	using LENode = unluac.decompile.branch.LENode;
	using LTNode = unluac.decompile.branch.LTNode;
	using OrBranch = unluac.decompile.branch.OrBranch;
	using TestNode = unluac.decompile.branch.TestNode;
	using TestSetNode = unluac.decompile.branch.TestSetNode;
	using TrueNode = unluac.decompile.branch.TrueNode;
	using ClosureExpression = unluac.decompile.expression.ClosureExpression;
	using ConstantExpression = unluac.decompile.expression.ConstantExpression;
	using Expression = unluac.decompile.expression.Expression;
	using FunctionCall = unluac.decompile.expression.FunctionCall;
	using TableLiteral = unluac.decompile.expression.TableLiteral;
	using TableReference = unluac.decompile.expression.TableReference;
	using Vararg = unluac.decompile.expression.Vararg;
	using CallOperation = unluac.decompile.operation.CallOperation;
	using GlobalSet = unluac.decompile.operation.GlobalSet;
	using Operation = unluac.decompile.operation.Operation;
	using RegisterSet = unluac.decompile.operation.RegisterSet;
	using ReturnOperation = unluac.decompile.operation.ReturnOperation;
	using TableSet = unluac.decompile.operation.TableSet;
	using UpvalueSet = unluac.decompile.operation.UpvalueSet;
	using Assignment = unluac.decompile.statement.Assignment;
	using Statement = unluac.decompile.statement.Statement;
	using GlobalTarget = unluac.decompile.target.GlobalTarget;
	using TableTarget = unluac.decompile.target.TableTarget;
	using Target = unluac.decompile.target.Target;
	using UpvalueTarget = unluac.decompile.target.UpvalueTarget;
	using VariableTarget = unluac.decompile.target.VariableTarget;
	using LBoolean = unluac.parse.LBoolean;
	using LFunction = unluac.parse.LFunction;
	//using Stack<T> = unluac.util.Stack<T>;

	public class Decompiler
	{

	  private readonly int registers;
	  private readonly int length;
	  public readonly Code code;
	  private readonly Upvalues upvalues;
	  public readonly Declaration[] declList;

	  protected internal ULFunction f;
	  protected internal LFunction function;
	  private readonly LFunction[] functions;
	  private readonly int @params;
	  private readonly int vararg;

	  private readonly Op tforTarget;

	  public Decompiler(LFunction function)
	  {
		this.f = new ULFunction(function);
		this.function = function;
		registers = function.maximumStackSize;
		length = function.code.Length;
		code = new Code(function);
		if(function.locals.Length >= function.numParams)
		{
		  declList = new Declaration[function.locals.Length];
		  for(int i = 0; i < declList.Length; i++)
		  {
			declList[i] = new Declaration(function.locals[i]);
		  }
		  }
		else
		{
	  //TODO: debug info missing;
		  declList = new Declaration[function.numParams];
		  for(int i = 0; i < declList.Length; i++)
		  {
			declList[i] = new Declaration("_ARG_" + i + "_", 0, length - 1);
		  }
		}
		upvalues = new Upvalues(function.upvalues);
		functions = function.functions;
		@params = function.numParams;
		vararg = function.vararg;
		tforTarget = function.header.version.getTForTarget();
	  }

	  private Registers r;
	  private Block outer;

	  public virtual void decompile()
	  {
		r = new Registers(registers, length, declList, f);
		findReverseTargets();
		handleBranches(true);
		outer = handleBranches(false);
		processSequence(1, length);
	  }

	  public virtual void print()
	  {
		print(new Output());
	  }

	  public virtual void print(OutputProvider @out)
	  {
		print(new Output(@out.print, @out.println));
	  }

	  public virtual void print(Output @out)
	  {
		handleInitialDeclares(@out);
		outer.print(@out);
	  }

	  private void handleInitialDeclares(Output @out)
	  {
		List<Declaration> initdecls = new List<Declaration>(declList.Length);
		for(int i = @params + (vararg & 1); i < declList.Length; i++)
		{
		  if(declList[i].begin == 0)
		  {
			initdecls.Add(declList[i]);
		  }
		}
		if(initdecls.Count > 0)
		{
		  @out.print("local ");
		  @out.print(initdecls[0].name);
		  for(int i = 1; i < initdecls.Count; i++)
		  {
			@out.print(", ");
			@out.print(initdecls[i].name);
		  }
		  @out.println();
		}
	  }

	  private List<Operation> processLine(int line)
	  {
		List<Operation> operations = new List<Operation>();
		int A = code.A(line);
		int B = code.B(line);
		int C = code.C(line);
		int Bx = code.Bx(line);

		switch(code.op(line))
		{
		  case Op.MOVE:
			operations.Add(new RegisterSet(line, A, r.getExpression(B, line)));
			break;
		  case Op.LOADK:
			operations.Add(new RegisterSet(line, A, f.getConstantExpression(Bx)));
			break;
		  case Op.LOADBOOL:
			operations.Add(new RegisterSet(line, A, new ConstantExpression(new Constant(B != 0 ? LBoolean.LTRUE : LBoolean.LFALSE), -1)));
			break;
		  case Op.LOADNIL:
			  {
			int maximum;
			if(function.header.version.usesOldLoadNilEncoding())
			{
			  maximum = B;
			}
			else
			{
			  maximum = A + B;
			}
			while(A <= maximum)
			{
			  operations.Add(new RegisterSet(line, A, Expression.NIL));
			  A++;
			}
			break;
		  }
//JAVA TO VB & C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
		  case Op.GETUPVAL:
			operations.Add(new RegisterSet(line, A, upvalues.getExpression(B)));
			break;
		  case Op.GETTABUP:
			if(B == 0 && (C & 0x100) != 0)
			{
			  operations.Add(new RegisterSet(line, A, f.getGlobalExpression(C & 0xFF))); //TODO: check
			}
			else
			{
			  operations.Add(new RegisterSet(line, A, new TableReference(upvalues.getExpression(B), r.getKExpression(C, line))));
			}
			break;
		  case Op.GETGLOBAL:
			operations.Add(new RegisterSet(line, A, f.getGlobalExpression(Bx)));
			break;
		  case Op.GETTABLE:
			operations.Add(new RegisterSet(line, A, new TableReference(r.getExpression(B, line), r.getKExpression(C, line))));
			break;
		  case Op.SETUPVAL:
			operations.Add(new UpvalueSet(line, upvalues.getName(B), r.getExpression(A, line)));
			break;
		  case Op.SETTABUP:
			if(A == 0 && (B & 0x100) != 0)
			{
			  operations.Add(new GlobalSet(line, f.getGlobalName(B & 0xFF), r.getKExpression(C, line))); //TODO: check
			}
			else
			{
			  operations.Add(new TableSet(line, upvalues.getExpression(A), r.getKExpression(B, line), r.getKExpression(C, line), true, line));
			}
			break;
		  case Op.SETGLOBAL:
			operations.Add(new GlobalSet(line, f.getGlobalName(Bx), r.getExpression(A, line)));
			break;
		  case Op.SETTABLE:
			operations.Add(new TableSet(line, r.getExpression(A, line), r.getKExpression(B, line), r.getKExpression(C, line), true, line));
			break;
		  case Op.NEWTABLE:
			operations.Add(new RegisterSet(line, A, new TableLiteral(B, C)));
			break;
		  case Op.SELF:
			  {
		// We can later determine is : syntax was used by comparing subexpressions with ==
			Expression common = r.getExpression(B, line);
			operations.Add(new RegisterSet(line, A + 1, common));
			operations.Add(new RegisterSet(line, A, new TableReference(common, r.getKExpression(C, line))));
			break;
		  }
//JAVA TO VB & C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
		  case Op.ADD:
			operations.Add(new RegisterSet(line, A, Expression.makeADD(r.getKExpression(B, line), r.getKExpression(C, line))));
			break;
		  case Op.SUB:
			operations.Add(new RegisterSet(line, A, Expression.makeSUB(r.getKExpression(B, line), r.getKExpression(C, line))));
			break;
		  case Op.MUL:
			operations.Add(new RegisterSet(line, A, Expression.makeMUL(r.getKExpression(B, line), r.getKExpression(C, line))));
			break;
		  case Op.DIV:
			operations.Add(new RegisterSet(line, A, Expression.makeDIV(r.getKExpression(B, line), r.getKExpression(C, line))));
			break;
		  case Op.MOD:
			operations.Add(new RegisterSet(line, A, Expression.makeMOD(r.getKExpression(B, line), r.getKExpression(C, line))));
			break;
		  case Op.POW:
			operations.Add(new RegisterSet(line, A, Expression.makePOW(r.getKExpression(B, line), r.getKExpression(C, line))));
			break;
		  case Op.UNM:
			operations.Add(new RegisterSet(line, A, Expression.makeUNM(r.getExpression(B, line))));
			break;
		  case Op.NOT:
			operations.Add(new RegisterSet(line, A, Expression.makeNOT(r.getExpression(B, line))));
			break;
		  case Op.LEN:
			operations.Add(new RegisterSet(line, A, Expression.makeLEN(r.getExpression(B, line))));
			break;
		  case Op.CONCAT:
			  
			Expression @value1 = r.getExpression(C, line);
		//Remember that CONCAT is right associative.
			while(C-- > B)
			{
			  @value1 = Expression.makeCONCAT(r.getExpression(C, line), @value1);
			}
			operations.Add(new RegisterSet(line, A, @value1));
			break;
		  
//JAVA TO VB & C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
		  case Op.JMP:
		  case Op.EQ:
		  case Op.LT:
		  case Op.LE:
		  case Op.TEST:
		  case Op.TESTSET:
		// Do nothing... handled with branches 
			break;
		  case Op.CALL:
			  {
			bool multiple = (C >= 3 || C == 0);
			if(B == 0)
				B = registers - A;
			if(C == 0)
				C = registers - A + 1;
			Expression function2 = r.getExpression(A, line);
			Expression[] arguments = new Expression[B - 1];
			for(int register = A + 1; register <= A + B - 1; register++)
			{
			  arguments[register - A - 1] = r.getExpression(register, line);
			}
			FunctionCall @value = new FunctionCall(function2, arguments, multiple);
			if(C == 1)
			{
			  operations.Add(new CallOperation(line, @value));
			}
			else
			{
			  if(C == 2 && !multiple)
			  {
				operations.Add(new RegisterSet(line, A, @value));
			  }
			  else
			  {
				for(int register = A; register <= A + C - 2; register++)
				{
				  operations.Add(new RegisterSet(line, register, @value));
				}
			  }
			}
			break;
		  }
//JAVA TO VB & C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
		  case Op.TAILCALL:
			  {
			if(B == 0)
				B = registers - A;
			Expression function1 = r.getExpression(A, line);
			Expression[] arguments = new Expression[B - 1];
			for(int register = A + 1; register <= A + B - 1; register++)
			{
			  arguments[register - A - 1] = r.getExpression(register, line);
			}
			FunctionCall @value2 = new FunctionCall(function1, arguments, true);
			operations.Add(new ReturnOperation(line, @value2));
			skip[line + 1] = true;
			break;
		  }
//JAVA TO VB & C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
		  case Op.RETURN:
			  {
			if(B == 0)
				B = registers - A + 1;
			Expression[] values = new Expression[B - 1];
			for(int register = A; register <= A + B - 2; register++)
			{
			  values[register - A] = r.getExpression(register, line);
			}
			operations.Add(new ReturnOperation(line, values));
			break;
		  }
//JAVA TO VB & C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
		  case Op.FORLOOP:
		  case Op.FORPREP:
		  case Op.TFORCALL:
		  case Op.TFORLOOP:
		// Do nothing... handled with branches 
			break;
		  case Op.SETLIST:
			  
			if(C == 0)
			{
			  C = code.codepoint(line + 1);
			  skip[line + 1] = true;
			}
			if(B == 0)
			{
			  B = registers - A - 1;
			}
			Expression table = r.getValue(A, line);
			for(int i = 1; i <= B; i++)
			{
			  operations.Add(new TableSet(line, table, new ConstantExpression(new Constant((C - 1) * 50 + i), -1), r.getExpression(A + i, line), false, r.getUpdated(A + i, line)));
			}
			break;
		  
//JAVA TO VB & C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
		  case Op.CLOSE:
			break;
		  case Op.CLOSURE:
			  
			LFunction f2 = functions[Bx];
			operations.Add(new RegisterSet(line, A, new ClosureExpression(f2, declList, line + 1)));
			if(function.header.version.usesInlineUpvalueDeclarations())
			{
		  // Skip upvalue declarations
			  for(int i = 0; i < f2.numUpvalues; i++)
			  {
				skip[line + 1 + i] = true;
			  }
			}
			break;
		  
//JAVA TO VB & C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
		  case Op.VARARG:
			  
			bool multiple1 = (B != 2);
			if(B == 1)
				throw new Exception();
			if(B == 0)
				B = registers - A + 1;
			Expression @value3 = new Vararg(B - 1, multiple1);
			for(int register = A; register <= A + B - 2; register++)
			{
			  operations.Add(new RegisterSet(line, register, @value3));
			}
			break;
		  
//JAVA TO VB & C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
		  default:
			throw new Exception("Illegal instruction: " + code.op(line));
		}
		return operations;
	  }

//  *
//   * When lines are processed out of order, they are noted
//   * here so they can be skipped when encountered normally.
//   
	  internal bool[] skip;

//  *
//   * Precalculated array of which lines are the targets of
//   * jump instructions that go backwards... such targets
//   * must be at the statement/block level in the outputted
//   * code (they cannot be mid-expression).
//   
	  internal bool[] reverseTarget;

	  private void findReverseTargets()
	  {
		reverseTarget = new bool[length + 1];
		for(int i = 0; i < reverseTarget.Length; i++)
        {
            reverseTarget[i] = false;
        }
		for(int line = 1; line <= length; line++)
		{
		  if(code.op(line) == Op.JMP && code.sBx(line) < 0)
		  {
			reverseTarget[line + 1 + code.sBx(line)] = true;
		  }
		}
	  }

	  private Assignment processOperation(Operation operation, int line, int nextLine, Block block)
	  {
		Assignment assign = null;
		bool wasMultiple = false;
		Statement stmt = operation.process(r, block);
		if(stmt != null)
		{
		  if(stmt is Assignment)
		  {
			assign = (Assignment) stmt;
			if(!assign.getFirstValue().isMultiple())
			{
			  block.addStatement(stmt);
			}
			else
			{
			  wasMultiple = true;
			}
			}
		  else
		  {
			block.addStatement(stmt);
		  }
	  //System.out.println("-- added statemtent @" + line);
		  if(assign != null)
		  {
		//System.out.println("-- checking for multiassign @" + nextLine);
			while(nextLine < block.end && isMoveIntoTarget(nextLine))
			{
		  //System.out.println("-- found multiassign @" + nextLine);
			  Target target = getMoveIntoTargetTarget(nextLine, line + 1);
			  Expression @value = getMoveIntoTargetValue(nextLine, line + 1); //updated?
			  assign.addFirst(target, @value);
			  skip[nextLine] = true;
			  nextLine++;
			}
			if(wasMultiple && !assign.getFirstValue().isMultiple())
			{
			  block.addStatement(stmt);
			}
		  }
		}
		return assign;
	  }

	  private void processSequence(int begin, int end)
	  {
		int blockIndex = 1;
		Stack<Block> blockStack = new Stack<Block>();
		blockStack.Push(blocks[0]);
		skip = new bool[end + 1];
		for(int line = begin; line <= end; line++)
		{
//      
//      System.out.print("-- line " + line + "; R[0] = ");
//      r.getValue(0, line).print(new Output());
//      System.out.println();
//      System.out.print("-- line " + line + "; R[1] = ");
//      r.getValue(1, line).print(new Output());
//      System.out.println();
//      System.out.print("-- line " + line + "; R[2] = ");
//      r.getValue(2, line).print(new Output());
//      System.out.println();
//      
		  Operation blockHandler = null;
		  while(blockStack.Peek().end <= line)
		  {
			Block block1 = blockStack.Pop();
			blockHandler = block1.process(this);
			if(blockHandler != null)
			{
			  break;
			}
		  }
		  if(blockHandler == null)
		  {
			while(blockIndex < blocks.Count && blocks[blockIndex].begin <= line)
			{
			  blockStack.Push(blocks[blockIndex++]);
			}
		  }
		  Block block = blockStack.Peek();
		  r.startLine(line); //Must occur AFTER block.rewrite
		  if(skip[line])
		  {
			List<Declaration> newLocals1 = r.getNewLocals(line);
			if(newLocals1.Count != 0)
			{
			  Assignment assign1 = new Assignment();
			  assign1.declare(newLocals1[0].begin);
			  foreach(Declaration decl in newLocals1)
			  {
				assign1.addLast(new VariableTarget(decl), r.getValue(decl.register, line));
			  }
			  blockStack.Peek().addStatement(assign1);
			}
			continue;
		  }
		  List<Operation> operations = processLine(line);
		  List<Declaration> newLocals = r.getNewLocals(blockHandler == null ? line : line - 1);
	  //List<Declaration> newLocals = r.getNewLocals(line);
		  Assignment assign = null;
		  if(blockHandler == null)
		  {
			if(code.op(line) == Op.LOADNIL)
			{
			  assign = new Assignment();
			  int count = 0;
			  foreach(Operation operation in operations)
			  {
				RegisterSet @set = (RegisterSet) operation;
				operation.process(r, block);
				if(r.isAssignable(@set.register, @set.line))
				{
				  assign.addLast(r.getTarget(@set.register, @set.line), @set.value);
				  count++;
				}
			  }
			  if(count > 0)
			  {
				block.addStatement(assign);
			  }
			  }
			else
			{
		  //System.out.println("-- Process iterating ... ");
			  foreach(Operation operation in operations)
			  {
			//System.out.println("-- iter");
				Assignment temp = processOperation(operation, line, line + 1, block);
				if(temp != null)
				{
				  assign = temp;
			  //System.out.print("-- top assign -> "); temp.getFirstTarget().print(new Output()); System.out.println();
				}
			  }
			  if(assign != null && assign.getFirstValue().isMultiple())
			  {
				block.addStatement(assign);
			  }
			}
			  }
		  else
		  {
			assign = processOperation(blockHandler, line, line, block);
		  }
		  if(assign != null)
		  {
			if(newLocals.Count != 0)
			{
			  assign.declare(newLocals[0].begin);
			  foreach(Declaration decl in newLocals)
			  {
			//System.out.println("-- adding decl @" + line);
				assign.addLast(new VariableTarget(decl), r.getValue(decl.register, line + 1));
			  }
		  //blockStack.peek().addStatement(assign);
			}
		  }
		  if(blockHandler == null)
		  {
			if(assign != null)
			{

			}
			else if(newLocals.Count != 0 && code.op(line) != Op.FORPREP)
			{
			  if(code.op(line) != Op.JMP || code.op(line + 1 + code.sBx(line)) != tforTarget)
			  {
				assign = new Assignment();
				assign.declare(newLocals[0].begin);
				foreach(Declaration decl in newLocals)
				{
				  assign.addLast(new VariableTarget(decl), r.getValue(decl.register, line));
				}
				blockStack.Peek().addStatement(assign);
			  }
			}
		  }
		  if(blockHandler != null)
		  {
		//System.out.println("-- repeat @" + line);
			line--;
			continue;
		  }
		}
	  }

	  private bool isMoveIntoTarget(int line)
	  {
		switch(code.op(line))
		{
		  case Op.MOVE:
			return r.isAssignable(code.A(line), line) && !r.isLocal(code.B(line), line);
		  case Op.SETUPVAL:
		  case Op.SETGLOBAL:
			return !r.isLocal(code.A(line), line);
		  case Op.SETTABLE:
			  {
			int C = code.C(line);
			if((C & 0x100) != 0)
			{
			  return false;
			}
			else
			{
			  return !r.isLocal(C, line);
			}
		  }
//JAVA TO VB & C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
		  default:
			return false;
		}
	  }

	  private Target getMoveIntoTargetTarget(int line, int previous)
	  {
		switch(code.op(line))
		{
		  case Op.MOVE:
			return r.getTarget(code.A(line), line);
		  case Op.SETUPVAL:
			return new UpvalueTarget(upvalues.getName(code.B(line)));
		  case Op.SETGLOBAL:
			return new GlobalTarget(f.getGlobalName(code.Bx(line)));
		  case Op.SETTABLE:
			return new TableTarget(r.getExpression(code.A(line), previous), r.getKExpression(code.B(line), previous));
		  default:
			throw new Exception();
		}
	  }

	  private Expression getMoveIntoTargetValue(int line, int previous)
	  {
		int A = code.A(line);
		int B = code.B(line);
		int C = code.C(line);
		switch(code.op(line))
		{
		  case Op.MOVE:
			return r.getValue(B, previous);
		  case Op.SETUPVAL:
		  case Op.SETGLOBAL:
			return r.getExpression(A, previous);
		  case Op.SETTABLE:
			if((C & 0x100) != 0)
			{
			  throw new Exception();
			}
			else
			{
			  return r.getExpression(C, previous);
			}
//JAVA TO VB & C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
		  default:
			throw new Exception();
		}
	  }

	  private List<Block> blocks;

	  private OuterBlock handleBranches(bool first)
	  {
		List<Block> oldBlocks = blocks;
		blocks = new List<Block>();
		OuterBlock outer = new OuterBlock(function, length);
		blocks.Add(outer);
		bool[] isBreak = new bool[length + 1];
		bool[] loopRemoved = new bool[length + 1];
		if(!first)
		{
		  foreach(Block block in oldBlocks)
		  {
			if(block is AlwaysLoop)
			{
			  blocks.Add(block);
			}
			if(block is Break)
			{
			  blocks.Add(block);
			  isBreak[block.begin] = true;
			}
		  }
		  List<Block> delete = new List<Block>();
		  foreach(Block block in blocks)
		  {
			if(block is AlwaysLoop)
			{
			  foreach(Block block2 in blocks)
			  {
				if(block != block2)
				{
				  if(block.begin == block2.begin)
				  {
					if(block.end < block2.end)
					{
					  delete.Add(block);
					  loopRemoved[block.end - 1] = true;
					}
					else
					{
					  delete.Add(block2);
					  loopRemoved[block2.end - 1] = true;
					}
				  }
				}
			  }
			}
		  }
		  foreach(Block block in delete)
		  {
			blocks.Remove(block);
		  }
		}
		skip = new bool[length + 1];
		unluac.util.Stack<Branch> stack = new unluac.util.Stack<Branch>();
		bool reduce = false;
		bool testset = false;
		int testsetend = -1;
		for(int line = 1; line <= length; line++)
		{
		  if(!skip[line])
		  {
			switch(code.op(line))
			{
			  case Op.EQ:
				  {
				EQNode node = new EQNode(code.B(line), code.C(line), code.A(line) != 0, line, line + 2, line + 2 + code.sBx(line + 1));
				stack.push(node);
				skip[line + 1] = true;
				if(code.op(node.end) == Op.LOADBOOL)
				{
				  if(code.C(node.end) != 0)
				  {
					node.isCompareSet = true;
					node.setTarget = code.A(node.end);
				  }
				  else if(code.op(node.end - 1) == Op.LOADBOOL)
				  {
					if(code.C(node.end - 1) != 0)
					{
					  node.isCompareSet = true;
					  node.setTarget = code.A(node.end);
					}
				  }
				}
				continue;
			  }
//JAVA TO VB & C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
			  case Op.LT:
				  {
				LTNode node = new LTNode(code.B(line), code.C(line), code.A(line) != 0, line, line + 2, line + 2 + code.sBx(line + 1));
				stack.push(node);
				skip[line + 1] = true;
				if(code.op(node.end) == Op.LOADBOOL)
				{
				  if(code.C(node.end) != 0)
				  {
					node.isCompareSet = true;
					node.setTarget = code.A(node.end);
				  }
				  else if(code.op(node.end - 1) == Op.LOADBOOL)
				  {
					if(code.C(node.end - 1) != 0)
					{
					  node.isCompareSet = true;
					  node.setTarget = code.A(node.end);
					}
				  }
				}
				continue;
			  }
//JAVA TO VB & C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
			  case Op.LE:
				  {
				LENode node = new LENode(code.B(line), code.C(line), code.A(line) != 0, line, line + 2, line + 2 + code.sBx(line + 1));
				stack.push(node);
				skip[line + 1] = true;
				if(code.op(node.end) == Op.LOADBOOL)
				{
				  if(code.C(node.end) != 0)
				  {
					node.isCompareSet = true;
					node.setTarget = code.A(node.end);
				  }
				  else if(code.op(node.end - 1) == Op.LOADBOOL)
				  {
					if(code.C(node.end - 1) != 0)
					{
					  node.isCompareSet = true;
					  node.setTarget = code.A(node.end);
					}
				  }
				}
				continue;
			  }
//JAVA TO VB & C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
			  case Op.TEST:
				stack.push(new TestNode(code.A(line), code.C(line) != 0, line, line + 2, line + 2 + code.sBx(line + 1)));
				skip[line + 1] = true;
				continue;
			  case Op.TESTSET:
				testset = true;
				testsetend = line + 2 + code.sBx(line + 1);
				stack.push(new TestSetNode(code.A(line), code.B(line), code.C(line) != 0, line, line + 2, line + 2 + code.sBx(line + 1)));
				skip[line + 1] = true;
				continue;
			  case Op.JMP:
				  
				reduce = true;
				int tline = line + 1 + code.sBx(line);
				if(tline >= 2 && code.op(tline - 1) == Op.LOADBOOL && code.C(tline - 1) != 0)
				{
				  stack.push(new TrueNode(code.A(tline - 1), false, line, line + 1, tline));
				  skip[line + 1] = true;
				}
				else if(code.op(tline) == tforTarget && !skip[tline])
				{
				  int A = code.A(tline);
				  int C = code.C(tline);
				  if(C == 0)
					  throw new Exception();
				  r.setInternalLoopVariable(A, tline, line + 1); //TODO: end?
				  r.setInternalLoopVariable(A + 1, tline, line + 1);
				  r.setInternalLoopVariable(A + 2, tline, line + 1);
				  for(int index = 1; index <= C; index++)
				  {
					r.setExplicitLoopVariable(A + 2 + index, line, tline + 2); //TODO: end?
				  }
				  skip[tline] = true;
				  skip[tline + 1] = true;
				  blocks.Add(new TForBlock(function, line + 1, tline + 2, A, C, r));
				  }
				else if(code.sBx(line) == 2 && code.op(line + 1) == Op.LOADBOOL && code.C(line + 1) != 0)
				{
			  // This is the tail of a boolean set with a compare node and assign node 
				  blocks.Add(new BooleanIndicator(function, line));
				}
				else
				{
//              
//              for(Block block : blocks) {
//                if(!block.breakable() && block.end == tline) {
//                  block.end = line;
//                }
//              }
//              
				  if(first || loopRemoved[line])
				  {
					if(tline > line)
					{
					  isBreak[line] = true;
					  blocks.Add(new Break(function, line, tline));
					}
					else
					{
					  Block enclosing = enclosingBreakableBlock(line);
					  if(enclosing != null && enclosing.breakable() && code.op(enclosing.end) == Op.JMP && code.sBx(enclosing.end) + enclosing.end + 1 == tline)
					  {
						isBreak[line] = true;
						blocks.Add(new Break(function, line, enclosing.end));
					  }
					  else
					  {
						blocks.Add(new AlwaysLoop(function, tline, line + 1));
					  }

					}
				  }
				}
				break;
			  
//JAVA TO VB & C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
			  case Op.FORPREP:
				reduce = true;
				blocks.Add(new ForBlock(function, line + 1, line + 2 + code.sBx(line), code.A(line), r));
				skip[line + 1 + code.sBx(line)] = true;
				r.setInternalLoopVariable(code.A(line), line, line + 2 + code.sBx(line));
				r.setInternalLoopVariable(code.A(line) + 1, line, line + 2 + code.sBx(line));
				r.setInternalLoopVariable(code.A(line) + 2, line, line + 2 + code.sBx(line));
				r.setExplicitLoopVariable(code.A(line) + 3, line, line + 2 + code.sBx(line));
				break;
			  case Op.FORLOOP:
			// Should be skipped by preceding FORPREP 
				throw new Exception();
			  default:
				reduce = isStatement(line);
				break;
			}
		  }

		  if((line + 1) <= length && reverseTarget[line + 1])
		  {
			reduce = true;
		  }
		  if(testset && testsetend == line + 1)
		  {
			reduce = true;
		  }
		  if(stack.isEmpty())
		  {
			reduce = false;
		  }
		  if(reduce)
		  {
			reduce = false;
            unluac.util.Stack<Branch> conditions = new unluac.util.Stack<Branch>();
            unluac.util.Stack<unluac.util.Stack<Branch>> backups = new unluac.util.Stack<unluac.util.Stack<Branch>>();
			do
			{
			  bool isAssignNode = stack.peek() is TestSetNode;
			  int assignEnd = stack.peek().end;
			  bool compareCorrect = false;
			  if(stack.peek() is TrueNode)
			  {
				isAssignNode = true;
				compareCorrect = true;
			//assignEnd = stack.peek().begin;
				if(code.C(assignEnd) != 0)
				{
				  assignEnd += 2;
				}
				else
				{
				  assignEnd += 1;
				}
			//System.exit(0);
				}
			  else if(stack.peek().isCompareSet)
			  {
			//System.err.println("c" + stack.peek().setTarget); 
				if(code.op(stack.peek().begin) != Op.LOADBOOL || code.C(stack.peek().begin) == 0)
				{
				  isAssignNode = true;
				  if(code.C(assignEnd) != 0)
				  {
					assignEnd += 2;
				  }
				  else
				  {
					assignEnd += 1;
				  }
				  compareCorrect = true;
				}
				  }
			  else if(assignEnd - 3 >= 1 && code.op(assignEnd - 2) == Op.LOADBOOL && code.C(assignEnd - 2) != 0 && code.op(assignEnd - 3) == Op.JMP && code.sBx(assignEnd - 3) == 2)
			  {
				if(stack.peek() is TestNode)
				{
				  TestNode node = (TestNode) stack.peek();
				  if(node.test == code.A(assignEnd - 2))
				  {
					isAssignNode = true;
				  }
				}
				  }
			  else if(assignEnd - 2 >= 1 && code.op(assignEnd - 1) == Op.LOADBOOL && code.C(assignEnd - 1) != 0 && code.op(assignEnd - 2) == Op.JMP && code.sBx(assignEnd - 2) == 2)
			  {
				if(stack.peek() is TestNode)
				{
				  isAssignNode = true;
				  assignEnd += 1;
				}
				}
			  else if(assignEnd - 1 >= 1 && r.isLocal(getAssignment(assignEnd - 1), assignEnd - 1) && assignEnd > stack.peek().line)
			  {
				Declaration decl = r.getDeclaration(getAssignment(assignEnd - 1), assignEnd - 1);
				if(decl.begin == assignEnd - 1 && decl.end > assignEnd - 1)
				{
				  isAssignNode = true;
				}
			  }
			  if(!compareCorrect && assignEnd - 1 == stack.peek().begin && code.op(stack.peek().begin) == Op.LOADBOOL && code.C(stack.peek().begin) != 0)
			  {
				backup = null;
				int begin = stack.peek().begin;
				assignEnd = begin + 2;
				int target = code.A(begin);
				conditions.push(popCompareSetCondition(stack, assignEnd));
				conditions.peek().setTarget = target;
				conditions.peek().end = assignEnd;
				conditions.peek().begin = begin;
			  }
			  else if(isAssignNode)
			  {
				backup = null;
				int target = stack.peek().setTarget;
				int begin = stack.peek().begin;
				conditions.push(popSetCondition(stack, assignEnd));
				conditions.peek().setTarget = target;
				conditions.peek().end = assignEnd;
				conditions.peek().begin = begin;
			  }
			  else
			  {
                  backup = new unluac.util.Stack<Branch>();
				conditions.push(popCondition(stack));
				backup.reverse();
			  }
			  backups.push(backup);
			} while(!stack.isEmpty());
			do
			{
			  Branch cond = conditions.pop();
              unluac.util.Stack<Branch> backup1 = backups.pop();
			  int breakTarget = this.breakTarget(cond.begin);
			  bool breakable = (breakTarget >= 1);
			  if(breakable && code.op(breakTarget) == Op.JMP)
			  {
				breakTarget += 1 + code.sBx(breakTarget);
			  }
			  if(breakable && breakTarget == cond.end)
			  {
				Block immediateEnclosing = enclosingBlock(cond.begin);
				for(int iline = Math.Max(cond.end, immediateEnclosing.end - 1); iline >= Math.Max(cond.begin, immediateEnclosing.begin); iline--)
				{
				  if(code.op(iline) == Op.JMP && iline + 1 + code.sBx(iline) == breakTarget)
				  {
					cond.end = iline;
					break;
				  }
				}
			  }
		  // A branch has a tail if the instruction just before the end target is JMP 
			  bool hasTail = cond.end >= 2 && code.op(cond.end - 1) == Op.JMP;
		  // This is the target of the tail JMP 
			  int tail = hasTail ? cond.end + code.sBx(cond.end - 1) : -1;
			  int originalTail = tail;
			  Block enclosing = enclosingUnprotectedBlock(cond.begin);
		  // Checking enclosing unprotected block to undo JMP redirects. 
			  if(enclosing != null)
			  {
			//System.err.println("loopback: " + enclosing.getLoopback());
			//System.err.println("cond.end: " + cond.end);
			//System.err.println("tail    : " + tail);
				if(enclosing.getLoopback() == cond.end)
				{
				  cond.end = enclosing.end - 1;
				  hasTail = cond.end >= 2 && code.op(cond.end - 1) == Op.JMP;
				  tail = hasTail ? cond.end + code.sBx(cond.end - 1) : -1;
				}
				if(hasTail && enclosing.getLoopback() == tail)
				{
				  tail = enclosing.end - 1;
				}
			  }
			  if(cond.isSet)
			  {
				bool empty = cond.begin == cond.end;
				if(code.op(cond.begin) == Op.JMP && code.sBx(cond.begin) == 2 && code.op(cond.begin + 1) == Op.LOADBOOL && code.C(cond.begin + 1) != 0)
				{
				  empty = true;
				}
				blocks.Add(new SetBlock(function, cond, cond.setTarget, line, cond.begin, cond.end, empty, r));
				}
			  else if(code.op(cond.begin) == Op.LOADBOOL && code.C(cond.begin) != 0)
			  {
				int begin = cond.begin;
				int target = code.A(begin);
				if(code.B(begin) == 0)
				{
				  cond = cond.invert();
				}
				blocks.Add(new CompareBlock(function, begin, begin + 2, target, cond));
				}
			  else if(cond.end < cond.begin)
			  {
				blocks.Add(new RepeatBlock(function, cond, r));
			  }
			  else if(hasTail)
			  {
				Op endOp = code.op(cond.end - 2);
				bool isEndCondJump = endOp == Op.EQ || endOp == Op.LE || endOp == Op.LT || endOp == Op.TEST || endOp == Op.TESTSET;
				if(tail > cond.end || (tail == cond.end && !isEndCondJump))
				{
				  Op op = code.op(tail - 1);
				  int sbx = code.sBx(tail - 1);
				  int loopback2 = tail + sbx;
				  bool isBreakableLoopEnd = function.header.version.isBreakableLoopEnd(op);
				  if(isBreakableLoopEnd && loopback2 <= cond.begin && !isBreak[tail - 1])
				  {
				// (ends with break) 
					blocks.Add(new IfThenEndBlock(function, cond, backup1, r));
				  }
				  else
				  {
					skip[cond.end - 1] = true; //Skip the JMP over the else block
					bool emptyElse = tail == cond.end;
					IfThenElseBlock ifthen = new IfThenElseBlock(function, cond, originalTail, emptyElse, r);
					blocks.Add(ifthen);

					if(!emptyElse)
					{
					  ElseEndBlock elseend = new ElseEndBlock(function, cond.end, tail);
					  blocks.Add(elseend);
					}
				  }
					}
				else
				{
				  int loopback = tail;
				  bool existsStatement = false;
				  for(int sl = loopback; sl < cond.begin; sl++)
				  {
					if(!skip[sl] && isStatement(sl))
					{
					  existsStatement = true;
					  break;
					}
				  }
			  //TODO: check for 5.2-style if cond then break end
				  if(loopback >= cond.begin || existsStatement)
				  {
					blocks.Add(new IfThenEndBlock(function, cond, backup1, r));
				  }
				  else
				  {
					skip[cond.end - 1] = true;
					blocks.Add(new WhileBlock(function, cond, originalTail, r));
				  }
				}
				  }
			  else
			  {
				blocks.Add(new IfThenEndBlock(function, cond, backup1, r));
			  }
			} while(!conditions.isEmpty());
		  }
		}
	//Find variables whose scope isn't controlled by existing blocks:
		foreach(Declaration decl in declList)
		{
		  if(!decl.forLoop && !decl.forLoopExplicit)
		  {
			bool needsDoEnd = true;
			foreach(Block block in blocks)
			{
			  if(block.contains(decl.begin))
			  {
				if(block.scopeEnd() == decl.end)
				{
				  needsDoEnd = false;
				  break;
				}
			  }
			}
			if(needsDoEnd)
			{
		  //Without accounting for the order of declarations, we might
		  //create another do..end block later that would eliminate the
		  //need for this one. But order of decls should fix this.
			  blocks.Add(new DoEndBlock(function, decl.begin, decl.end + 1));
			}
		  }
		}
	// Remove breaks that were later parsed as else jumps
		List<Block>.Enumerator iter = blocks.GetEnumerator();
		while(iter.MoveNext())
		{
		  Block block = iter.Current;
		  if(skip[block.begin] && block is Break)
		  {
			blocks.Remove(block);
		  }
		}
        blocks.Sort();
		//Collections.sort(blocks);
		backup = null;
		return outer;
	  }

	  private int breakTarget(int line)
	  {
		int tline = int.MaxValue;
		foreach(Block block in blocks)
		{
		  if(block.breakable() && block.contains(line))
		  {
			tline = Math.Min(tline, block.end);
		  }
		}
		if(tline == int.MaxValue)
			return -1;
		return tline;
	  }

	  private Block enclosingBlock(int line)
	  {
	//Assumes the outer block is first
		Block outer = blocks[0];
		Block enclosing = outer;
		for(int i = 1; i < blocks.Count; i++)
		{
		  Block next = blocks[i];
		  if(next.isContainer() && enclosing.contains(next) && next.contains(line) && !next.loopRedirectAdjustment)
		  {
			enclosing = next;
		  }
		}
		return enclosing;
	  }

	  private Block enclosingBlock(Block block)
	  {
	//Assumes the outer block is first
		Block outer = blocks[0];
		Block enclosing = outer;
		for(int i = 1; i < blocks.Count; i++)
		{
		  Block next = blocks[i];
		  if(next == block)
			  continue;
		  if(next.contains(block) && enclosing.contains(next))
		  {
			enclosing = next;
		  }
		}
		return enclosing;
	  }

	  private Block enclosingBreakableBlock(int line)
	  {
		Block outer = blocks[0];
		Block enclosing = outer;
		for(int i = 1; i < blocks.Count; i++)
		{
		  Block next = blocks[i];
		  if(enclosing.contains(next) && next.contains(line) && next.breakable() && !next.loopRedirectAdjustment)
		  {
			enclosing = next;
		  }
		}
		return enclosing == outer ? null : enclosing;
	  }

	  private Block enclosingUnprotectedBlock(int line)
	  {
	//Assumes the outer block is first
		Block outer = blocks[0];
		Block enclosing = outer;
		for(int i = 1; i < blocks.Count; i++)
		{
		  Block next = blocks[i];
		  if(enclosing.contains(next) && next.contains(line) && next.isUnprotected() && !next.loopRedirectAdjustment)
		  {
			enclosing = next;
		  }
		}
		return enclosing == outer ? null : enclosing;
	  }

      private static unluac.util.Stack<Branch> backup;

      public virtual Branch popCondition(unluac.util.Stack<Branch> stack)
	  {
		Branch branch = stack.pop();
		if(backup != null)
			backup.push(branch);
		if(branch is TestSetNode)
		{
		  throw new Exception();
		}
		int begin = branch.begin;
		if(code.op(branch.begin) == Op.JMP)
		{
		  begin += 1 + code.sBx(branch.begin);
		}
		while(stack.Count > 0)
		{
		  Branch next = stack.peek();
		  if(next is TestSetNode)
			  break;
		  if(next.end == begin)
		  {
			branch = new OrBranch(popCondition(stack).invert(), branch);
		  }
		  else if(next.end == branch.end)
		  {
			branch = new AndBranch(popCondition(stack), branch);
		  }
		  else
		  {
			break;
		  }
		}
		return branch;
	  }

      public virtual Branch popSetCondition(unluac.util.Stack<Branch> stack, int assignEnd)
	  {
	//System.err.println("assign end " + assignEnd);
		stack.push(new AssignNode(assignEnd - 1, assignEnd, assignEnd));
	//Invert argument doesn't matter because begin == end
		Branch rtn = _helper_popSetCondition(stack, false, assignEnd);
		return rtn;
	  }

      public virtual Branch popCompareSetCondition(unluac.util.Stack<Branch> stack, int assignEnd)
	  {
		Branch top = stack.pop();
		bool invert = false;
		if(code.B(top.begin) == 0) //top = top.invert();
			invert = true;
		top.begin = assignEnd;
		top.end = assignEnd;
		stack.push(top);
	//stack.pop();
	//stack.push(new AssignNode(assignEnd - 1, assignEnd, assignEnd));
	//Invert argument doesn't matter because begin == end
		Branch rtn = _helper_popSetCondition(stack, invert, assignEnd);
		return rtn;
	  }

      private Branch _helper_popSetCondition(unluac.util.Stack<Branch> stack, bool invert, int assignEnd)
	  {
		Branch branch = stack.pop();
		int begin = branch.begin;
		int end = branch.end;
	//System.err.println(stack.size());
	//System.err.println("_helper_popSetCondition; count: " + count);
	//System.err.println("_helper_popSetCondition; begin: " + begin);
	//System.err.println("_helper_popSetCondition; end:   " + end);
		if(invert)
		{
		  branch = branch.invert();
		}
		if(code.op(begin) == Op.LOADBOOL)
		{
		  if(code.C(begin) != 0)
		  {
			begin += 2;
		  }
		  else
		  {
			begin += 1;
		  }
		}
		if(code.op(end) == Op.LOADBOOL)
		{
		  if(code.C(end) != 0)
		  {
			end += 2;
		  }
		  else
		  {
			end += 1;
		  }
		}
	//System.err.println("_helper_popSetCondition; begin_adj: " + begin);
	//System.err.println("_helper_popSetCondition; end_adj:   " + end);
	//if(count >= 2) System.exit(1);
		int target = branch.setTarget;
		while(!stack.isEmpty())
		{
		  Branch next = stack.peek();
	  //System.err.println("_helper_popSetCondition; next begin: " + next.begin);
	  //System.err.println("_helper_popSetCondition; next end:   " + next.end);
		  bool ninvert;
		  int nend = next.end;
		  if(code.op(next.end) == Op.LOADBOOL)
		  {
			ninvert = code.B(next.end) != 0;
			if(code.C(next.end) != 0)
			{
			  nend += 2;
			}
			else
			{
			  nend += 1;
			}
			}
		  else if(next is TestSetNode)
		  {
			TestSetNode node = (TestSetNode) next;
			ninvert = node._invert;
		  }
		  else if(next is TestNode)
		  {
			TestNode node = (TestNode) next;
			ninvert = node._invert;
		  }
		  else
		  {
			ninvert = false;
			if(nend >= assignEnd)
			{
		  //System.err.println("break");
			  break;
			}
		  }
		  int addr;
		  if(ninvert == invert)
		  {
			addr = end;
		  }
		  else
		  {
			addr = begin;
		  }

	  //System.err.println(" addr: " + addr + "(" + begin + ", " + end + ")");
	  //System.err.println(" nend: " + nend);
	  //System.err.println(" ninv: " + ninvert);
	  //System.err.println("-------------");
	  //System.exit(0);

		  if(addr == nend)
		  {
			if(addr != nend)
				ninvert = !ninvert;
			if(ninvert)
			{
			  branch = new OrBranch(_helper_popSetCondition(stack, ninvert, assignEnd), branch);
			}
			else
			{
			  branch = new AndBranch(_helper_popSetCondition(stack, ninvert, assignEnd), branch);
			}
			branch.end = nend;
			}
		  else
		  {
			if(!(branch is TestSetNode))
			{
			  stack.push(branch);
			  branch = popCondition(stack);
			}
		//System.out.println("--break");
			break;
		  }
		}
		branch.isSet = true;
		branch.setTarget = target;
		return branch;
	  }

	  private bool isStatement(int line)
	  {
		return isStatement(line, -1);
	  }

	  private bool isStatement(int line, int testRegister)
	  {
		switch(code.op(line))
		{
		  case Op.MOVE:
          case Op.LOADK:
          case Op.LOADBOOL:
          case Op.GETUPVAL:
          case Op.GETTABUP:
          case Op.GETGLOBAL:
          case Op.GETTABLE:
          case Op.NEWTABLE:
          case Op.ADD:
          case Op.SUB:
          case Op.MUL:
          case Op.DIV:
          case Op.MOD:
          case Op.POW:
          case Op.UNM:
          case Op.NOT:
          case Op.LEN:
          case Op.CONCAT:
          case Op.CLOSURE:
			return r.isLocal(code.A(line), line) || code.A(line) == testRegister;
          case Op.LOADNIL:
			for(int register = code.A(line); register <= code.B(line); register++)
			{
			  if(r.isLocal(register, line))
			  {
				return true;
			  }
			}
			return false;
          case Op.SETGLOBAL:
          case Op.SETUPVAL:
          case Op.SETTABUP:
          case Op.SETTABLE:
          case Op.JMP:
          case Op.TAILCALL:
          case Op.RETURN:
          case Op.FORLOOP:
          case Op.FORPREP:
          case Op.TFORCALL:
          case Op.TFORLOOP:
          case Op.CLOSE:
			return true;
          case Op.SELF:
			return r.isLocal(code.A(line), line) || r.isLocal(code.A(line) + 1, line);
          case Op.EQ:
          case Op.LT:
          case Op.LE:
          case Op.TEST:
          case Op.TESTSET:
          case Op.SETLIST:
			return false;
          case Op.CALL:
			  {
			int a = code.A(line);
			int c = code.C(line);
			if(c == 1)
			{
			  return true;
			}
			if(c == 0)
				c = registers - a + 1;
			for(int register = a; register < a + c - 1; register++)
			{
			  if(r.isLocal(register, line))
			  {
				return true;
			  }
			}
			return (c == 2 && a == testRegister);
		  }
//JAVA TO VB & C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
          case Op.VARARG:
			  {
			int a = code.A(line);
			int b = code.B(line);
			if(b == 0)
				b = registers - a + 1;
			for(int register = a; register < a + b - 1; register++)
			{
			  if(r.isLocal(register, line))
			  {
				return true;
			  }
			}
			return false;
		  }
//JAVA TO VB & C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
		  default:
			throw new Exception("Illegal opcode: " + code.op(line));
		}
	  }

//  *
//   * Returns the single register assigned to at the line or
//   * -1 if no register or multiple registers is/are assigned to.
//   
	  private int getAssignment(int line)
	  {
		switch(code.op(line))
		{
            case Op.MOVE:
            case Op.LOADK:
          case Op.LOADBOOL:
          case Op.GETUPVAL:
          case Op.GETTABUP:
          case Op.GETGLOBAL:
          case Op.GETTABLE:
          case Op.NEWTABLE:
          case Op.ADD:
          case Op.SUB:
          case Op.MUL:
          case Op.DIV:
          case Op.MOD:
          case Op.POW:
          case Op.UNM:
          case Op.NOT:
          case Op.LEN:
          case Op.CONCAT:
          case Op.CLOSURE:
			return code.A(line);
          case Op.LOADNIL:
			if(code.A(line) == code.B(line))
			{
			  return code.A(line);
			}
			else
			{
			  return -1;
			}
//JAVA TO VB & C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
          case Op.SETGLOBAL:
          case Op.SETUPVAL:
          case Op.SETTABUP:
          case Op.SETTABLE:
          case Op.JMP:
          case Op.TAILCALL:
          case Op.RETURN:
          case Op.FORLOOP:
          case Op.FORPREP:
          case Op.TFORCALL:
          case Op.TFORLOOP:
          case Op.CLOSE:
			return -1;
          case Op.SELF:
			return -1;
          case Op.EQ:
          case Op.LT:
          case Op.LE:
          case Op.TEST:
          case Op.TESTSET:
          case Op.SETLIST:
			return -1;
          case Op.CALL:
			  {
			if(code.C(line) == 2)
			{
			  return code.A(line);
			}
			else
			{
			  return -1;
			}
		  }
//JAVA TO VB & C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
          case Op.VARARG:
			  {
			if(code.C(line) == 2)
			{
			  return code.B(line);
			}
			else
			{
			  return -1;
			}
		  }
//JAVA TO VB & C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
		  default:
			throw new Exception("Illegal opcode: " + code.op(line));
		}
	  }

	}

}
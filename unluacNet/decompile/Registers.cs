using System.Collections.Generic;
using System;

namespace unluac.decompile
{


	using ConstantExpression = unluac.decompile.expression.ConstantExpression;
	using Expression = unluac.decompile.expression.Expression;
	using LocalVariable = unluac.decompile.expression.LocalVariable;
	using Target = unluac.decompile.target.Target;
	using VariableTarget = unluac.decompile.target.VariableTarget;

	public class Registers
	{

	  public readonly int registers;
	  public readonly int length;

	  private readonly Declaration[,] decls;
	  private readonly ULFunction f;
	  private readonly Expression[,] values;
	  private readonly int[,] updated;

	  public Registers(int registers, int length, Declaration[] declList, ULFunction f)
	  {
		this.registers = registers;
		this.length = length;
//JAVA TO VB & C# CONVERTER TODO TASK: Java rectangular arrays cannot be reliably converted:
		decls = new Declaration[registers,length + 1];
		for(int i = 0; i < declList.Length; i++)
		{
		  Declaration decl = declList[i];
		  int register = 0;
		  while(decls[register,decl.begin] != null)
		  {
			register++;
		  }
		  decl.register = register;
		  for(int line = decl.begin; line <= decl.end; line++)
		  {
			decls[register,line] = decl;
		  }
		}
//JAVA TO VB & C# CONVERTER TODO TASK: Java rectangular arrays cannot be reliably converted:
		values = new Expression[registers,length + 1];
		for(int register = 0; register < registers; register++)
		{
		  values[register,0] = Expression.NIL;
		}
//JAVA TO VB & C# CONVERTER TODO TASK: Java rectangular arrays cannot be reliably converted:
		updated = new int[registers,length + 1];
		startedLines = new bool[length + 1];
          for(int i = 0; i < startedLines.Length; i++)
          {
              startedLines[i] = false;
          }
		this.f = f;
	  }

	  public virtual bool isAssignable(int register, int line)
	  {
		return isLocal(register, line) && !decls[register,line].forLoop;
	  }

	  public virtual bool isLocal(int register, int line)
	  {
		if(register < 0)
			return false;
		return decls[register,line] != null;
	  }

	  public virtual bool isNewLocal(int register, int line)
	  {
		Declaration decl = decls[register,line];
		return decl != null && decl.begin == line && !decl.forLoop;
	  }

	  public virtual List<Declaration> getNewLocals(int line)
	  {
		List<Declaration> locals = new List<Declaration>(registers);
		for(int register = 0; register < registers; register++)
		{
		  if(isNewLocal(register, line))
		  {
			locals.Add(getDeclaration(register, line));
		  }
		}
		return locals;
	  }

	  public virtual Declaration getDeclaration(int register, int line)
	  {
		return decls[register,line];
	  }

	  private bool[] startedLines;

	  public virtual void startLine(int line)
	  {
	//if(startedLines[line]) return;
		startedLines[line] = true;
		for(int register = 0; register < registers; register++)
		{
		  values[register,line] = values[register,line - 1];
		  updated[register,line] = updated[register,line - 1];
		}
	  }

	  public virtual Expression getExpression(int register, int line)
	  {
		if(isLocal(register, line - 1))
		{
		  return new LocalVariable(getDeclaration(register, line - 1));
		}
		else
		{
		  return values[register,line - 1];
		}
	  }

	  public virtual Expression getKExpression(int register, int line)
	  {
		if((register & 0x100) != 0)
		{
		  return f.getConstantExpression(register & 0xFF);
		}
		else
		{
		  return getExpression(register, line);
		}
	  }

	  public virtual Expression getValue(int register, int line)
	  {
		return values[register,line - 1];
	  }

	  public virtual int getUpdated(int register, int line)
	  {
		return updated[register,line];
	  }

	  public virtual void setValue(int register, int line, Expression expression)
	  {
		values[register,line] = expression;
		updated[register,line] = line;
	  }

	  public virtual Target getTarget(int register, int line)
	  {
		if(!isLocal(register, line))
		{
		  throw new Exception("No declaration exists in register " + register + " at line " + line);
		}
		return new VariableTarget(decls[register,line]);
	  }

	  public virtual void setInternalLoopVariable(int register, int begin, int end)
	  {
		Declaration decl = getDeclaration(register, begin);
		if(decl == null)
		{
		  decl = new Declaration("_FOR_", begin, end);
		  decl.register = register;
		  newDeclaration(decl, register, begin, end);
		}
		decl.forLoop = true;
	  }

	  public virtual void setExplicitLoopVariable(int register, int begin, int end)
	  {
		Declaration decl = getDeclaration(register, begin);
		if(decl == null)
		{
		  decl = new Declaration("_FORV_" + register + "_", begin, end);
		  decl.register = register;
		  newDeclaration(decl, register, begin, end);
		}
		decl.forLoopExplicit = true;
	  }

	  private void newDeclaration(Declaration decl, int register, int begin, int end)
	  {
		for(int line = begin; line <= end; line++)
		{
		  decls[register,line] = decl;
		}
	  }

	}

}
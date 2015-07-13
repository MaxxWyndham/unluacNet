namespace unluac.decompile.expression
{

	using Declaration = unluac.decompile.Declaration;
	using Decompiler = unluac.decompile.Decompiler;
	using Output = unluac.decompile.Output;
	using TableTarget = unluac.decompile.target.TableTarget;
	using Target = unluac.decompile.target.Target;
	using VariableTarget = unluac.decompile.target.VariableTarget;
	using LFunction = unluac.parse.LFunction;
	using LUpvalue = unluac.parse.LUpvalue;

	public class ClosureExpression : Expression
	{

	  private readonly LFunction function;
	  private int upvalueLine;
	  private Declaration[] declList;

	  public ClosureExpression(LFunction function, Declaration[] declList, int upvalueLine) : base(PRECEDENCE_ATOMIC)
	  {
		this.function = function;
		this.upvalueLine = upvalueLine;
		this.declList = declList;
	  }

	  public override int getConstantIndex()
	  {
		return -1;
	  }

	  public override bool isClosure()
	  {
		return true;
	  }

	  public override bool isUpvalueOf(int register)
	  {
//    
//    if(function.header.version == 0x51) {
//      return false; //TODO:
//    }
//    
		for(int i = 0; i < function.upvalues.Length; i++)
		{
		  LUpvalue upvalue = function.upvalues[i];
		  if(upvalue.instack && upvalue.idx == register)
		  {
			return true;
		  }
		}
		return false;
	  }

	  public override int closureUpvalueLine()
	  {
		return upvalueLine;
	  }

	  public override void print(Output @out)
	  {
		Decompiler d = new Decompiler(function);
		@out.print("function");
		printMain(@out, d, true);
	  }

	  public override void printClosure(Output @out, Target name)
	  {
		Decompiler d = new Decompiler(function);
		@out.print("function ");
		if(function.numParams >= 1 && d.declList[0].name.Equals("self") && name is TableTarget)
		{
		  name.printMethod(@out);
		  printMain(@out, d, false);
		}
		else
		{
		  name.print(@out);
		  printMain(@out, d, true);
		}
	  }

	  private void printMain(Output @out, Decompiler d, bool includeFirst)
	  {
		@out.print("(");
		int Start = includeFirst ? 0 : 1;
		if(function.numParams > Start)
		{
		  new VariableTarget(d.declList[Start]).print(@out);
		  for(int i = Start + 1; i < function.numParams; i++)
		  {
			@out.print(", ");
			new VariableTarget(d.declList[i]).print(@out);
		  }
		}
		if((function.vararg & 1) == 1)
		{
		  if(function.numParams > Start)
		  {
			@out.print(", ...");
		  }
		  else
		  {
			@out.print("...");
		  }
		}
		@out.print(")");
		@out.println();
		@out.indent();
		d.decompile();
		d.print(@out);
		@out.dedent();
		@out.print("end");
	//out.println(); //This is an extra space for formatting
	  }

	}

}
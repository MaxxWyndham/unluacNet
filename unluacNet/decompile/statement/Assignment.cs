using System.Collections.Generic;

namespace unluac.decompile.statement
{


	using Declaration = unluac.decompile.Declaration;
	using Output = unluac.decompile.Output;
	using Expression = unluac.decompile.expression.Expression;
	using Target = unluac.decompile.target.Target;

	public class Assignment : Statement
	{

	  private readonly List<Target> targets = new List<Target>(5);
	  private readonly List<Expression> values = new List<Expression>(5);

	  private bool allnil = true;
	  private bool _declare = false;
	  private int declareStart = 0;

	  public Assignment()
	  {

	  }

	  public virtual Target getFirstTarget()
	  {
		return targets[0];
	  }

	  public virtual Expression getFirstValue()
	  {
		return values[0];
	  }

	  public virtual bool assignsTarget(Declaration decl)
	  {
		foreach(Target target in targets)
		{
		  if(target.isDeclaration(decl))
		  {
			return true;
		  }
		}
		return false;
	  }

	  public virtual int getArity()
	  {
		return targets.Count;
	  }

	  public Assignment(Target target, Expression @value)
	  {
		targets.Add(target);
		values.Add(@value);
		allnil = allnil && @value.isNil();
	  }

	  public virtual void addFirst(Target target, Expression @value)
	  {
		targets.Insert(0, target);
		values.Insert(0, @value);
		allnil = allnil && @value.isNil();
	  }

	  public virtual void addLast(Target target, Expression @value)
	  {
		if(targets.Contains(target))
		{
		  int index = targets.IndexOf(target);
		  targets.RemoveAt(index);
		  @value = values[index];
          values.RemoveAt(index);
		}
		targets.Add(target);
		values.Add(@value);
		allnil = allnil && @value.isNil();
	  }

	  public virtual bool assignListEquals(List<Declaration> decls)
	  {
		if(decls.Count != targets.Count)
			return false;
		foreach(Target target in targets)
		{
		  bool found = false;
		  foreach(Declaration decl in decls)
		  {
			if(target.isDeclaration(decl))
			{
			  found = true;
			  break;
			}
		  }
		  if(!found)
			  return false;
		}
		return true;
	  }

	  public virtual void declare(int declareStart)
	  {
		_declare = true;
		this.declareStart = declareStart;
	  }

	  public override void print(Output @out)
	  {
		if(targets.Count != 0)
		{
		  if(_declare)
		  {
			@out.print("local ");
		  }
		  bool functionSugar = false;
		  if(targets.Count == 1 && values.Count == 1 && values[0].isClosure() && targets[0].isFunctionName())
		  {
			Expression closure = values[0];
		//comment = "" + declareStart + " >= " + closure.closureUpvalueLine();
		//System.out.println("" + declareStart + " >= " + closure.closureUpvalueLine());
		// This check only works in Lua version 0x51
			if(!_declare || declareStart >= closure.closureUpvalueLine())
			{
			  functionSugar = true;
			}
			if(targets[0].isLocal() && closure.isUpvalueOf(targets[0].getIndex()))
			{
			  functionSugar = true;
			}
		//if(closure.isUpvalueOf(targets.get(0).))
		  }
		  if(!functionSugar)
		  {
			targets[0].print(@out);
			for(int i = 1; i < targets.Count; i++)
			{
			  @out.print(", ");
			  targets[i].print(@out);
			}
			if(!_declare || !allnil)
			{
			  @out.print(" = ");
			  Expression.printSequence(@out, values, false, false);
			}
			}
		  else
		  {
			values[0].printClosure(@out, targets[0]);
		  }
		  if(comment != null)
		  {
			@out.print(" -- ");
			@out.print(comment);
		  }
		}
	  }

	}

}
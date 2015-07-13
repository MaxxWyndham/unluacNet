using System;

namespace unluac.decompile.target
{

	using Declaration = unluac.decompile.Declaration;
	using Output = unluac.decompile.Output;

	public class VariableTarget : Target
	{

	  public readonly Declaration decl;

	  public VariableTarget(Declaration decl)
	  {
		this.decl = decl;
	  }

	  public override void print(Output @out)
	  {
		@out.print(decl.name);
	  }

	  public override void printMethod(Output @out)
	  {
		throw new Exception();
	  }

	  public override bool isDeclaration(Declaration decl)
	  {
		return this.decl == decl;
	  }

	  public override bool isLocal()
	  {
		return true;
	  }

	  public override int getIndex()
	  {
		return decl.register;
	  }

	  public override bool Equals(object obj)
	  {
		if(obj is VariableTarget)
		{
		  VariableTarget t = (VariableTarget) obj;
		  return decl == t.decl;
		}
		else
		{
		  return false;
		}
	  }

	}

}
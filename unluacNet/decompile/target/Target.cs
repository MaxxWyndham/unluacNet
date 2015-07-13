using System;
namespace unluac.decompile.target
{

	using Declaration = unluac.decompile.Declaration;
	using Output = unluac.decompile.Output;

	abstract public class Target
	{

	  abstract public void print(Output @out);

	  abstract public void printMethod(Output @out);

	  public virtual bool isDeclaration(Declaration decl)
	  {
		return false;
	  }

	  public virtual bool isLocal()
	  {
		return false;
	  }

	  public virtual int getIndex()
	  {
          throw new Exception();
	  }

	  public virtual bool isFunctionName()
	  {
		return true;
	  }

	}

}
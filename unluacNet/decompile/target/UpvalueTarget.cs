using System;
namespace unluac.decompile.target
{

	using Output = unluac.decompile.Output;

	public class UpvalueTarget : Target
	{

	  private readonly string name;

	  public UpvalueTarget(string name)
	  {
		this.name = name;
	  }

	  public override void print(Output @out)
	  {
		@out.print(name);
	  }

	  public override void printMethod(Output @out)
	  {
          throw new Exception();
	  }

	}

}
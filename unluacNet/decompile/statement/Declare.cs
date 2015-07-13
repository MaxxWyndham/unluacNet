using System.Collections.Generic;

namespace unluac.decompile.statement
{


	using Declaration = unluac.decompile.Declaration;
	using Output = unluac.decompile.Output;

	public class Declare : Statement
	{

	  private readonly List<Declaration> decls;

	  public Declare(List<Declaration> decls)
	  {
		this.decls = decls;
	  }

	  public override void print(Output @out)
	  {
		@out.print("local ");
		@out.print(decls[0].name);
		for(int i = 1; i < decls.Count; i++)
		{
		  @out.print(", ");
		  @out.print(decls[i].name);
		}
	  }

	}

}
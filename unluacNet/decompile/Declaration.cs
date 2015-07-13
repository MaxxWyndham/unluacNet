namespace unluac.decompile
{

	using LLocal = unluac.parse.LLocal;

	public class Declaration
	{

	  public readonly string name;
	  public readonly int begin;
	  public readonly int end;
	  public int register;

//  *
//   * Whether this is an invisible for-loop book-keeping variable.
//   
	  public bool forLoop = false;

//  *
//   * Whether this is an explicit for-loop declared variable.
//   
	  public bool forLoopExplicit = false;

	  public Declaration(LLocal local)
	  {
		this.name = local.ToString();
		this.begin = local.Start;
		this.end = local.end;
	  }

	  public Declaration(string name, int begin, int end)
	  {
		this.name = name;
		this.begin = begin;
		this.end = end;
	  }

	}

}
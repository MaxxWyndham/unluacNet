using System.Collections.Generic;

namespace unluac.decompile.statement
{


	using Output = unluac.decompile.Output;
	using IfThenElseBlock = unluac.decompile.block.IfThenElseBlock;

	abstract public class Statement
	{

//  *
//   * Prints out a sequences of statements on separate lines. Correctly
//   * informs the last statement that it is last in a block.
//   
	  public static void printSequence(Output @out, List<Statement> stmts)
	  {
		int n = stmts.Count;
		for(int i = 0; i < n; i++)
		{
		  bool last = (i + 1 == n);
		  Statement stmt = stmts[i];
		  Statement next = last ? null : stmts[i + 1];
		  if(last)
		  {
			stmt.printTail(@out);
		  }
		  else
		  {
			stmt.print(@out);
		  }
		  if(next != null && stmt is FunctionCallStatement && next.beginsWithParen())
		  {
			@out.print(";");
		  }
		  if(!(stmt is IfThenElseBlock))
		  {
			@out.println();
		  }
		}
	  }

	  abstract public void print(Output @out);

	  public virtual void printTail(Output @out)
	  {
		print(@out);
	  }

	  public string comment;

	  public virtual void addComment(string comment)
	  {
		this.comment = comment;
	  }

	  public virtual bool beginsWithParen()
	  {
		return false;
	  }

	}

}
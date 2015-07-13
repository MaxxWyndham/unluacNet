using System;

namespace unluac.decompile.expression
{

	using Output = unluac.decompile.Output;

	public class TableReference : Expression
	{

	  private readonly Expression table;
	  private readonly Expression index;

	  public TableReference(Expression table, Expression index) : base(PRECEDENCE_ATOMIC)
	  {
		this.table = table;
		this.index = index;
	  }

	  public override int getConstantIndex()
	  {
		return Math.Max(table.getConstantIndex(), index.getConstantIndex());
	  }

	  public override void print(Output @out)
	  {
		table.print(@out);
		if(index.isIdentifier())
		{
		  @out.print(".");
		  @out.print(index.asName());
		}
		else
		{
		  @out.print("[");
		  index.print(@out);
		  @out.print("]");
		}
	  }

	  public override bool isDotChain()
	  {
		return index.isIdentifier() && table.isDotChain();
	  }

	  public override bool isMemberAccess()
	  {
		return index.isIdentifier();
	  }

	  public override Expression getTable()
	  {
		return table;
	  }

	  public override string getField()
	  {
		return index.asName();
	  }


	}

}
namespace unluac.decompile.target
{

	using Output = unluac.decompile.Output;
	using Expression = unluac.decompile.expression.Expression;
	using TableReference = unluac.decompile.expression.TableReference;

	public class TableTarget : Target
	{

	  private readonly Expression table;
	  private readonly Expression index;

	  public TableTarget(Expression table, Expression index)
	  {
		this.table = table;
		this.index = index;
	  }

	  public override void print(Output @out)
	  {
		new TableReference(table, index).print(@out);
	  }

	  public override void printMethod(Output @out)
	  {
		table.print(@out);
		@out.print(":");
		@out.print(index.asName());
	  }

	  public override bool isFunctionName()
	  {
		if(!index.isIdentifier())
		{
		  return false;
		}
		if(!table.isDotChain())
		{
		  return false;
		}
		return true;
	  }

	}

}
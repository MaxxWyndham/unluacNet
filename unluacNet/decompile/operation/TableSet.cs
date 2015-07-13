namespace unluac.decompile.operation
{

	using Registers = unluac.decompile.Registers;
	using Block = unluac.decompile.block.Block;
	using Expression = unluac.decompile.expression.Expression;
	using TableLiteral = unluac.decompile.expression.TableLiteral;
	using Assignment = unluac.decompile.statement.Assignment;
	using Statement = unluac.decompile.statement.Statement;
	using TableTarget = unluac.decompile.target.TableTarget;

	public class TableSet : Operation
	{

	  private Expression table;
	  private Expression index;
	  private Expression @value;
	  private bool isTable;
	  private int timestamp;

	  public TableSet(int line, Expression table, Expression index, Expression @value, bool isTable, int timestamp) : base(line)
	  {
		this.table = table;
		this.index = index;
		this.value = @value;
		this.isTable = isTable;
		this.timestamp = timestamp;
	  }

	  public override Statement process(Registers r, Block block)
	  {
		if(table.isTableLiteral())
		{
		  table.addEntry(new TableLiteral.Entry(index, @value, !isTable, timestamp));
		  return null;
		}
		else
		{
		  return new Assignment(new TableTarget(table, index), @value);
		}
	  }

	}

}
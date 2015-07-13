using System;
using System.Collections.Generic;

namespace unluac.decompile.expression
{


	using Output = unluac.decompile.Output;

	public class TableLiteral : Expression
	{

	  public class Entry : IComparable<Entry>
	  {

		public readonly Expression key;
		public readonly Expression @value;
		public readonly bool isList;
		public readonly int timestamp;

		public Entry(Expression key, Expression @value, bool isList, int timestamp)
		{
		  this.key = key;
		  this.value = @value;
		  this.isList = isList;
		  this.timestamp = timestamp;
		}

		public virtual int CompareTo(Entry e)
		{
		  return ((int) timestamp).CompareTo(e.timestamp);
		}
	  }

	  private List<Entry> entries;

	  private bool isObject = true;
	  private bool isList = true;
	  private int listLength = 1;

	  public TableLiteral() : this(5, 5)
	  {
	  }

	  public TableLiteral(int arraySize, int hashSize) : base(PRECEDENCE_ATOMIC)
	  {
		entries = new List<Entry>(arraySize + hashSize);
	  }

	  public override int getConstantIndex()
	  {
		int index = -1;
		foreach(Entry entry in entries)
		{
		  index = Math.Max(entry.key.getConstantIndex(), index);
		  index = Math.Max(entry.value.getConstantIndex(), index);
		}
		return index;
	  }

	  public override void print(Output @out)
	  {
          entries.Sort();
		//Collections.sort(entries);
		listLength = 1;
		if(entries.Count == 0)
		{
		  @out.print("{}");
		}
		else
		{
		  bool lineBreak = isList && entries.Count > 5 || isObject && entries.Count > 2 || !isObject;
	  //System.out.println(" -- " + (isList && entries.size() > 5));
	  //System.out.println(" -- " + (isObject && entries.size() > 2));
	  //System.out.println(" -- " + (!isObject));
		  if(!lineBreak)
		  {
			foreach(Entry entry in entries)
			{
			  Expression @value = entry.value;
			  if(!(@value.isBrief()))
			  {
				lineBreak = true;
				break;
			  }
			}
		  }
		  @out.print("{");
		  if(lineBreak)
		  {
			@out.println();
			@out.indent();
		  }
		  printEntry(0, @out);
		  if(!entries[0].value.isMultiple())
		  {
			for(int index = 1; index < entries.Count; index++)
			{
			  @out.print(",");
			  if(lineBreak)
			  {
				@out.println();
			  }
			  else
			  {
				@out.print(" ");
			  }
			  printEntry(index, @out);
			  if(entries[index].value.isMultiple())
			  {
				break;
			  }
			}
		  }
		  if(lineBreak)
		  {
			@out.println();
			@out.dedent();
		  }
		  @out.print("}");
		}
	  }

	  private void printEntry(int index, Output @out)
	  {
		Entry entry = entries[index];
		Expression key = entry.key;
		Expression @value = entry.value;
		bool isList = entry.isList;
		bool multiple = index + 1 >= entries.Count || @value.isMultiple();
		if(isList && key.isInteger() && listLength == key.asInteger())
		{
		  if(multiple)
		  {
			@value.printMultiple(@out);
		  }
		  else
		  {
			@value.print(@out);
		  }
		  listLength++;
		  }
		else if(isObject && key.isIdentifier())
		{
		  @out.print(key.asName());
		  @out.print(" = ");
		  @value.print(@out);
		}
		else
		{
		  @out.print("[");
		  key.print(@out);
		  @out.print("] = ");
		  @value.print(@out);
		}
	  }

	  public override bool isTableLiteral()
	  {
		return true;
	  }

	  public override void addEntry(Entry entry)
	  {
		entries.Add(entry);
		isObject = isObject && (entry.isList || entry.key.isIdentifier());
		isList = isList && entry.isList;
	  }

	  public override bool isBrief()
	  {
		return false;
	  }

	}

}
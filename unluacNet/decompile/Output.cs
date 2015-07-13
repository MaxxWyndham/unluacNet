using System;

namespace unluac.decompile
{

	public class Output
	{
        
	  private Action _outLine;
	  private Action<string> _out;
	  private int indentationLevel = 0;
	  private int position = 0;

	  public Output() : this(s => Console.Write(s),()=>Console.WriteLine())
      {
	  }

	  public Output(Action<string> out1, Action outLine)
	  {
		_out = out1;
        _outLine = outLine;
	  }

	  public virtual void indent()
	  {
		indentationLevel += 2;
	  }

	  public virtual void dedent()
	  {
		indentationLevel -= 2;
	  }

	  public virtual int getIndentationLevel()
	  {
		return indentationLevel;
	  }

	  public virtual int getPosition()
	  {
		return position;
	  }

	  public virtual void setIndentationLevel(int indentationLevel)
	  {
		this.indentationLevel = indentationLevel;
	  }

	  private void start()
	  {
		if(position == 0)
		{
		  for(int i = indentationLevel; i != 0; i--)
		  {
			_out(" ");
			position++;
		  }
		}
	  }

	  public virtual void print(string s)
	  {
		start();
		_out(s);
		position += s.Length;
	  }

	  public virtual void println()
	  {
		start();
		_outLine();
		position = 0;
	  }

	  public virtual void println(string s)
	  {
		print(s);
		println();
	  }

	}

}
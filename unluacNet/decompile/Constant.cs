using System;
using System.Collections.Generic;

namespace unluac.decompile
{


	using LBoolean = unluac.parse.LBoolean;
	using LNil = unluac.parse.LNil;
	using LNumber = unluac.parse.LNumber;
	using LObject = unluac.parse.LObject;
	using LString = unluac.parse.LString;

	public class Constant
	{

	  private static readonly HashSet<string> reservedWords = new HashSet<string>();

	  static Constant()
	  {
		reservedWords.Add("and");
		reservedWords.Add("break");
		reservedWords.Add("do");
		reservedWords.Add("else");
		reservedWords.Add("elseif");
		reservedWords.Add("end");
		reservedWords.Add("false");
		reservedWords.Add("for");
		reservedWords.Add("function");
		reservedWords.Add("if");
		reservedWords.Add("in");
		reservedWords.Add("local");
		reservedWords.Add("nil");
		reservedWords.Add("not");
		reservedWords.Add("or");
		reservedWords.Add("repeat");
		reservedWords.Add("return");
		reservedWords.Add("then");
		reservedWords.Add("true");
		reservedWords.Add("until");
		reservedWords.Add("while");
	  }

	  private readonly int type;

	  private readonly bool @bool;
	  private readonly LNumber number;
	  private readonly string @string;

	  public Constant(int constant)
	  {
		type = 2;
		@bool = false;
		number = LNumber.makeInteger(constant);
		@string = null;
	  }

	  public Constant(LObject constant)
	  {
		if(constant is LNil)
		{
		  type = 0;
		  @bool = false;
		  number = null;
		  @string = null;
		}
		else if(constant is LBoolean)
		{
		  type = 1;
		  @bool = constant == LBoolean.LTRUE;
		  number = null;
		  @string = null;
		}
		else if(constant is LNumber)
		{
		  type = 2;
		  @bool = false;
		  number = (LNumber) constant;
		  @string = null;
		}
		else if(constant is LString)
		{
		  type = 3;
		  @bool = false;
		  number = null;
		  @string = ((LString) constant).deref();
		}
        else if(constant == null)
        {
            throw new Exception("Constant is null!");
        }
		else
		{
		  throw new Exception("Illegal constant type: " + constant.ToString());
		}
	  }

	  public virtual void print(Output @out)
	  {
		switch(type)
		{
		  case 0:
			@out.print("nil");
			break;
		  case 1:
			@out.print(@bool ? "true" : "false");
			break;
		  case 2:
			@out.print(number.ToString());
			break;
		  case 3:
			int newlines = 0;
			int carriageReturns = 0;
			for(int i = 0; i < @string.Length; i++)
			{
			  newlines += @string[i] == '\n' ? 1 : 0;
			  carriageReturns += @string[i] == '\r' ? 1 : 0;
			}
			if(carriageReturns == 0 && (newlines > 1 || (newlines == 1 && @string.IndexOf('\n') != @string.Length - 1)))
			{
			  int pipe = 0;
			  string pipeString = "]]";
			  while(@string.IndexOf(pipeString) >= 0)
			  {
				pipe++;
				pipeString = "]";
				int i = pipe;
				while(i-- > 0)
					pipeString += "=";
				pipeString += "]";
			  }
			  @out.print("[");
			  while(pipe-- > 0)
				  @out.print("=");
			  @out.print("[");
			  int indent = @out.getIndentationLevel();
			  @out.setIndentationLevel(0);
			  @out.println();
			  @out.print(@string);
			  @out.print(pipeString);
			  @out.setIndentationLevel(indent);
			  }
			else
			{
			  @out.print("\"");
			  for(int i = 0; i < @string.Length; i++)
			  {
				char c = @string[i];
				if(c <= 31 || c >= 127)
				{
				  if(c == 7)
				  {
					@out.print("\\a");
				  }
				  else if(c == 8)
				  {
					@out.print("\\b");
				  }
				  else if(c == 12)
				  {
					@out.print("\\f");
				  }
				  else if(c == 10)
				  {
					@out.print("\\n");
				  }
				  else if(c == 13)
				  {
					@out.print("\\r");
				  }
				  else if(c == 9)
				  {
					@out.print("\\t");
				  }
				  else if(c == 11)
				  {
					@out.print("\\v");
				  }
				  else
				  {
					string dec = Convert.ToString(c);
					int len = dec.Length;
					@out.print("\\");
					while(len++ < 3)
					{
					  @out.print("0");
					}
					@out.print(dec);
				  }
					}
				else if(c == 34)
				{
				  @out.print("\\\"");
				}
				else if(c == 92)
				{
				  @out.print("\\\\");
				}
				else
				{
				  @out.print(char.ToString(c));
				}
			  }
			  @out.print("\"");
			}
			break;
		  default:
			throw new Exception();
		}
	  }

	  public virtual bool isNil()
	  {
		return type == 0;
	  }

	  public virtual bool isBoolean()
	  {
		return type == 1;
	  }

	  public virtual bool isNumber()
	  {
		return type == 2;
	  }

	  public virtual bool isInteger()
	  {
		return number.value() == Math.Round(number.value());
	  }

	  public virtual int asInteger()
	  {
		if(!isInteger())
		{
		  throw new Exception();
		}
		return (int) number.value();
	  }

	  public virtual bool isString()
	  {
		return type == 3;
	  }

	  public virtual bool isIdentifier()
	  {
		if(!isString())
		{
		  return false;
		}
		if(reservedWords.Contains(@string))
		{
		  return false;
		}
		if(@string.Length == 0)
		{
		  return false;
		}
		char Start = @string[0];
		if(Start != '_' && !char.IsLetter(Start))
		{
		  return false;
		}
		for(int i = 1; i < @string.Length; i++)
		{
		  char next = @string[i];
		  if(char.IsLetter(next))
		  {
			continue;
		  }
		  if(char.IsDigit(next))
		  {
			continue;
		  }
		  if(next == '_')
		  {
			continue;
		  }
		  return false;
		}
		return true;
	  }

	  public virtual string asName()
	  {
		if(type != 3)
		{
		  throw new Exception();
		}
		return @string;
	  }

	}

}
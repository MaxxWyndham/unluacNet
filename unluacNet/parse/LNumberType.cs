using System;
using System.IO;
using unluacNet;

namespace unluac.parse
{



	public class LNumberType : BObjectType<LNumber>
	{

	  public readonly int size;
	  public readonly bool integral;

	  public LNumberType(int size, bool integral)
	  {
		this.size = size;
		this.integral = integral;
		if(!(size == 4 || size == 8))
		{
		  throw new Exception("The input chunk has an unsupported Lua number size: " + size);
		}
	  }

      public override LNumber parse(ByteBuffer buffer, BHeader header)
	  {
		LNumber @value = null;
		if(integral)
		{
		  switch(size)
		  {
			case 4:
			  @value = new LIntNumber(buffer.GetInt());
			  break;
            case 8:
			  @value = new LLongNumber(buffer.GetLong());
		  break;
		  }
		  }
		else
		{
		  switch(size)
		  {
              case 4:
                  @value = new LFloatNumber(buffer.GetFloat());
			  
			  break;
              case 8:
			  @value = new LDoubleNumber(buffer.GetDouble());
			  break;
		  }
		}
		if(@value == null)
		{
		  throw new Exception("The input chunk has an unsupported Lua number format");
		}
		if(header.debug)
		{
		  Console.WriteLine("-- parsed <number> " + @value);
		}
		return @value;
	  }

	}

}
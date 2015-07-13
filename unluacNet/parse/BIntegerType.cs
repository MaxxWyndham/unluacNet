using System;
using System.Numerics;
using System.IO;
using unluacNet;

namespace unluac.parse
{


	public class BIntegerType : BObjectType<BInteger>
	{

	  public readonly int intSize;

	  public BIntegerType(int intSize)
	  {
		this.intSize = intSize;
	  }

      protected internal virtual BInteger raw_parse(ByteBuffer buffer, BHeader header)
	  {
		BInteger @value;
		switch(intSize)
		{
		  case 0:
			@value = new BInteger(0);
			break;
		  case 1:
			@value = new BInteger(buffer.GetByte());
			break;
		  case 2:
			@value = new BInteger(buffer.GetShort());
			break;
		  case 4:
			@value = new BInteger(buffer.GetInt());
			break;
		  default:
			  {
			byte[] bytes = new byte[intSize];
			int Start = 0;
			int delta = 1;
			if(!BitConverter.IsLittleEndian)
			{
			  Start = intSize - 1;
			  delta = -1;
			}
			for(int i = Start; i >= 0 && i < intSize; i += delta)
			{
			  bytes[i] = (byte)Convert.ToSByte(buffer.GetByte());
			}
			@value = new BInteger(new BigInteger(bytes));
		  }

	  break;
		}
		return @value;
	  }

      public override BInteger parse(ByteBuffer buffer, BHeader header)
	  {
		BInteger @value = raw_parse(buffer, header);
		if(header.debug)
		{
		  Console.WriteLine("-- parsed <integer> " + @value.asInt());
		}
		return @value;
	  }

	}

}
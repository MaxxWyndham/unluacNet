using System;
using System.IO;
using unluacNet;

namespace unluac.parse
{



	public class LConstantType : BObjectType<LObject>
	{

        public override LObject parse(ByteBuffer buffer, BHeader header)
	  {
		int type = 0xFF & buffer.GetByte();
		if(header.debug)
		{
		  Console.Write("-- parsing <constant>, type is ");
		  switch(type)
		  {
			case 0:
			  Console.WriteLine("<nil>");
			  break;
			case 1:
			  Console.WriteLine("<boolean>");
			  break;
			case 3:
			  Console.WriteLine("<number>");
			  break;
			case 4:
			  Console.WriteLine("<strin>");
			  break;
			default:
			  Console.WriteLine("illegal " + type);
			  break;
		  }
		}
		switch(type)
		{
		  case 0:
			return LNil.NIL;
		  case 1:
			return header.@bool.parse(buffer, header);
		  case 3:
			return header.number.parse(buffer, header);
		  case 4:
			return header.@string.parse(buffer, header);
		  default:
			throw new Exception();
		}
	  }

	}

}
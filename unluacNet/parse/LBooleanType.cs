using System;
using System.IO;
using unluacNet;

namespace unluac.parse
{



	public class LBooleanType : BObjectType<LBoolean>
	{

        public override LBoolean parse(ByteBuffer buffer, BHeader header)
	  {
		int @value = buffer.GetByte();
		if((@value & 0xFFFFFFFE) != 0)
		{
		  throw new Exception();
		}
		else
		{
		  LBoolean @bool = @value == 0 ? LBoolean.LFALSE : LBoolean.LTRUE;
		  if(header.debug)
		  {
			Console.WriteLine("-- parsed <boolean> " + @bool);
		  }
		  return @bool;
		}
	  }

	}

}
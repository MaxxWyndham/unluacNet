using System;
using System.IO;
using unluacNet;

namespace unluac.parse
{



	public class LLocalType : BObjectType<LLocal>
	{

        public override LLocal parse(ByteBuffer buffer, BHeader header)
	  {
		LString name = header.@string.parse(buffer, header);
		BInteger Start = header.integer.parse(buffer, header);
		BInteger end = header.integer.parse(buffer, header);
		if(header.debug)
		{
		  Console.Write("-- parsing local, name: ");
		  Console.Write(name);
		  Console.Write(" from " + Start.asInt() + " to " + end.asInt());
		  Console.WriteLine();
		}
		return new LLocal(name, Start, end);
	  }

	}

}
using System;
using System.Threading;
using System.IO;
using unluacNet;

namespace unluac.parse
{



	public class LStringType : BObjectType<LString>
	{

        private ThreadLocal<System.Text.StringBuilder> b = new ThreadLocal<System.Text.StringBuilder>(() => { return new System.Text.StringBuilder(); });

	  public override LString parse(ByteBuffer buffer, BHeader header)
	  {
		BSizeT sizeT = header.sizeT.parse(buffer, header);
		System.Text.StringBuilder b = this.b.Value;
		b.Length = 0;
		sizeT.iterate(()=>b.Append((char)(0xFF & buffer.GetByte())));
		string s = b.ToString();
		if(header.debug)
		{
		  Console.WriteLine("-- parsed <string> \"" + s + "\"");
		}
		return new LString(sizeT, s);
	  }

	}

}
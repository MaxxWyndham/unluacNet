using System.Collections.Generic;
using System.IO;
using unluacNet;

namespace unluac.parse
{


	abstract public class BObjectType<T> where T : BObject
	{

        abstract public T parse(ByteBuffer buffer, BHeader header);
      

//JAVA TO VB & C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public final BList<T> parseList(final ByteBuffer buffer, final BHeader header)
      public BList<T> parseList(ByteBuffer buffer, BHeader header)
	  {
		BInteger length = header.integer.parse(buffer, header);
		List<T> values = new List<T>();
		length.iterate(() => values.Add(parse(buffer,header))) ;
		return new BList<T>(length, values);
	  }


	}

}
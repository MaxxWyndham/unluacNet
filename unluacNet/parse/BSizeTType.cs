using System;
using System.IO;
using unluacNet;

namespace unluac.parse
{


	public class BSizeTType : BObjectType<BSizeT>
	{

	  public readonly int sizeTSize;

	  private BIntegerType integerType;

	  public BSizeTType(int sizeTSize)
	  {
		this.sizeTSize = sizeTSize;
		integerType = new BIntegerType(sizeTSize);
	  }


      public override BSizeT parse(ByteBuffer buffer, BHeader header)
	  {
		BSizeT @value = new BSizeT(integerType.raw_parse(buffer, header));
		if(header.debug)
		{
		  Console.WriteLine("-- parsed <size_t> " + @value.asInt());
		}
		return @value;
	  }

	}

}
using System.IO;
using unluacNet;
namespace unluac.parse
{


	public class LUpvalueType : BObjectType<LUpvalue>
	{

        public override LUpvalue parse(ByteBuffer buffer, BHeader header)
	  {
		LUpvalue upvalue = new LUpvalue();
		upvalue.instack = buffer.GetByte() != 0;
		upvalue.idx = 0xFF & buffer.GetByte();
		return upvalue;
	  }

	}
}
using System;
using System.IO;

namespace unluac.parse
{


	public class LSourceLines
	{

	  public static LSourceLines parse(MemoryStream buffer)
	  {
          byte[] byteArray = new byte[4];
          byteArray[0] = Convert.ToByte(buffer.ReadByte());
          byteArray[1] = Convert.ToByte(buffer.ReadByte());
          byteArray[2] = Convert.ToByte(buffer.ReadByte());
          byteArray[3] = Convert.ToByte(buffer.ReadByte());
          int number = BitConverter.ToInt32(byteArray, 0); //buffer.getInt();
		while(number-- > 0)
        {
            buffer.ReadByte();
            buffer.ReadByte();
            buffer.ReadByte();
            buffer.ReadByte();
		}
		return null;
	  }

	}

}
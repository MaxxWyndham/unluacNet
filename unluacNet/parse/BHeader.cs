using System;
using System.IO;
using unluacNet;


namespace unluac.parse
{


    using Version = unluac.Version;


    public class BHeader
    {

        private static readonly sbyte[] signature = { 0x1B, 0x4C, 0x75, 0x61 };

        private static readonly byte[] luacTail = { 0x19, 0x93, 0x0D, 0x0A, 0x1A, 0x0A };

        public readonly bool debug = false;

        public readonly Version version;

        public readonly BIntegerType integer;
        public readonly BSizeTType sizeT;
        public readonly LBooleanType @bool;
        public readonly LNumberType number;
        public readonly LStringType @string;
        public readonly LConstantType constant;
        public readonly LLocalType local;
        public readonly LUpvalueType upvalue;
        public readonly LFunctionType function;
        public int Endianness;

        public BHeader(ByteBuffer buffer)
        {
            // 4 byte Lua signature
            for (int i = 0; i < signature.Length; i++)
            {
                byte sigByte = buffer.GetByte();

                //Console.WriteLine("Header reading byte: " + sigByte + " is meant to be " + signature[i]);
                if (sigByte != signature[i])
                {
                    throw new Exception("The input file does not have the signature of a valid Lua file.");
                }
            }
            // 1 byte Lua version
            int versionNumber = 0xFF & buffer.GetByte();
            switch (versionNumber)
            {
                case 0x51:
                    version = Version.LUA51;
                    break;
                case 0x52:
                    version = Version.LUA52;
                    break;
                default:
                    int major = versionNumber >> 4;
                    int minor = versionNumber & 0x0F;
                    throw new Exception("The input chunk's Lua version is " + major + "." + minor + "; unluac can only handle Lua 5.1 and Lua 5.2.");
            }
            if (debug)
            {
                Console.WriteLine("-- version: 0x" + String.Format("{0:x}", versionNumber));
            }
            // 1 byte Lua "format"
            int format = 0xFF & buffer.GetByte();
            if (format != 0)
            {
                throw new Exception("The input chunk reports a non-standard lua format: " + format);
            }
            if (debug)
            {
                Console.WriteLine("-- format: " + format);
            }
            // 1 byte endianness
            Endianness = 0xFF & buffer.GetByte();
            switch (Endianness)
            {
                case 0:
                    //buffer.order(ByteOrder.BIG_ENDIAN);
                    break;
                case 1:
                    //buffer.order(ByteOrder.LITTLE_ENDIAN);
                    break;
                default:
                    throw new Exception("The input chunk reports an invalid endianness: " + Endianness);
            }
            if (debug)
            {
                Console.WriteLine("-- endianness: " + Endianness + (Endianness == 0 ? " (big)" : " (little)"));
            }
            // 1 byte int size
            int intSize = 0xFF & buffer.GetByte();
            if (debug)
            {
                Console.WriteLine("-- int size: " + intSize);
            }
            integer = new BIntegerType(intSize);
            // 1 byte sizeT size
            int sizeTSize = 0xFF & buffer.GetByte();
            if (debug)
            {
                Console.WriteLine("-- size_t size: " + sizeTSize);
            }
            sizeT = new BSizeTType(sizeTSize);
            // 1 byte instruction size
            int instructionSize = 0xFF & buffer.GetByte();
            if (debug)
            {
                Console.WriteLine("-- instruction size: " + instructionSize);
            }
            if (instructionSize != 4)
            {
                throw new Exception("The input chunk reports an unsupported instruction size: " + instructionSize + " bytes");
            }
            int lNumberSize = 0xFF & buffer.GetByte();
            if (debug)
            {
                Console.WriteLine("-- Lua number size: " + lNumberSize);
            }
            int lNumberIntegralCode = 0xFF & buffer.GetByte();
            if (debug)
            {
                Console.WriteLine("-- Lua number integral code: " + lNumberIntegralCode);
            }
            if (lNumberIntegralCode > 1)
            {
                throw new Exception("The input chunk reports an invalid code for lua number integralness: " + lNumberIntegralCode);
            }
            bool lNumberIntegral = (lNumberIntegralCode == 1);
            number = new LNumberType(lNumberSize, lNumberIntegral);
            @bool = new LBooleanType();
            @string = new LStringType();
            constant = new LConstantType();
            local = new LLocalType();
            upvalue = new LUpvalueType();
            function = version.getLFunctionType();
            if (version.hasHeaderTail())
            {
                for (int i = 0; i < luacTail.Length; i++)
                {
                    if (buffer.GetByte() != luacTail[i])
                    {
                        throw new Exception("The input file does not have the header tail of a valid Lua file.");
                    }
                }
            }
        }

    }
}
using System;
using System.IO;
using unluacNet;

namespace unluac.parse
{



	public class LFunctionType : BObjectType<LFunction>
	{

	  public static readonly LFunctionType TYPE51 = new LFunctionType();
	  public static readonly LFunctionType TYPE52 = new LFunctionType52();

	  protected internal class LFunctionParseState
	  {

		public LString name;
		internal int lineBegin;
		internal int lineEnd;
		internal int lenUpvalues;
		internal int lenParameter;
		internal int vararg;
		internal int maximumStackSize;
		internal int length;
		internal int[] code;
		internal BList<LObject> constants;
		internal BList<LFunction> functions;
		internal BList<BInteger> lines;
		internal BList<LLocal> locals;
		internal LUpvalue[] upvalues;
	  }

	  public override LFunction parse(ByteBuffer buffer, BHeader header)
	  {
		if(header.debug)
		{
		  Console.WriteLine("-- beginning to parse function");
		}
		if(header.debug)
		{
		  Console.WriteLine("-- parsing name...start...end...upvalues...params...varargs...stack");
		}
		LFunctionParseState s = new LFunctionParseState();
		parse_main(buffer, header, s);
		return new LFunction(header, s.code, s.locals.asArray(new LLocal[s.locals.length.asInt()]), s.constants.asArray(new LObject[s.constants.length.asInt()]), s.upvalues, s.functions.asArray(new LFunction[s.functions.length.asInt()]), s.maximumStackSize, s.lenUpvalues, s.lenParameter, s.vararg);
	  }

	  protected internal virtual void parse_main(ByteBuffer buffer, BHeader header, LFunctionParseState s)
	  {
		s.name = header.@string.parse(buffer, header);
		s.lineBegin = header.integer.parse(buffer, header).asInt();
		s.lineEnd = header.integer.parse(buffer, header).asInt();
        s.lenUpvalues = 0xFF & buffer.GetByte();
        s.lenParameter = 0xFF & buffer.GetByte();
        s.vararg = 0xFF & buffer.GetByte();
        s.maximumStackSize = 0xFF & buffer.GetByte();
		parse_code(buffer, header, s);
		parse_constants(buffer, header, s);
		parse_upvalues(buffer, header, s);
		parse_debug(buffer, header, s);
	  }

      protected internal virtual void parse_code(ByteBuffer buffer, BHeader header, LFunctionParseState s)
	  {
		if(header.debug)
		{
		  Console.WriteLine("-- beginning to parse bytecode list");
		}
		s.length = header.integer.parse(buffer, header).asInt();
		s.code = new int[s.length];
		for(int i = 0; i < s.length; i++)
		{
            /*byte[] intByteArray = new byte[4];
            intByteArray[0] = Convert.ToByte(buffer.ReadByte());
            intByteArray[1] = Convert.ToByte(buffer.ReadByte());
            intByteArray[2] = Convert.ToByte(buffer.ReadByte());
            intByteArray[3] = Convert.ToByte(buffer.ReadByte());*/
            s.code[i] = buffer.GetInt(); //BitConverter.ToInt32(intByteArray, 0);//buffer.getInt();
		  if(header.debug)
		  {
			Console.WriteLine("-- parsed codepoint " + s.code[i].ToString("X8"));
		  }
		}
	  }

      protected internal virtual void parse_constants(ByteBuffer buffer, BHeader header, LFunctionParseState s)
	  {
		if(header.debug)
		{
		  Console.WriteLine("-- beginning to parse constants list");
		}
		s.constants = header.constant.parseList(buffer, header);
		if(header.debug)
		{
		  Console.WriteLine("-- beginning to parse functions list");
		}
		s.functions = header.function.parseList(buffer, header);
	  }

      protected internal virtual void parse_debug(ByteBuffer buffer, BHeader header, LFunctionParseState s)
	  {
		if(header.debug)
		{
		  Console.WriteLine("-- beginning to parse source lines list");
		}
		s.lines = header.integer.parseList(buffer, header);
		if(header.debug)
		{
		  Console.WriteLine("-- beginning to parse locals list");
		}
		s.locals = header.local.parseList(buffer, header);
		if(header.debug)
		{
		  Console.WriteLine("-- beginning to parse upvalues list");
		}
		BList<LString> upvalueNames = header.@string.parseList(buffer, header);
		for(int i = 0; i < upvalueNames.length.asInt(); i++)
		{
		  s.upvalues[i].name = upvalueNames.get(i).deref();
		}
	  }

      protected internal virtual void parse_upvalues(ByteBuffer buffer, BHeader header, LFunctionParseState s)
	  {
		s.upvalues = new LUpvalue[s.lenUpvalues];
		for(int i = 0; i < s.lenUpvalues; i++)
		{
		  s.upvalues[i] = new LUpvalue();
		}
	  }

	}

	internal class LFunctionType52 : LFunctionType
	{

        protected internal override void parse_main(ByteBuffer buffer, BHeader header, LFunctionParseState s)
	  {
		s.lineBegin = header.integer.parse(buffer, header).asInt();
		s.lineEnd = header.integer.parse(buffer, header).asInt();
		s.lenParameter = 0xFF & buffer.GetByte();
        s.vararg = 0xFF & buffer.GetByte();
        s.maximumStackSize = 0xFF & buffer.GetByte();
		parse_code(buffer, header, s);
		parse_constants(buffer, header, s);
		parse_upvalues(buffer, header, s);
		parse_debug(buffer, header, s);
	  }

        protected internal override void parse_debug(ByteBuffer buffer, BHeader header, LFunctionParseState s)
	  {
		s.name = header.@string.parse(buffer, header);
		base.parse_debug(buffer, header, s);
	  }

        protected internal override void parse_upvalues(ByteBuffer buffer, BHeader header, LFunctionParseState s)
	  {
		BList<LUpvalue> upvalues = header.upvalue.parseList(buffer, header);
		s.lenUpvalues = upvalues.length.asInt();
		s.upvalues = upvalues.asArray(new LUpvalue[s.lenUpvalues]);
	  }
	}
}
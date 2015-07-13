using System;
namespace unluac.decompile
{

	using LFunction = unluac.parse.LFunction;

	public class Code
	{

	  public static int extract_A(int codepoint)
	  {
		return (codepoint >> 6) & 0x0000000FF;
	  }

	  public static int extract_C(int codepoint)
	  {
		return (codepoint >> 14) & 0x000001FF;
	  }

	  public static int extract_B(int codepoint)
	  {
//JAVA TO VB & C# CONVERTER TODO TASK: There is no '>>>' operator in .NET:
          return (int)(((uint)codepoint) >> 23);
	  }

	  public static int extract_Bx(int codepoint)
	  {
//JAVA TO VB & C# CONVERTER TODO TASK: There is no '>>>' operator in .NET:
          return (int)(((uint)codepoint) >> 14);
	  }

	  public static int extract_sBx(int codepoint)
	  {
//JAVA TO VB & C# CONVERTER TODO TASK: There is no '>>>' operator in .NET:
		return (int)(((uint)codepoint) >> 14) - 131071;
	  }
	  private readonly OpcodeMap map;
	  private readonly int[] code;

	  public Code(LFunction function)
	  {
		this.code = function.code;
		map = function.header.version.getOpcodeMap();
	  }

  //public boolean reentered = false;

	  public virtual Op op(int line)
	  {
//    if(!reentered) {
//      reentered = true;
//      System.out.println("line " + line + ": " + toString(line));
//      reentered = false;
//    }
		return map.@get(code[line - 1] & 0x0000003F);
	  }

	  public virtual int A(int line)
	  {
		return extract_A(code[line - 1]);
	  }

	  public virtual int C(int line)
	  {
		return extract_C(code[line - 1]);
	  }

	  public virtual int B(int line)
	  {
		return extract_B(code[line - 1]);
	  }

	  public virtual int Bx(int line)
	  {
		return extract_Bx(code[line - 1]);
	  }

	  public virtual int sBx(int line)
	  {
		return extract_sBx(code[line - 1]);
	  }

	  public virtual int codepoint(int line)
	  {
		return code[line - 1];
	  }

	  public virtual string ToString(int line)
	  {
		return OpClass.Ops[(Op)line].codePointToString(codepoint(line));
	  }

	}

}
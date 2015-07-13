using System;
using System.Collections.Generic;
namespace unluac.decompile
{

	public enum Op
	{
  // Lua 5.1 Opcodes
	  MOVE, //=OpcodeFormat.A_B,
	  LOADK, //=OpcodeFormat.A_Bx,
	  LOADBOOL, //=OpcodeFormat.A_B_C,
	  LOADNIL, //=OpcodeFormat.A_B,
	  GETUPVAL, //=OpcodeFormat.A_B,
	  GETGLOBAL, //=OpcodeFormat.A_Bx,
	  GETTABLE, //=OpcodeFormat.A_B_C,
	  SETGLOBAL, //=OpcodeFormat.A_Bx,
	  SETUPVAL, //=OpcodeFormat.A_B,
	  SETTABLE, //=OpcodeFormat.A_B_C,
	  NEWTABLE, //=OpcodeFormat.A_B_C,
	  SELF,//=OpcodeFormat.A_B_C,
	  ADD,//=OpcodeFormat.A_B_C,
	  SUB,//=OpcodeFormat.A_B_C,
	  MUL,//=OpcodeFormat.A_B_C,
	  DIV,//=OpcodeFormat.A_B_C,
	  MOD,//=OpcodeFormat.A_B_C,
	  POW,//=OpcodeFormat.A_B_C,
	  UNM,//=OpcodeFormat.A_B,
	  NOT,//=OpcodeFormat.A_B,
	  LEN,//=OpcodeFormat.A_B,
	  CONCAT,//=OpcodeFormat.A_B_C,
	  JMP,//=OpcodeFormat.sBx, // Different in 5.2
	  EQ,//=OpcodeFormat.A_B_C,
	  LT,//=OpcodeFormat.A_B_C,
	  LE,//=OpcodeFormat.A_B_C,
	  TEST,//=OpcodeFormat.A_C,
	  TESTSET,//=OpcodeFormat.A_B_C,
	  CALL,//=OpcodeFormat.A_B_C,
	  TAILCALL,//=OpcodeFormat.A_B_C,
	  RETURN,//=OpcodeFormat.A_B,
	  FORLOOP,//=OpcodeFormat.A_sBx,
	  FORPREP,//=OpcodeFormat.A_sBx,
	  TFORLOOP,//=OpcodeFormat.A_C,
	  SETLIST,//=OpcodeFormat.A_B_C,
	  CLOSE,//=OpcodeFormat.A,
	  CLOSURE,//=OpcodeFormat.A_Bx,
	  VARARG,//=OpcodeFormat.A_B,
  // Lua 5.2 Opcodes
	  LOADKX,//=OpcodeFormat.A,
	  GETTABUP,//=OpcodeFormat.A_B_C,
	  SETTABUP,//=OpcodeFormat.A_B_C,
	  TFORCALL,//=OpcodeFormat.A_C,
	  EXTRAARG,//=OpcodeFormat.Ax
        NULL
    }
    

}
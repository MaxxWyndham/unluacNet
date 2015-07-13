using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unluac.decompile
{
    public class OpClass
    {
        //JAVA TO VB & C# CONVERTER TODO TASK: Enums cannot contain fields in .NET:
        private OpcodeFormat format;
        private Op opType;
        public static Dictionary<Op, OpClass> Ops;

        static OpClass()
        {
            Ops = new Dictionary<Op, OpClass>();
            Ops.Add(Op.MOVE, new OpClass(OpcodeFormat.A_B, Op.MOVE));
            Ops.Add(Op.LOADK, new OpClass(OpcodeFormat.A_Bx, Op.LOADK));
            Ops.Add(Op.LOADBOOL, new OpClass(OpcodeFormat.A_B_C, Op.LOADBOOL));
            Ops.Add(Op.LOADNIL, new OpClass(OpcodeFormat.A_B, Op.LOADNIL));
            Ops.Add(Op.GETUPVAL, new OpClass(OpcodeFormat.A_B, Op.GETUPVAL));
            Ops.Add(Op.GETGLOBAL, new OpClass(OpcodeFormat.A_Bx, Op.GETGLOBAL));
            Ops.Add(Op.GETTABLE, new OpClass(OpcodeFormat.A_B_C, Op.GETTABLE));
            Ops.Add(Op.SETGLOBAL, new OpClass(OpcodeFormat.A_Bx, Op.SETGLOBAL));
            Ops.Add(Op.SETUPVAL, new OpClass(OpcodeFormat.A_B, Op.SETUPVAL));
            Ops.Add(Op.SETTABLE, new OpClass(OpcodeFormat.A_B_C, Op.SETTABLE));
            Ops.Add(Op.NEWTABLE, new OpClass(OpcodeFormat.A_B_C, Op.NEWTABLE));
            Ops.Add(Op.SELF, new OpClass(OpcodeFormat.A_B_C, Op.SELF));
            Ops.Add(Op.ADD, new OpClass(OpcodeFormat.A_B_C, Op.ADD));
            Ops.Add(Op.SUB, new OpClass(OpcodeFormat.A_B_C, Op.SUB));
            Ops.Add(Op.MUL, new OpClass(OpcodeFormat.A_B_C, Op.MUL));
            Ops.Add(Op.DIV, new OpClass(OpcodeFormat.A_B_C, Op.DIV));
            Ops.Add(Op.MOD, new OpClass(OpcodeFormat.A_B_C, Op.MOD));
            Ops.Add(Op.POW, new OpClass(OpcodeFormat.A_B_C, Op.POW));
            Ops.Add(Op.UNM, new OpClass(OpcodeFormat.A_B, Op.UNM));
            Ops.Add(Op.NOT, new OpClass(OpcodeFormat.A_B, Op.NOT));
            Ops.Add(Op.LEN, new OpClass(OpcodeFormat.A_B, Op.LEN));
            Ops.Add(Op.CONCAT, new OpClass(OpcodeFormat.A_B_C, Op.CONCAT));
            Ops.Add(Op.JMP, new OpClass(OpcodeFormat.sBx, Op.JMP));
            Ops.Add(Op.EQ, new OpClass(OpcodeFormat.A_B_C, Op.EQ));
            Ops.Add(Op.LT, new OpClass(OpcodeFormat.A_B_C, Op.LT));
            Ops.Add(Op.LE, new OpClass(OpcodeFormat.A_B_C, Op.LE));
            Ops.Add(Op.TEST, new OpClass(OpcodeFormat.A_C, Op.TEST));
            Ops.Add(Op.TESTSET, new OpClass(OpcodeFormat.A_B_C, Op.TESTSET));
            Ops.Add(Op.CALL, new OpClass(OpcodeFormat.A_B_C, Op.CALL));
            Ops.Add(Op.TAILCALL, new OpClass(OpcodeFormat.A_B_C, Op.TAILCALL));
            Ops.Add(Op.RETURN, new OpClass(OpcodeFormat.A_B, Op.RETURN));
            Ops.Add(Op.FORLOOP, new OpClass(OpcodeFormat.A_sBx, Op.FORLOOP));
            Ops.Add(Op.FORPREP, new OpClass(OpcodeFormat.A_sBx, Op.FORPREP));
            Ops.Add(Op.TFORLOOP, new OpClass(OpcodeFormat.A_C, Op.TFORLOOP));
            Ops.Add(Op.SETLIST, new OpClass(OpcodeFormat.A_B_C, Op.SETLIST));
            Ops.Add(Op.CLOSE, new OpClass(OpcodeFormat.A, Op.CLOSE));
            Ops.Add(Op.CLOSURE, new OpClass(OpcodeFormat.A_Bx, Op.CLOSURE));
            Ops.Add(Op.VARARG, new OpClass(OpcodeFormat.A_B, Op.VARARG));
            Ops.Add(Op.LOADKX, new OpClass(OpcodeFormat.A, Op.LOADKX));
            Ops.Add(Op.GETTABUP, new OpClass(OpcodeFormat.A_B_C, Op.GETTABUP));
            Ops.Add(Op.SETTABUP, new OpClass(OpcodeFormat.A_B_C, Op.SETTABUP));
            Ops.Add(Op.TFORCALL, new OpClass(OpcodeFormat.A_C, Op.TFORCALL));
            Ops.Add(Op.EXTRAARG, new OpClass(OpcodeFormat.Ax, Op.EXTRAARG));
        }

        //JAVA TO VB & C# CONVERTER TODO TASK: Enums cannot contain methods in .NET:
        private OpClass(OpcodeFormat format, Op opType)
        {
            this.format = format;
            this.opType = opType;
        }

        //JAVA TO VB & C# CONVERTER TODO TASK: Enums cannot contain methods in .NET:
        public String codePointToString(int codepoint)
        {
            switch (format)
            {
                case OpcodeFormat.A:
                    return opType.ToString() + " " + Code.extract_A(codepoint);
                case OpcodeFormat.A_B:
                    return this.opType.ToString() + " " + Code.extract_A(codepoint) + " " + Code.extract_B(codepoint);
                case OpcodeFormat.A_C:
                    return this.opType.ToString() + " " + Code.extract_A(codepoint) + " " + Code.extract_C(codepoint);
                case OpcodeFormat.A_B_C:
                    return this.opType.ToString() + " " + Code.extract_A(codepoint) + " " + Code.extract_B(codepoint) + " " + Code.extract_C(codepoint);
                case OpcodeFormat.A_Bx:
                    return this.opType.ToString() + " " + Code.extract_A(codepoint) + " " + Code.extract_Bx(codepoint);
                case OpcodeFormat.A_sBx:
                    return this.opType.ToString() + " " + Code.extract_A(codepoint) + " " + Code.extract_sBx(codepoint);
                case OpcodeFormat.Ax:
                    return this.opType.ToString() + " <Ax>";
                case OpcodeFormat.sBx:
                    return this.opType.ToString() + " " + Code.extract_sBx(codepoint);
                default:
                    return this.opType.ToString();
            }
        }
    }
}

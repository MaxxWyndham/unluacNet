namespace unluac
{

	using Op = unluac.decompile.Op;
	using OpcodeMap = unluac.decompile.OpcodeMap;
	using LFunctionType = unluac.parse.LFunctionType;

	public abstract class Version
	{

	  public static readonly Version LUA51 = new Version51();
	  public static readonly Version LUA52 = new Version52();

	  protected internal readonly int versionNumber;

	  protected internal Version(int versionNumber)
	  {
		this.versionNumber = versionNumber;
	  }

	  public abstract bool hasHeaderTail();

	  public abstract LFunctionType getLFunctionType();

	  public virtual OpcodeMap getOpcodeMap()
	  {
		return new OpcodeMap(versionNumber);
	  }

	  public abstract int getOuterBlockScopeAdjustment();

	  public abstract bool usesOldLoadNilEncoding();

	  public abstract bool usesInlineUpvalueDeclarations();

	  public abstract Op getTForTarget();

	  public abstract bool isBreakableLoopEnd(Op op);

	}

	internal class Version51 : Version
	{

	  internal Version51() : base(0x51)
	  {
	  }

	  public override bool hasHeaderTail()
	  {
		return false;
	  }

	  public override LFunctionType getLFunctionType()
	  {
		return LFunctionType.TYPE51;
	  }

	  public override int getOuterBlockScopeAdjustment()
	  {
		return -1;
	  }

	  public override bool usesOldLoadNilEncoding()
	  {
		return true;
	  }

	  public override bool usesInlineUpvalueDeclarations()
	  {
		return true;
	  }

	  public override Op getTForTarget()
	  {
		return Op.TFORLOOP;
	  }

	  public override bool isBreakableLoopEnd(Op op)
	  {
		return op == Op.JMP || op == Op.FORLOOP;
	  }

	}

	internal class Version52 : Version
	{

	  internal Version52() : base(0x52)
	  {
	  }

	  public override bool hasHeaderTail()
	  {
		return true;
	  }

	  public override LFunctionType getLFunctionType()
	  {
		return LFunctionType.TYPE52;
	  }

	  public override int getOuterBlockScopeAdjustment()
	  {
		return 0;
	  }

	  public override bool usesOldLoadNilEncoding()
	  {
		return false;
	  }

	  public override bool usesInlineUpvalueDeclarations()
	  {
		return false;
	  }

	  public override Op getTForTarget()
	  {
		return Op.TFORCALL;
	  }

	  public override bool isBreakableLoopEnd(Op op)
	  {
		return op == Op.JMP || op == Op.FORLOOP || op == Op.TFORLOOP;
	  }

	}
}
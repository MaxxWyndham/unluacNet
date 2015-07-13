using System;
namespace unluac.parse
{


	abstract public class LObject : BObject
	{

	  public virtual string deref()
	  {
          throw new Exception();
	  }

	  abstract public bool Equals(object o);

	}

}
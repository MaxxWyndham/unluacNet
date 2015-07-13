using System.Collections.Generic;

namespace unluac.parse
{


	public class BList<T> : BObject where T : BObject
	{

	  public readonly BInteger length;
	  private readonly List<T> values;

	  public BList(BInteger length, List<T> values)
	  {
		this.length = length;
		this.values = values;
	  }

	  public virtual T @get(int index)
	  {
		return values[index];
	  }

//JAVA TO VB & C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public T[] asArray(final T[] array)
	  public virtual T[] asArray(T[] array)
	  {
          int i = 0;
          length.iterate(() => { array[i] = values[i]; i++; });
            /*new Runnable()
//JAVA TO VB & C# CONVERTER TODO TASK: Anonymous inner classes are not converted to .NET:
		{

		  private int i = 0;

		  public void run()
		  {
			array[i] = values[i];
			i++;
		  }

		  }
	   );*/
		return array;
	  }

	}

}
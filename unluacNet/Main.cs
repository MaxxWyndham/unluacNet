using System;
using System.IO;

namespace unluac
{


	using Decompiler = unluac.decompile.Decompiler;
	using Output = unluac.decompile.Output;
	using OutputProvider = unluac.decompile.OutputProvider;
	using BHeader = unluac.parse.BHeader;
	using LFunction = unluac.parse.LFunction;

	//public class Main1
	//{
/*
	  public static string version = "1.1.0";

	  static void Main2(string[] args)
	  {
		if(args.Length == 0 || args.Length > 1)
		{
		  Console.WriteLine("unluac v" + version);
		  if(args.Length == 0)
		  {
			Console.WriteLine("  error: no input file provided");
		  }
		  else
		  {
			Console.WriteLine("  error: too many arguments");
		  }
		  Console.WriteLine("  usage: java -jar unluac.jar <file>");
		  Environment.Exit(1);
		  }
		else
		{
		  string fn = args[0];
		  LFunction lmain = null;
		  try
		  {
			lmain = file_to_function(fn);
		  }
		  catch(IOException e)
		  {
			Console.WriteLine("unluac v" + version);
			Console.Write("  error: ");
			Console.WriteLine(e.Message);
			Environment.Exit(1);
		  }
		  Decompiler d = new Decompiler(lmain);
		  d.decompile();
		  d.print();
		  Environment.Exit(0);
		}
	  }*/

//JAVA TO VB & C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: private static LFunction file_to_function(String fn) throws IOException
	/*  private static LFunction file_to_function(string fn)
	  {
		FileStream file = File.OpenRead(fn);
        ByteBuffer buffer = new MemoryStream((int)file.Length);
        
		//buffer.order(ByteOrder.LITTLE_ENDIAN);
		int len = (int) file.Length;

          byte[] byteArray = new byte[len];
          file.Read(byteArray,0,len);

		//FileChannel @in = file.getChannel();
		buffer.Write(byteArray,0,len);
        
		
		BHeader header = new BHeader(buffer);
		return header.function.parse(buffer, header);
	  }*/

//JAVA TO VB & C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void decompile(String in, String out) throws IOException
	  //public static void decompile(string @in, out string @out)
	  //{
		/*LFunction lmain = file_to_function(@in);
		Decompiler d = new Decompiler(lmain);
		d.decompile();
		//const PrintStream pout = new PrintStream(@out);
        using (MemoryStream outStream = new MemoryStream())
        {
            using (StreamWriter pout = new StreamWriter(outStream))
            {
                d.print(new Output((s) => { pout.Write(s); }, () => { pout.WriteLine(); }));
                pout.Flush();
                byte[] streamOutput = new byte[outStream.Length];
                outStream.Read(streamOutput, 0, (int)outStream.Length);
                @out = BitConverter.ToString(streamOutput, 0);
            }
        }*/
//JAVA TO VB & C# CONVERTER TODO TASK: Anonymous inner classes are not converted to .NET:
		/*{

		  public void print(string s)
		  {
			pout.print(s);
		  }

		  public void println()
		  {
			pout.println();
		  }

		  }
	   ));*/
		//pout.flush();
		//pout.Close();
	  //}

	//}
//
}
using System;
using System.Linq;

namespace pl_projekt
{

  class Test : Serializable
  {
    public string ToXml()
    {
      return "<kunda>jebat</kunda>";
    }
  }
  
  class Program
  {
    static void Main(string[] args)
    {

      LinkedList<Test> lst = new LinkedList<Test>();
       
      lst.Append(new Test());
      lst.Append(new Test());
      lst.Append(new Test());
      
      foreach (var t in lst)
      {
        Console.WriteLine(t.ToXml());
      }

    }
  }
}
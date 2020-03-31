using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace pl_projekt
{
  public class FleetManager
  {
    private LinkedList<Aircraft> fleet = new LinkedList<Aircraft>();
    private string filename = null;

    void TryLoad(string file)
    {
      XDocument doc = XDocument.Load(file);
      XElement fl = doc.Root;
      fleet = new LinkedList<Aircraft>();
      foreach (var element in fl.Elements())
      {
        fleet.Insert(new Aircraft(element));
      }
    }
    
    void LoadFleet(string file)
    {
      try
      {
        TryLoad(file);
        filename = file;
      }
      catch (Exception e)
      {
        Console.WriteLine("Fleet could not be loaded. ");
        fleet = new LinkedList<Aircraft>();
      }
    }

    static void PrintPrompt()
    {
      Console.Write("$> ");
      Console.Out.Flush();
    }
    
    static String GetPath()
    {
      Console.WriteLine("Input file path or press enter for default path (./fleet.xml).");
      PrintPrompt();
      string i = Console.ReadLine();
      return String.IsNullOrWhiteSpace(i) ? "fleet.xml" : i.Trim();
    }

    static bool YNQuestion(string question)
    {
      string input = "";
      string q = String.Format("{0} [Y/n] ", question);
      while (input != "y" && input != "n")
      {
        Console.Write(q);
        Console.Out.Flush();
        input = Console.ReadLine();
        if (String.IsNullOrWhiteSpace(input))
        {
          continue;
        }
        input = input.ToLower().Trim();
      }
      return input == "y";
    }
    
    static bool ShouldLoadFile()
    {
      return YNQuestion("Load from existing file?");
    }
    
    void Init()
    {
      if (ShouldLoadFile()) { LoadFleet(GetPath()); }
    }

    void PrintFleet()
    {
      foreach (var ac in fleet)
      {
        Console.WriteLine(ac);
      }
    }

    void Store(String file)
    {
      var writer = new StreamWriter(file);
      
      writer.WriteLine("<fleet>");
      foreach (var ac in fleet)
      {
        writer.Write("    ");
        writer.WriteLine(ac.ToXml());
      }
      writer.WriteLine("</fleet>");
      writer.Flush();
      writer.Close();
    }

    string AskPersistently(string question)
    {
      string input = "";

      while (String.IsNullOrWhiteSpace(input))
      {
        Console.Write(question);
        Console.Out.Flush();
        input = Console.ReadLine();
      }

      return input;

    }

    int AskForIntPersistently(string question)
    {
      string input = "";
      int val = 0;
      while (!int.TryParse(input, out val))
      {
        input = AskPersistently(question);
      }

      return val;
    }

    void AddAircraft()
    {
      string manufacturer = AskPersistently("Manufacturer: ");
      string type = AskPersistently("Type: ");
      string registration = AskPersistently("Registration: ");
      Console.Write("Airline (blank for private aircraft): ");
      Console.Out.Flush();
      string? airline = Console.ReadLine();
      airline = String.IsNullOrWhiteSpace(airline) ? null : airline;
      int economySeats = AskForIntPersistently("Economy Seats: ");
      int businessSeats = AskForIntPersistently("Business Seats: ");
      bool isAvailable = YNQuestion("Is Available?");
      
      fleet.Insert(new Aircraft(manufacturer, type, registration, airline, economySeats, businessSeats, isAvailable));
    }

    string AskOnce(string question)
    {
      Console.WriteLine(question);
      PrintPrompt();
      string input;
      input = Console.ReadLine();
      return input;
    }

    int AskIntOrEmpty(string question)
    {
      int res = -1;
      string input = "";
      do
      {
        input = AskOnce(question);
      } while (!String.IsNullOrWhiteSpace(input) && !int.TryParse(input, out res));

      return res;
    }

    static string ReplaceIfNotEmpty(string newVal, String old)
    {
      return String.IsNullOrWhiteSpace(newVal) ? old : newVal;
    }

    static int ReplaceIfNotNegative(int newVal, int old)
    {
      return newVal >= 0 ? newVal : old;
    }
    
    void ModifyAircraft(ref Aircraft ac)
    {
      
      Console.WriteLine("Modyfing aircraft {0}", ac);

      string manufacturer = AskOnce("Manufacturer (blank to keep current): ");
      string type = AskOnce("Type (blank to keep current): ");
      string registration = AskOnce("Registration (blank to keep current): ");
      string airline = AskOnce("Airline (blank to keep current, 0 for private aircraft): ");
      int economySeats = AskIntOrEmpty("Economy Seats (negative or empty keep current): ");
      int businessSeats = AskIntOrEmpty("Business Seats (negative or empty keep current): ");
      bool isAvailable = YNQuestion("Is Available?");
      
      ac.Manufacturer = ReplaceIfNotEmpty(manufacturer, ac.Manufacturer);
      ac.Type = ReplaceIfNotEmpty(type, ac.Type);
      ac.Registration = ReplaceIfNotEmpty(registration, ac.Registration);
      ac.Airline = ReplaceIfNotEmpty(airline, ac.Airline);
      ac.Airline = ac.Airline == "0" ? "" : ac.Airline;
      ac.EconomySeats = ReplaceIfNotNegative(economySeats, ac.EconomySeats);
      ac.BusinessSeats = ReplaceIfNotNegative(businessSeats, ac.BusinessSeats);
      ac.IsAvailable = isAvailable;

    }
    
    void ModifyInput(String input)
    {
      if (input == "1")
      {
        AddAircraft();
      }
      else if (input == "2")
      {
        string registration = AskPersistently("Registration: ").ToUpper();
        try
        {
          ref Aircraft aircraft = ref fleet.FindWhere(ac => ac.Registration == registration);
          ModifyAircraft(ref aircraft);
        }
        catch (Exception e)
        {
          Console.WriteLine("Aircraft with registration {0} not found", registration);
        }
      }
      else if (input == "3")
      {
        string registration = AskPersistently("Registration: ").ToUpper();
        fleet.RemoveIf(ac => ac.Registration == registration);
      }
    }
    
    void ModifyMenu()
    {
      while (true)
      {
        Console.WriteLine("[1] Add Aircraft [2] Modify Aircraft [3] Remove Aircraft [4] Back");
        while (true)
        {
          PrintPrompt();
          var input = Console.ReadLine();
          if (input == null || input == "4")
          {
            return;
          }

          if (String.IsNullOrWhiteSpace(input))
          {
            continue;
          }
          
          ModifyInput(input);
          break;
        }
      }
    }

    string AskForFilename()
    {
      string file = null;
      
      while (String.IsNullOrWhiteSpace(file))
      {
        Console.Write("Filename: ");
        Console.Out.Flush();
        file = Console.ReadLine();
      }

      return file;
    }
    
    void StoreMenu()
    {

      string file = "";
      
      if (String.IsNullOrWhiteSpace(filename))
      {
        file = AskForFilename();
      }
      else
      {
        if (YNQuestion(String.Format("Store to currently loaded file? ({0})", filename)))
        {
          file = filename;
        } 
        else
        {
          file = AskForFilename();
        }
      }
      Store(file);
    }
    
    void HandleInput(String input)
    {
      if (input == "1")
      {
        PrintFleet();
      }
      else if (input == "2")
      {
        ModifyMenu();
      }
      else if (input == "3")
      {
       LoadFleet(GetPath()); 
      }
      else if (input == "4")
      {
        StoreMenu();
      }
    }
    void Menu()
    {
      while (true)
      {
        Console.WriteLine("[1] Print Fleet [2] Modify Fleet [3] Load File [4] Save To File [5] Exit");
        while (true)
        {
          PrintPrompt();
          var input = Console.ReadLine();
          if (input == null || input == "5")
          {
            return;
          }

          if (String.IsNullOrWhiteSpace(input))
          {
            continue;
          }
          
          HandleInput(input);
          break;
          
        }
      }
    }

    public void Run()
    {
      Init();
      Menu();
    }
    
  }
}
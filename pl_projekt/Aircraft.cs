using System;
using System.Text;
using System.Xml.Linq;

namespace pl_projekt
{
  public struct Aircraft : Serializable, IComparable
  {
    public string Manufacturer;
    public string Type;
    public string Registration;
    public string? Airline;
    public int EconomySeats;
    public int BusinessSeats;
    public bool IsAvailable;

    public Aircraft(string manufacturer, string type, string registration, string? airline, int economySeats,
      int businessSeats, bool isAvailable)
    {
      Manufacturer = manufacturer;
      Type = type;
      Registration = registration.ToUpper();
      Airline = airline;
      EconomySeats = economySeats;
      BusinessSeats = businessSeats;
      IsAvailable = isAvailable;
    }
    
    public Aircraft(XElement xml)
    {
      Manufacturer = xml.Element("manufacturer").Value;
      Type = xml.Element("type").Value;
      Registration = xml.Element("registration").Value.ToUpper();
      Airline = (xml.Element("airline") != null && !xml.Element("airline").IsEmpty) ? xml.Element("airline").Value : null;
      EconomySeats = int.Parse(xml.Element("economySeats").Value);
      BusinessSeats = int.Parse(xml.Element("businessSeats").Value);
      IsAvailable = xml.Element("isAvailable").Value == "true";
    }

    public override string ToString()
    {
      String pattern = "{0} {1} {2} ({3}), economy seats: {4}, business seats: {5}, currently {6}";
      String owner = Airline != null ? Airline : "Private";
      String availability = IsAvailable ? "available" : "unavailable";
      return String.Format(pattern, owner, Manufacturer, Type, Registration, EconomySeats, BusinessSeats, availability);
    }

    public int CompareTo(object? obj)
    {
      if (obj == null || !(obj is Aircraft))
      {
        throw new ArgumentException("Invalid comparison. ");
      }

      var other = (Aircraft) obj;

      int byManufacturer = String.Compare(Manufacturer, other.Manufacturer);
      if (byManufacturer != 0) { return byManufacturer; }
      
      int byType = String.Compare(Type, other.Type);
      if (byType != 0) { return byType; }

      if (IsAvailable && !other.IsAvailable) { return -1; }
      if (!IsAvailable && other.IsAvailable) { return 1; }
      
      return String.Compare(Registration, other.Registration);
    }

    static public bool operator<(Aircraft left, Aircraft right)
    {
      return left.CompareTo(right) > 0;
    }
    
    static public bool operator>(Aircraft left, Aircraft right)
    {
      return left.CompareTo(right) < 0;
    }

    public string ToXml()
    {

      String pattern = "<{0}>{1}</{0}>";
      StringBuilder sb = new StringBuilder();

      sb.Append("<aircraft>");
      
      sb.AppendFormat(pattern, "manufacturer", Manufacturer);
      sb.AppendFormat(pattern, "type", Type);
      sb.AppendFormat(pattern, "registration", Registration);
      if (Airline != null) { sb.AppendFormat(pattern, "airline", Airline); }
      sb.AppendFormat(pattern, "economySeats", EconomySeats);
      sb.AppendFormat(pattern, "businessSeats", BusinessSeats);
      sb.AppendFormat(pattern, "isAvailable", IsAvailable ? "true" : "false");

      sb.Append("</aircraft>");
      
      return sb.ToString();
    }
  }
}
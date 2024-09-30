namespace JobiJoba;

public class Ad{
    public int Id {get; set;}

    public string? JobiJoba_Ad_Id {get; set;}

    public string? Title {get; set;}

    public string? Description {get; set;}

    public string? City {get; set;}

    public List<string>? ContractTypes {get; set;}

    public string? Company {get; set;} 

    public DateTime PublicationDate { get; set;}
}
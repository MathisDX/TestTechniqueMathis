using System.Text.Json;
using JobiJoba;

public class ConversionToAdService{
    public Ad Convert(JsonElement input){

        Ad ad = new Ad{
            JobiJoba_Ad_Id = input.GetProperty("id").ToString(),
            Title = input.GetProperty("title").ToString(),
            Description = input.GetProperty("description").ToString(),
            City = input.GetProperty("city").ToString(),
            ContractTypes = input.GetProperty("contractType").EnumerateArray().Select(item => item.ToString()).ToList(),
            Company = input.GetProperty("company").ToString(),
            PublicationDate = DateTime.Parse(input.GetProperty("publicationDate").ToString())
        };
        return ad;
    }
}
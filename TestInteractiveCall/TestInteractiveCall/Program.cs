using issues_collection;
using System.ComponentModel;


// when you have an enum for errors (declared below)
// to send the declaration to the user you have to

// create an istance of an IssueCOllector with your enum as a generic type
IssueCollector<MyErrors> collector = new IssueCollector<MyErrors>();

// send the request
collector.SendRequest();

// and get the request back (as an async task)
Task<MyErrors> error = collector.GetCode();

Console.WriteLine("waiting for the responce...");

Console.WriteLine("The error is: " + await error);

// WARNING:
// many things can go wrong douring this process:
//  - the http request fails
//  - the user dose not reply
//  - the user insert an invalid code
//  - ...
//
// so be carefull to catch all the possible errors!

public enum MyErrors
{
    Errore = 0,
    Attrezzagio = 1,
    Lavaggio = 4,
    Pausa = 50,
    // if one of the states is made up of multiples words is raccomended to use a description
    // remember to include: System.ComponentModel;
    [Description("Incastro Macchina")]
    IncastroMacchina
}

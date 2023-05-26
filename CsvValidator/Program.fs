// For more information see https://aka.ms/fsharp-console-apps
//printfn "Hello from F#"

open System.Text.RegularExpressions
open FSharp.Data

// Source: https://learn.microsoft.com/en-us/dotnet/fsharp/language-reference/active-patterns
// ParseRegex parses a regular expression and returns a list of the strings that match each group in
// the regular expression.
// List.tail is called to eliminate the first element in the list, which is the full matched expression,
// since only the matches for each group are wanted.
let (|ParseRegex|_|) regex str =
   let m = Regex(regex).Match(str)
   if m.Success
   then Some (List.tail [ for x in m.Groups -> x.Value ])
   else None

let (|Integer|_|) (str: string) =
    let mutable intvalue = 0
    if System.Int32.TryParse(str, &intvalue) then Some(intvalue)
    else None

let (|Float|_|) (str: string) =
   let mutable floatvalue = 0.0
   if System.Double.TryParse(str, &floatvalue) then Some(floatvalue)
   else None

// Source: https://learn.microsoft.com/en-us/dotnet/fsharp/language-reference/active-patterns
// With own modifications
let parseDate str =
   match str with
   | ParseRegex "(\d{1,4})-(\d{1,2})-(\d{1,2}T([0-2][0-3]):([0-5][0-9]):([0-9][0-9][0-9]))" [Integer y; Integer m; Integer d; Integer hh; Integer mm; Integer ss]
          -> new System.DateTime(y, m, d)
   | _ -> new System.DateTime()


let parseNumeric str =
   match str with
   | Integer i -> printfn "%d : Integer" i
   | Float f -> printfn "%f : Floating point" f
   | _ -> printfn "%s : Not matched." str

let headers =
    [|
        "Departure"
        "Return"
        "Departure station id"
        "Departure station name"
        "Return station id"
        "Return station name"
        "Covered distance (m)"
        "Duration (sec.)"
    |]

let csvRows =
    CsvFile
        .Load(__SOURCE_DIRECTORY__ + "/../output.csv")
        .Cache()

for row in csvRows.Rows |> Seq.truncate 10 do
    printfn ("%s, %s, %s, %s, %s, %s, %s, %s") (row.GetColumn "Departure") (row.GetColumn "Return") (row.GetColumn "Departure station id") (row.GetColumn "Departure station name") (row.GetColumn "Return station id") (row.GetColumn "Return station name") (row.GetColumn "Covered distance (m)") (row.GetColumn "Duration (sec.)")

    parseNumeric (row.GetColumn "Departure station id")
    parseNumeric (row.GetColumn "Return station id")
    parseNumeric (row.GetColumn "Covered distance (m)")
    parseNumeric (row.GetColumn "Duration (sec.)")

    
    
    //printfn "%s" (row.GetColumn headers[4])
    //printfn "%A" row.Columns
    //printfn "ROW: (%s, %s, %s)" (row.GetColumn "High") (row.GetColumn "Low") (row.GetColumn "Date")

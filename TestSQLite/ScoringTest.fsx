#r @"C:\Users\PatrickB\Source\Repos\FSharp.Stats\bin\FSharp.Stats.dll"

open FSharp.Stats

type POI =
    {
     Name          : int
     RetentionTime : float
     Score         : float
    }

let createPOI name rt s =
    {
     POI.Name          = name
     POI.RetentionTime = rt
     POI.Score         = s
    }

let rtList    = [|12.; 12.3; 12.2; 12.6; 12.5|]
let scoreList = [|10.; 11.; 9.; 8.; 7.5|]
let nameList  = [|1..5|]

let poiList =
    Array.map3 (fun x y z -> createPOI x y z) nameList rtList scoreList

let calculateBestScore (inputList : POI array) =
    let averagePOIRT = Array.average (Array.map (fun x -> x.RetentionTime) inputList)
    let sdPOIRT      = 
        let rec loop acc i =
            if i = inputList.Length-1 then sqrt(acc / float inputList.Length)
            else loop (acc + (inputList.[i].RetentionTime - averagePOIRT) * (inputList.[i].RetentionTime - averagePOIRT)) (i + 1)
        loop 0. 0
    sdPOIRT

calculateBestScore poiList
let averagePOIRT = Array.average (Array.map (fun x -> x.RetentionTime) poiList)
let medianPOIRT  = Array.median  (Array.map (fun x -> x.RetentionTime) poiList)
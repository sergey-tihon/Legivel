﻿open System
open System.Text
open System.IO
open System.Text.RegularExpressions

#I "."
#r @"bin\Debug\YamlParser.dll"

open RegexDSL
open YamlParse


//  Tests
let engine = Yaml12Parser()

let ``s-l+block-node`` s = 
    let ps = ParseState.Create s
    let ps = ps.SetIndent -1
    let ps = ps.SetSubIndent 0
    let ps = ps.SetStyleContext ``Block-in``
    let d = engine.``s-l+block-node`` ps 
    d


let ``s-l+block-collection`` s = 
    let ps = ParseState.Create s
    let ps = ps.SetIndent 0
    let ps = ps.SetSubIndent 0
    let ps = ps.SetStyleContext ``Block-in``
    let d = engine.``s-l+block-collection`` ps 
    d


#load "nlog.fsx"
NlogInit.With __SOURCE_DIRECTORY__ __SOURCE_FILE__

let YamlParse s =
    try
        let pr = (``s-l+block-node`` s).Value 
        let tr = pr |> snd
        let short (s:string) = if s.Length > 10 then s.Substring(0, 10) else s
        tr.TraceSuccess  |> List.iter(fun (s,ps) -> printfn "%s\t\"%s\"" s (short (ps.InputString.Replace("\n","\\n"))))
        let rs = pr |> fst
        printfn "%s" (rs.ToCanonical(0))
    with
    | e -> printfn "%A" e


//``s-l+block-collection`` "\n# Statistics:\n  hr:  # Home runs\n     65\n  avg: # Average\n   0.278"

YamlParse "a:\n key: value\n"

YamlParse "plain key: in-line value\n: # Both empty\n\"quoted key\":- entry"

//YamlParse ":"

YamlParse "{ first: Sammy, last: Sosa }:\n# Statistics:\n  hr:  # Home runs\n     65\n  avg: # Average\n   0.278"

YamlParse "  hr:  # Home runs\n     65\n  avg: # Average\n   0.278"

YamlParse "\n# Statistics:\n  hr:  # Home runs\n     65\n  avg: # Average\n   0.278"


YamlParse "block sequence:\n  - one\n  - two : three\n"



let ``ns-l-block-map-implicit-entry`` s =
    let ps = ParseState.Create s
    let ps = ps.SetIndent 1
    let ps = ps.SetSubIndent 0
    let ps = ps.SetStyleContext ``Block-key``
    let d = engine.``ns-l-block-map-implicit-entry`` ps 
    d

let KeyParse s =
    try
        let pr = (``ns-l-block-map-implicit-entry`` s).Value 
        let tr = pr |> fun (a,b,c) -> c
        let short (s:string) = if s.Length > 10 then s.Substring(0, 10) else s
        tr.TraceSuccess  |> List.iter(fun (s,ps) -> printfn "%s\t\"%s\"" s (short (ps.InputString.Replace("\n","\\n"))))
        let rs = pr |> fun (a,b,c) -> a
        printfn "%s" (rs.ToCanonical(0))
    with
    | e -> printfn "%A" e


KeyParse " key: value\n"

YamlParse " key: value\n"



﻿module TestStringScalars

open YamlParse
open RepresentationGraph
open NUnit.Framework
open System
open System.Diagnostics
open FsUnit
open TagResolution

open TestUtils


//  Double quote strings

[<Test>]
let ``Test Double Quoted Single Line - Simple``() =
    let yml = YamlParse "\"my simple string\"" 
    [yml] |> Some |> ToScalar |> should equal "my simple string"


//  Single Quoted strings
[<Test>]
let ``Test Single Quoted Single Line - Simple``() =
    let yml = YamlParse "'my simple string'" 
    [yml] |> Some |> ToScalar |> should equal "my simple string"

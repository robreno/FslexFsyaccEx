module FSharpAnatomy.Program 

open System
open System.IO

open FSharp.Idioms

open FSharp.Literals.Literal
open FslexFsyaccEx.Yacc


let [<EntryPoint>] main _ = 
    Console.WriteLine(stringify 0)
    0

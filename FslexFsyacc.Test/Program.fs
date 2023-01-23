module FslexFsyaccEx.Program 

open System
open System.IO

open FSharp.Literals.Literal
open FslexFsyaccEx.Yacc
open System.Collections.Generic

let x<'a when 'a : comparison > (z:'a) = ()

let [<EntryPoint>] main _ = 
    Console.WriteLine(stringify "")
    
    0

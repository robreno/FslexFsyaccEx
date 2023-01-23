module FslexFsyaccEx.Fsyacc.FsyaccCompiler

open FslexFsyaccEx.Runtime
open FslexFsyaccEx.Fsyacc
open FslexFsyaccEx.Fsyacc.FsyaccTokenUtils

let parser = Parser<int*int*FsyaccToken>(
    Fsyacc2ParseTable.rules,
    Fsyacc2ParseTable.actions,
    Fsyacc2ParseTable.closures,getTag,getLexeme)

let parse(tokens:seq<int*int*FsyaccToken>) =
    tokens
    |> parser.parse
    |> Fsyacc2ParseTable.unboxRoot

let compile (inp:string) =
    inp
    |> FsyaccTokenUtils.tokenize
    |> parse

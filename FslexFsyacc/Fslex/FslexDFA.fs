module FslexFsyaccEx.Fslex.FslexDFA
let nextStates = [0u,["%%",5u;"&",5u;"(",5u;")",3u;"*",3u;"+",3u;"/",5u;"=",5u;"?",3u;"CAP",5u;"HEADER",2u;"HOLE",3u;"ID",3u;"LITERAL",3u;"SEMANTIC",5u;"[",5u;"]",3u;"|",5u];1u,["%%",1u];2u,["%%",1u];3u,["(",4u;"HOLE",4u;"ID",4u;"LITERAL",4u;"[",4u]]
open FslexFsyaccEx.Runtime
open FslexFsyaccEx.Fslex
open FslexFsyaccEx.Fslex.FslexTokenUtils
type token = int*int*FslexToken
let rules:list<uint32 list*uint32 list*_> = [
    [1u],[],fun(lexbuf:token list)->
        [lexbuf.Head]
    [4u],[3u],fun(lexbuf:token list)->
        appendAMP lexbuf
    [2u;3u;5u],[],fun(lexbuf:token list)->
        lexbuf
]
let analyzer = Analyzer(nextStates, rules)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)
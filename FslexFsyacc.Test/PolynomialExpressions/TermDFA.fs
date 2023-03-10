module PolynomialExpressions.TermDFA
let nextStates = [0u,["+",1u;"-",1u;"ID",3u;"INT",2u];1u,["ID",3u;"INT",2u];2u,["ID",3u];3u,["**",4u];4u,["INT",5u]]
open FslexFsyaccEx.Runtime
open PolynomialExpressions.Tokenizer
type token = int*int*Token
let rules:list<uint32 list*uint32 list*_> = [
    [2u],[],fun(lexbuf:token list)->
        // multiline test
        toConst lexbuf
    [3u;5u],[],fun(lexbuf:token list)->
        toTerm lexbuf
]
let analyzer = Analyzer(nextStates, rules)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)
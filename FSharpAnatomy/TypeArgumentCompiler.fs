module FSharpAnatomy.TypeArgumentCompiler

open FslexFsyaccEx.Runtime
open FSharp.Literals.Literal
let analyze (posTokens:seq<Position<FSharpToken>>) = 
    posTokens
    |> ArrayTypeSuffixDFA.analyze

let parser = Parser<Position<FSharpToken>> (
    TypeArgumentParseTable.rules,
    TypeArgumentParseTable.actions,
    TypeArgumentParseTable.closures,
    
    TypeArgumentUtils.getTag,
    TypeArgumentUtils.getLexeme)

let parse (tokens:seq<Position<FSharpToken>>) =
    tokens
    |> parser.parse
    |> TypeArgumentParseTable.unboxRoot

let compile (txt:string) =
    let tokenize(context:CompilerContext<FSharpToken>) =
        let i = CompilerContext.nextIndex context
        match
            txt.[i..]
            |> TypeArgumentUtils.tokenize i
            |> ArrayTypeSuffixDFA.analyze
            |> Seq.head
        with tok ->
            {
                context with
                    tokens = tok::context.tokens
            }
            
    let parse loop (context:CompilerContext<FSharpToken>) =
        match context.tokens.Head with
        | {value=EOF} ->
            let states = 
                match parser.tryReduce(context.states) with
                | Some nextStates -> nextStates
                | None -> context.states
            match states with
            |[(1,lxm);(0,null)] ->  
                lxm
                |> TypeArgumentParseTable.unboxRoot
            | _ -> failwith "不完整"
        | lookahead ->
            let states = 
                try
                match parser.tryReduce(context.states,lookahead) with
                | Some nextStates -> nextStates
                | None -> context.states
                with _ -> failwith $"{stringify context}"
            match states with
            |[(1,lxm);(0,null)] ->  
                lxm
                |> TypeArgumentParseTable.unboxRoot
            | _ ->
                loop {
                    context with
                        states = parser.shift(states,lookahead)
                }

    let rec loop (context:CompilerContext<FSharpToken>) =
        context
        |> tokenize
        |> parse loop

    loop {
        tokens = []
        states = [0,null]
    }

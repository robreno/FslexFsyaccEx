module FslexFsyaccEx.Fsyacc.FsyaccFileRender

open FslexFsyaccEx.Runtime.RenderUtils

open System
open System.Text.RegularExpressions

open FSharp.Idioms
open FSharp.Idioms.Line

let renderHeader (header:string) =
    if String.IsNullOrWhiteSpace(header) then
        "%{%}"
    else
        [
            "%{"
            header
            "%}"
        ]
        |> String.concat "\r\n"

let renderSymbol (sym:string) =
    if sym.[0] = '{' && sym.[sym.Length-1] = '}'
    then sym.[1..sym.Length-2]
    else sym |> FslexFsyaccEx.Runtime.RenderUtils.renderSymbol

let renderBody (body:string list) =
    if body.IsEmpty then
        "(*empty*)"
    else
        body
        |> List.map renderSymbol
        |> String.concat " "

let renderSemantic(semantic:string) =
    if Regex.IsMatch(semantic,@"[\r\n]") then
        [
            "{"
            indentCodeBlock 4 semantic
            "}"
        ]
        |> String.concat "\r\n"
    elif String.IsNullOrWhiteSpace(semantic) then
        "{}"
    else "{" + semantic + "}"

let renderComponent (body:string list, name:string, semantic:string) =
    [
        yield "|"
        yield renderBody body
        if name = "" then () else yield "%prec "+name
        yield renderSemantic semantic
    ]
    |> String.concat " "

let renderRhs(bodies:(string list*string*string)list) =
    bodies
    |> List.map renderComponent
    |> String.concat "\r\n"

let renderRule (lhs:string,rhs:(string list*string*string)list) =
    [
        renderSymbol lhs + " :"
        renderRhs rhs |> indentCodeBlock 4
    ]
    |> String.concat "\r\n"

let renderPrecedence (assoc:string, symbols:string list) =
    let symbols =
        symbols
        |> List.map renderSymbol
        |> String.concat " "
    "%" + $"{assoc} {symbols}"

let renderDec (symbol:string, typeD:string) =
    [
        symbol
        typeD
    ]
    |> List.map renderSymbol
    |> String.concat " : "

let renderFsyacc
    (header:string                                       )
    (rules:(string*((string list*string*string)list))list)
    (precedences:(string*string list)list                )
    (declarations:(string*string)list                    )
    =
    let h = renderHeader header
    let r =
        rules
        |> List.map renderRule
        |> String.concat "\r\n"

    let p() =
        precedences
        |> List.map renderPrecedence
        |> String.concat "\r\n"

    let d() =
        declarations
        |> List.map renderDec
        |> String.concat "\r\n"

    let main =
        [
            r
            if precedences.IsEmpty then () else p()
            if declarations.IsEmpty then () else d()
        ]
        |> String.concat "\r\n%%\r\n"

    [h;main]
    |>String.concat "\r\n"

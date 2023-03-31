[<RequireQualifiedAccess>]
module Vp.FSharp.Sql.PostgreSql.PostgreSqlArrayDbValue

open NpgsqlTypes
open Vp.FSharp.Sql.PostgreSql


let inline private array dbType v =
    (NpgsqlDbType.Array ||| dbType, v :> obj) |> Custom

let inline ofArray<'a> dbType (values: 'a []) =
    array dbType values

let inline ofList<'a> dbType (values: 'a list) =
    array dbType values

let inline ofSeq<'a> dbType (values: 'a seq) =
    array dbType values
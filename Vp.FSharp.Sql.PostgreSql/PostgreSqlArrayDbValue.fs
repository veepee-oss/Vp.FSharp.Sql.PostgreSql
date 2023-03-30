[<RequireQualifiedAccess>]
module Vp.FSharp.Sql.PostgreSql.PostgreSqlArrayDbValue

open NpgsqlTypes
open Vp.FSharp.Sql.PostgreSql


let toArray dbType =
    let tuple2 a b = (a, b)
    List.toArray
    >> (fun v -> v :> obj)
    >> tuple2 (NpgsqlDbType.Array ||| dbType)
    >> Custom

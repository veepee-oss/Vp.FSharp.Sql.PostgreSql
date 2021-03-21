[<RequireQualifiedAccess>]
module Vp.FSharp.Sql.PostgreSql.PostgreSqlNullDbValue

open Vp.FSharp.Sql


let ifNone toDbValue = NullDbValue.ifNone toDbValue PostgreSqlDbValue.Null
let ifError toDbValue = NullDbValue.ifError toDbValue (fun _ -> PostgreSqlDbValue.Null)

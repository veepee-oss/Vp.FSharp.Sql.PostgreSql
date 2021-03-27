[<RequireQualifiedAccess>]
module Vp.FSharp.Sql.PostgreSql.PostgreSqlNullDbValue

open Vp.FSharp.Sql


/// Return PostgreSQL DB Null value if the given option is None, otherwise the underlying wrapped in Some.
let ifNone toDbValue = NullDbValue.ifNone toDbValue PostgreSqlDbValue.Null

/// Return PostgreSQL DB Null value if the given option is Error, otherwise the underlying wrapped in Ok.
let ifError toDbValue = NullDbValue.ifError toDbValue (fun _ -> PostgreSqlDbValue.Null)

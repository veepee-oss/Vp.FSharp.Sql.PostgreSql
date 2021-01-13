namespace Vp.FSharp.Sql.PostgreSql

open System
open System.Net
open System.Collections.Generic
open System.Net.NetworkInformation

open Npgsql
open NpgsqlTypes

open Vp.FSharp.Sql


/// Native PostgreSQL DB types.
/// See https://www.npgsql.org/doc/types/basic.html
/// and https://stackoverflow.com/a/845472/4636721
type PostgreSqlDbValue =
    | Null
    | Bit of bool
    | Boolean of bool
    | SmallInt of int16
    | Integer of int32
    | Oid of uint32
    | Xid of uint32
    | Cid of uint32
    | BigInt of int64

    | Real of single
    | Double of double

    | Money of decimal
    | Numeric of decimal

    | ByteA of uint8 array
    | OidVector of uint32 array

    | Uuid of Guid

    | INet of IPAddress
    | MacAddr of PhysicalAddress

    | TsQuery of NpgsqlTsQuery
    | TsVector of NpgsqlTsVector

    | Point of NpgsqlPoint
    | LSeg of NpgsqlLSeg
    | Path of NpgsqlPath
    | Polygon of NpgsqlPolygon
    | Line of NpgsqlLine
    | Circle of NpgsqlCircle
    | Box of NpgsqlBox

    | HStore of Dictionary<string, string>

    | Date of DateTime
    | Interval of TimeSpan
    | Time of DateTime
    | TimeTz of DateTime
    | Timestamp of DateTime
    | TimestampTz of DateTimeOffset

    | InternalChar of uint8

    | Char of string
    | VarChar of string

    | Name of string
    | CiText of string
    | Text of string
    | Xml of string
    | Json of string
    | Jsonb of string

    /// Only if the relevant Npgsql mapping for the Enum has been set up beforehand.
    /// See: https://www.npgsql.org/doc/types/enums_and_composites.html
    | Enum of Enum

type PostgreSqlCommandDefinition =
    CommandDefinition<
        NpgsqlConnection,
        NpgsqlCommand,
        NpgsqlParameter,
        NpgsqlDataReader,
        NpgsqlTransaction,
        PostgreSqlDbValue>

type PostgreSqlConfiguration =
    SqlConfigurationCache<
        NpgsqlConnection,
        NpgsqlCommand>

type PostgreSqlDependencies =
    SqlDependencies<
        NpgsqlConnection,
        NpgsqlCommand,
        NpgsqlParameter,
        NpgsqlDataReader,
        NpgsqlTransaction,
        PostgreSqlDbValue>

[<RequireQualifiedAccess>]
module PostgreSqlNullDbValue =
    let ifNone toDbValue = NullDbValue.ifNone toDbValue PostgreSqlDbValue.Null
    let ifError toDbValue = NullDbValue.ifError toDbValue (fun _ -> PostgreSqlDbValue.Null)

[<RequireQualifiedAccess>]
module PostgreSqlCommand =

    let private dbValueToParameter name value =
        let parameter = NpgsqlParameter()
        parameter.ParameterName <- name
        match value with
        | Null ->
            parameter.Value <- DBNull.Value
        | Bit value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Bit
        | Boolean value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Boolean
        | SmallInt value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Smallint
        | Integer value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Integer
        | Oid value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Oid
        | Xid value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Xid
        | Cid value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Cid
        | BigInt value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Bigint

        | Real value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Bigint
        | Double value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Bigint

        | Money value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Money
        | Numeric value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Numeric

        | ByteA value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Bytea
        | OidVector value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Oidvector

        | Uuid value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Uuid

        | INet value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Inet
        | MacAddr value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.MacAddr

        | TsQuery value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.TsQuery
        | TsVector value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.TsVector

        | Point value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Point
        | LSeg value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.LSeg
        | Path value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Path
        | Polygon value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Polygon
        | Line value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Line
        | Circle value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Circle
        | Box value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Box

        | HStore value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Hstore

        | Date value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Date
        | Interval value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Interval
        | Time value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Time
        | TimeTz value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.TimeTz
        | Timestamp value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Timestamp
        | TimestampTz value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.TimestampTz

        | InternalChar value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.InternalChar

        | Char value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Char
        | VarChar value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Varchar

        | Name value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Name
        | CiText value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Citext
        | Text value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Text
        | Xml value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Xml
        | Json value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Json
        | Jsonb value ->
            parameter.Value <- value
            parameter.NpgsqlDbType <- NpgsqlDbType.Jsonb
        | Enum value ->
            parameter.Value <- value
        parameter

    let private deps: PostgreSqlDependencies =
        { CreateCommand = fun connection -> connection.CreateCommand()
          ExecuteReaderAsync = fun command -> command.ExecuteReaderAsync
          DbValueToParameter = dbValueToParameter }

    /// Initialize a command definition with the given text contained in the given string.
    let text value : PostgreSqlCommandDefinition =
        SqlCommand.text value

    /// Initialize a command definition with the given text spanning over several strings (ie. list).
    let textFromList value : PostgreSqlCommandDefinition =
        SqlCommand.textFromList value

    /// Update the command definition so that when executing the command, it doesn't use any logger.
    /// Be it the default one (Global, if any.) or a previously overriden one.
    let noLogger commandDefinition = { commandDefinition with Logger = LoggerKind.Nothing }

    /// Update the command definition so that when executing the command, it use the given overriding logger.
    /// instead of the default one, aka the Global logger, if any.
    let overrideLogger value commandDefinition = { commandDefinition with Logger = LoggerKind.Override value }

    /// Update the command definition with the given parameters.
    let parameters value (commandDefinition: PostgreSqlCommandDefinition) : PostgreSqlCommandDefinition =
        SqlCommand.parameters value commandDefinition

    /// Update the command definition with the given cancellation token.
    let cancellationToken value (commandDefinition: PostgreSqlCommandDefinition) : PostgreSqlCommandDefinition =
        SqlCommand.cancellationToken value commandDefinition

    /// Update the command definition with the given timeout.
    let timeout value (commandDefinition: PostgreSqlCommandDefinition) : PostgreSqlCommandDefinition =
        SqlCommand.timeout value commandDefinition

    /// Update the command definition and sets the command type (ie. how it should be interpreted).
    let commandType value (commandDefinition: PostgreSqlCommandDefinition) : PostgreSqlCommandDefinition =
        SqlCommand.commandType value commandDefinition

    /// Update the command definition and sets whether the command should be prepared or not.
    let prepare value (commandDefinition: PostgreSqlCommandDefinition) : PostgreSqlCommandDefinition =
        SqlCommand.prepare value commandDefinition

    /// Update the command definition and sets whether the command should be wrapped in the given transaction.
    let transaction value (commandDefinition: PostgreSqlCommandDefinition) : PostgreSqlCommandDefinition =
        SqlCommand.transaction value commandDefinition

    /// Return the sets of rows as an AsyncSeq accordingly to the command definition.
    let queryAsyncSeq connection read (commandDefinition: PostgreSqlCommandDefinition) =
        SqlCommand.queryAsyncSeq
            connection deps (PostgreSqlConfiguration.Snapshot) read commandDefinition

    /// Return the sets of rows as a list accordingly to the command definition.
    let queryList connection read (commandDefinition: PostgreSqlCommandDefinition) =
        SqlCommand.queryList
            connection deps (PostgreSqlConfiguration.Snapshot) read commandDefinition

    /// Return the first set of rows as a list accordingly to the command definition.
    let querySetList connection read (commandDefinition: PostgreSqlCommandDefinition) =
        SqlCommand.querySetList
            connection deps (PostgreSqlConfiguration.Snapshot) read commandDefinition

    /// Return the 2 first sets of rows as a tuple of 2 lists accordingly to the command definition.
    let querySetList2 connection read1 read2 (commandDefinition: PostgreSqlCommandDefinition) =
        SqlCommand.querySetList2
            connection deps (PostgreSqlConfiguration.Snapshot) read1 read2 commandDefinition

    /// Return the 3 first sets of rows as a tuple of 3 lists accordingly to the command definition.
    let querySetList3 connection read1 read2 read3 (commandDefinition: PostgreSqlCommandDefinition) =
        SqlCommand.querySetList3
            connection deps (PostgreSqlConfiguration.Snapshot) read1 read2 read3 commandDefinition

    /// Execute the command accordingly to its definition and,
    /// - return the first cell value, if it is available and of the given type.
    /// - throw an exception, otherwise.
    let executeScalar<'Scalar> connection (commandDefinition: PostgreSqlCommandDefinition) =
        SqlCommand.executeScalar<'Scalar, _, _, _, _, _, _, _, _, _>
            connection deps (PostgreSqlConfiguration.Snapshot) commandDefinition

    /// Execute the command accordingly to its definition and,
    /// - return Some, if the first cell is available and of the given type.
    /// - return None, if first cell is DbNull.
    /// - throw an exception, otherwise.
    let executeScalarOrNone<'Scalar> connection (commandDefinition: PostgreSqlCommandDefinition) =
        SqlCommand.executeScalarOrNone<'Scalar, _, _, _, _, _, _, _, _, _>
            connection deps (PostgreSqlConfiguration.Snapshot) commandDefinition

    /// Execute the command accordingly to its definition and, return the number of rows affected.
    let executeNonQuery connection (commandDefinition: PostgreSqlCommandDefinition) =
        SqlCommand.executeNonQuery
            connection deps (PostgreSqlConfiguration.Snapshot) commandDefinition

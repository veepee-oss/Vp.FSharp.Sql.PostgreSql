[<RequireQualifiedAccess>]
module Vp.FSharp.Sql.PostgreSql.PostgreSqlCommand

open Vp.FSharp.Sql


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
        connection (Constants.Deps) (PostgreSqlConfiguration.Snapshot) read commandDefinition

/// Return the sets of rows as a list accordingly to the command definition.
let queryList connection read (commandDefinition: PostgreSqlCommandDefinition) =
    SqlCommand.queryList
        connection (Constants.Deps) (PostgreSqlConfiguration.Snapshot) read commandDefinition

/// Return the first set of rows as a list accordingly to the command definition.
let querySetList connection read (commandDefinition: PostgreSqlCommandDefinition) =
    SqlCommand.querySetList
        connection (Constants.Deps) (PostgreSqlConfiguration.Snapshot) read commandDefinition

/// Return the 2 first sets of rows as a tuple of 2 lists accordingly to the command definition.
let querySetList2 connection read1 read2 (commandDefinition: PostgreSqlCommandDefinition) =
    SqlCommand.querySetList2
        connection (Constants.Deps) (PostgreSqlConfiguration.Snapshot) read1 read2 commandDefinition

/// Return the 3 first sets of rows as a tuple of 3 lists accordingly to the command definition.
let querySetList3 connection read1 read2 read3 (commandDefinition: PostgreSqlCommandDefinition) =
    SqlCommand.querySetList3
        connection (Constants.Deps) (PostgreSqlConfiguration.Snapshot) read1 read2 read3 commandDefinition

/// Execute the command accordingly to its definition and,
/// - return the first cell value, if it is available and of the given type.
/// - throw an exception, otherwise.
let executeScalar<'Scalar> connection (commandDefinition: PostgreSqlCommandDefinition) =
    SqlCommand.executeScalar<'Scalar, _, _, _, _, _, _, _, _>
        connection (Constants.Deps) (PostgreSqlConfiguration.Snapshot) commandDefinition

/// Execute the command accordingly to its definition and,
/// - return Some, if the first cell is available and of the given type.
/// - return None, if first cell is DbNull.
/// - throw an exception, otherwise.
let executeScalarOrNone<'Scalar> connection (commandDefinition: PostgreSqlCommandDefinition) =
    SqlCommand.executeScalarOrNone<'Scalar, _, _, _, _, _, _, _, _>
        connection (Constants.Deps) (PostgreSqlConfiguration.Snapshot) commandDefinition

/// Execute the command accordingly to its definition and, return the number of rows affected.
let executeNonQuery connection (commandDefinition: PostgreSqlCommandDefinition) =
    SqlCommand.executeNonQuery
        connection (Constants.Deps) (PostgreSqlConfiguration.Snapshot) commandDefinition

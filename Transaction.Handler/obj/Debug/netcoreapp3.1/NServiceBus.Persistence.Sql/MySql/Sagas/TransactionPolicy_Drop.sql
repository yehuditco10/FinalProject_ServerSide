
/* TableNameVariable */

set @tableNameQuoted = concat('`', @tablePrefix, 'TransactionPolicy`');
set @tableNameNonQuoted = concat(@tablePrefix, 'TransactionPolicy');


/* DropTable */

set @dropTable = concat('drop table if exists ', @tableNameQuoted);
prepare script from @dropTable;
execute script;
deallocate prepare script;

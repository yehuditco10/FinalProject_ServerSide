
/* TableNameVariable */

/* Initialize */

declare
  sqlStatement varchar2(500);
  dataType varchar2(30);
  n number(10);
  currentSchema varchar2(500);
begin
  select sys_context('USERENV','CURRENT_SCHEMA') into currentSchema from dual;


/* CreateTable */

  select count(*) into n from user_tables where table_name = 'TRANSACTIONPOLICY';
  if(n = 0)
  then

    sqlStatement :=
       'create table "TRANSACTIONPOLICY"
       (
          id varchar2(38) not null,
          metadata clob not null,
          data clob not null,
          persistenceversion varchar2(23) not null,
          sagatypeversion varchar2(23) not null,
          concurrency number(9) not null,
          constraint "TRANSACTIONPOLICY_PK" primary key
          (
            id
          )
          enable
        )';

    execute immediate sqlStatement;

  end if;

/* AddProperty TransactionId */

select count(*) into n from all_tab_columns where table_name = 'TRANSACTIONPOLICY' and column_name = 'CORR_TRANSACTIONID' and owner = currentSchema;
if(n = 0)
then
  sqlStatement := 'alter table "TRANSACTIONPOLICY" add ( CORR_TRANSACTIONID VARCHAR2(38) )';

  execute immediate sqlStatement;
end if;

/* VerifyColumnType Guid */

select data_type ||
  case when char_length > 0 then
    '(' || char_length || ')'
  else
    case when data_precision is not null then
      '(' || data_precision ||
        case when data_scale is not null and data_scale > 0 then
          ',' || data_scale
        end || ')'
    end
  end into dataType
from all_tab_columns
where table_name = 'TRANSACTIONPOLICY' and column_name = 'CORR_TRANSACTIONID' and owner = currentSchema;

if(dataType <> 'VARCHAR2(38)')
then
  raise_application_error(-20000, 'Incorrect data type for Correlation_CORR_TRANSACTIONID.  Expected "VARCHAR2(38)" got "' || dataType || '".');
end if;

/* WriteCreateIndex TransactionId */

select count(*) into n from user_indexes where table_name = 'TRANSACTIONPOLICY' and index_name = 'SAGAIDX_1905A9F980E1732F4E4B00';
if(n = 0)
then
  sqlStatement := 'create unique index "SAGAIDX_1905A9F980E1732F4E4B00" on "TRANSACTIONPOLICY" (CORR_TRANSACTIONID ASC)';

  execute immediate sqlStatement;
end if;

/* PurgeObsoleteIndex */

/* PurgeObsoleteProperties */

select count(*) into n
from all_tab_columns
where table_name = 'TRANSACTIONPOLICY' and column_name like 'CORR_%' and
        column_name <> 'CORR_TRANSACTIONID' and owner = currentSchema;

if(n > 0)
then

  select 'alter table "TRANSACTIONPOLICY" drop column ' || column_name into sqlStatement
  from all_tab_columns
  where table_name = 'TRANSACTIONPOLICY' and column_name like 'CORR_%' and
        column_name <> 'CORR_TRANSACTIONID' and owner = currentSchema;

  execute immediate sqlStatement;

end if;

/* CompleteSagaScript */
end;

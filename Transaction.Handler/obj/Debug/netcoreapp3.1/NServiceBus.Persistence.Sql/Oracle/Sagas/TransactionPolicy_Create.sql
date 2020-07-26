
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

/* PurgeObsoleteIndex */

/* PurgeObsoleteProperties */

select count(*) into n
from all_tab_columns
where table_name = 'TRANSACTIONPOLICY' and column_name like 'CORR_%' and owner = currentSchema;

if(n > 0)
then

  select 'alter table "TRANSACTIONPOLICY" drop column ' || column_name into sqlStatement
  from all_tab_columns
  where table_name = 'TRANSACTIONPOLICY' and column_name like 'CORR_%' and owner = currentSchema;

  execute immediate sqlStatement;

end if;

/* CompleteSagaScript */
end;

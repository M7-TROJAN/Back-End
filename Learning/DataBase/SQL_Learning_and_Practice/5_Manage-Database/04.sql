BACKUP DATABASE CRMDB
TO DISK = 'C:\USERS\YOUYA\DESKTOP\BACKUPS\CRMDB.BAK' 
WITH NOINIT,
NAME = 'BACKUP_02',
DESCRIPTION = 'second backup'

RESTORE HEADERONLY FROM DISK = 'C:\USERS\YOUYA\DESKTOP\BACKUPS\CRMDB.BAK';

BACKUP LOG CRMDB 
TO DISK = 'C:\USERS\YOUYA\DESKTOP\BACKUPS\CRMDB_log.BAK'
WITH NOINIT,
NAME = 'BACKUP_LOG_01',
DESCRIPTION = 'first log backup'

UPDATE CUSTOMERS SET Telephone = '999-777-8888' WHERE Id = 101;

SELECT * FROM CUSTOMERS;

BACKUP LOG CRMDB 
TO DISK = 'C:\USERS\YOUYA\DESKTOP\BACKUPS\CRMDB_log.BAK'
WITH NOINIT,
NAME = 'BACKUP_LOG_02',
DESCRIPTION = 'second log backup'

UPDATE CUSTOMERS SET Telephone = '000-000-0000' WHERE Id = 101;

SELECT * FROM CUSTOMERS;

BACKUP LOG CRMDB 
TO DISK = 'C:\USERS\YOUYA\DESKTOP\BACKUPS\CRMDB_log.BAK'
WITH NOINIT,
NAME = 'BACKUP_LOG_03',
DESCRIPTION = 'third log backup'


RESTORE HEADERONLY FROM DISK = 'C:\USERS\YOUYA\DESKTOP\BACKUPS\CRMDB_log.BAK';
RESTORE FILELISTONLY FROM DISK = 'C:\USERS\YOUYA\DESKTOP\BACKUPS\CRMDB_log.BAK';


CREATE DATABASE [CRMDB_TEST]
ON PRIMARY
(
	NAME = 'CRMDB_TEST',
	FILENAME = 'C:\USERS\YOUYA\DESKTOP\DATA\CRMDB_TEST.mdf'
)
LOG ON
(
	NAME = 'CRMDB_TEST_LOG',
	FILENAME = 'C:\USERS\YOUYA\DESKTOP\DATA\CRMDB_TEST_LOG.ldf'
);
GO

RESTORE DATABASE CRMDB_TEST
FROM DISK = 'C:\USERS\YOUYA\DESKTOP\BACKUPS\CRMDB.BAK'
WITH REPLACE,
MOVE 'CRMDB' TO 'C:\USERS\YOUYA\DESKTOP\DATA\CRMDB_TEST.mdf',
MOVE 'CRMDB_LOG' TO 'C:\USERS\YOUYA\DESKTOP\DATA\CRMDB_TEST_LOG.ldf',
NORECOVERY;

RESTORE LOG CRMDB_TEST FROM DISK = 'C:\USERS\YOUYA\DESKTOP\BACKUPS\CRMDB_log.BAK'
WITH FILE = 1,
NORECOVERY


RESTORE LOG CRMDB_TEST FROM DISK = 'C:\USERS\YOUYA\DESKTOP\BACKUPS\CRMDB_log.BAK'
WITH FILE = 2,
NORECOVERY


RESTORE DATABASE CRMDB_TEST WITH RECOVERY;


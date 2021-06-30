INSERT INTO [Author]([FirstName], [LastName], [Birthday])
VALUES( 'Ivan', 'Ivanov', '1990-02-03' )

INSERT INTO [Author]([FirstName], [LastName], [Birthday])
VALUES( 'Adriel', 'Perez', '1990-02-03' ), ( 'Eric', 'Miller', '1998-13-11' )

INSERT INTO [Post]([Title], [Body], [AuthorId])
VALUES('SQL Introduction', 'Some text about SQL', 1)

INSERT INTO [Post]([Title], [Body], [AuthorId])
VALUES('SELECT Syntax', 'Some text about SELECT Syntax', 1)

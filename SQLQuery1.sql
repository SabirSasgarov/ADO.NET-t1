CREATE DATABASE Students

USE Students

CREATE TABLE StudentsTB
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(20) NOT NULL,
	Age INT CHECK(Age>0)
)

INSERT INTO StudentsTB VALUES
('Sabir',22),
('Xatire',25),
('MirHuseyn',24),
('Elcin',21),
('Ferid',18),
('Miryam',16);

SELECT * FROM StudentsTB

SELECT * FROM StudentsTB WHERE Name LIKE '%s%'

SELECT * FROM StudentsTB ORDER BY Id OFFSET 2 ROWS FETCH NEXT 10 ROWS ONLY;


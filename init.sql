CREATE TABLE pessoas(
	id UUID PRIMARY KEY,
	apelido VARCHAR(32) UNIQUE NOT NULL,
	nome VARCHAR(100) NOT NULL,
	nascimento VARCHAR(10) NOT NULL,
	stack VARCHAR(32)[]
);
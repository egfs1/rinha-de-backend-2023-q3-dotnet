CREATE EXTENSION pg_trgm;

CREATE TABLE pessoas(
	id UUID PRIMARY KEY NOT NULL,
	apelido VARCHAR(32) UNIQUE NOT NULL,
	nome VARCHAR(100) NOT NULL,
	nascimento VARCHAR(10) NOT NULL,
	stack VARCHAR(32)[] NULL,
	search TEXT NOT NULL
);

CREATE INDEX apelido_index_idx ON pessoas USING gin (apelido gin_trgm_ops);

CREATE INDEX search_index_idx ON pessoas USING gin (search gin_trgm_ops);
CREATE EXTENSION pg_trgm;

CREATE OR REPLACE FUNCTION concatenate_names(apelido VARCHAR, nome VARCHAR, stack VARCHAR[])
RETURNS TEXT AS $$
BEGIN
    RETURN apelido || ' ' || nome || ' ' || COALESCE(ARRAY_TO_STRING(stack, ' '), '');
END;
$$ LANGUAGE plpgsql IMMUTABLE;

CREATE TABLE pessoas(
	id UUID PRIMARY KEY NOT NULL,
	apelido VARCHAR(32) UNIQUE NOT NULL,
	nome VARCHAR(100) NOT NULL,
	nascimento VARCHAR(10) NOT NULL,
	stack VARCHAR(32)[] NULL,
	search TEXT GENERATED ALWAYS AS (concatenate_names(apelido, nome, stack)) STORED
);
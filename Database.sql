DROP TABLE IF EXISTS pazienti;
CREATE TABLE pazienti(
    idp TEXT NOT NULL,
    nome TEXT NOT NULL,
    cognome TEXT NOT NULL,
    codiceFiscale TEXT NOT NULL,
    indirizzo TEXT NOT NULL,
    PRIMARY KEY(idp)
);

DROP TABLE IF EXISTS tipoPersonale;
CREATE TABLE tipoPersonale(
    idt TEXT NOT NULL,
    tipo TEXT NOT NULL,
    PRIMARY KEY (idt)
);

DROP TABLE IF EXISTS personale;
CREATE TABLE personale(
    idpe TEXT NOT NULL,
    idt TEXT NOT NULL,
    nome TEXT NOT NULL,
    cognome TEXT NOT NULL,
    codiceFiscale TEXT NOT NULL,
    PRIMARY KEY (idpe),
    FOREIGN KEY (idt) REFERENCES tipoPersonale(idt)
);

DROP TABLE IF EXISTS operatori;
CREATE TABLE operatori(
    idop TEXT NOT NULL,
    idpe TEXT NOT NULL,
    PRIMARY KEY (idop),
    FOREIGN KEY (idpe) REFERENCES personale(idpe)
);

DROP TABLE IF EXISTS mediciPazienti;
CREATE TABLE mediciPazienti(
    idp TEXT NOT NULL,
    idpe TEXT NOT NULL,   
    FOREIGN KEY (idp) REFERENCES pazienti(idp),
    FOREIGN KEY (idpe) REFERENCES personale(idpe)
);

DROP TABLE IF EXISTS vaccini;
CREATE TABLE vaccini(
    idv TEXT NOT NULL,
    tipo TEXT NOT NULL,
    PRIMARY KEY (idv)
);

DROP TABLE IF EXISTS vacciniPazienti;
CREATE TABLE vacciniPazienti(
    idv TEXT NOT NULL,
    idp TEXT NOT NULL,
    data TEXT NOT NULL,
    FOREIGN KEY (idv) REFERENCES vaccini(idv),
     FOREIGN KEY (idp) REFERENCES pazienti(idp)
);

DROP TABLE IF EXISTS strutture;
CREATE TABLE strutture(
    ids TEXT NOT NULL,
    indirizzo TEXT NOT NULL,
    massimali INTEGER NOT NULL,
    email TEXT NOT NULL,
    PRIMARY KEY (ids)
);

DROP TABLE IF EXISTS turni;
CREATE TABLE turni(
    idtu TEXT NOT NULL,
    idop TEXT NOT NULL,
    ids TEXT NOT NULL,
    data TEXT NOT NULL,
    ora TEXT NOT NULL,
    PRIMARY KEY (idtu),
    FOREIGN KEY (ids) REFERENCES strutture(ids),
    FOREIGN KEY (idop) REFERENCES operatori(idop)
);

DROP TABLE IF EXISTS orelavorate;
CREATE TABLE orelavorate(
    idop TEXT NOT NULL,
    ids TEXT NOT NULL,
    data TEXT NOT NULL,
    ora TEXT NOT NULL,
    FOREIGN KEY (idop) REFERENCES operatori(idop),
    FOREIGN KEY (ids) REFERENCES strutture(ids)
);

DROP TABLE IF EXISTS spedizioni;
CREATE TABLE spedizioni(
    ids TEXT NOT NULL,
    datap TEXT NOT NULL,
    datac TEXT NOT NULL,
    quantita INTEGER NOT NULL,
    FOREIGN KEY (ids) REFERENCES strutture(ids)
);

DROP TABLE IF EXISTS scorte;
CREATE TABLE scorte(
    ids TEXT NOT NULL,
    quantita INTEGER NOT NULL,
    FOREIGN KEY (ids) REFERENCES strutture(ids)
);

DROP TABLE IF EXISTS vaccinoCovid;
CREATE TABLE vaccinoCovid(
    idp TEXT NOT NULL,
    idop TEXT NOT NULL,
    ids TEXT NOT NULL,
    data  TEXT NOT NULL,
    ora TEXT NOT NULL,
    FOREIGN KEY (idp) REFERENCES pazienti(idp),
    FOREIGN KEY (idop) REFERENCES operatori(idop),
    FOREIGN KEY (ids) REFERENCES strutture(ids)
);

DROP TABLE IF EXISTS prenotazioniCovid;
CREATE TABLE prenotazioniCovid(
    idpr TEXT NOT NULL,
    idp TEXT NOT NULL,
    data TEXT NOT NULL,
    ora TEXT NOT NULL,
    PRIMARY KEY (idpr),
    FOREIGN KEY (idp) REFERENCES pazienti(idp)
);

DROP TABLE IF EXISTS effettiCollaterali;
CREATE TABLE effettiCollaterali(
    idp TEXT NOT NULL,
    data  TEXT NOT NULL,
    descrizione TEXT NOT NULL,
    FOREIGN KEY (idp) REFERENCES pazienti(idp)
);
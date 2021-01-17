CREATE TABLE pazienti (
  idp TEXT PRIMARY KEY NOT NULL,
  idpe TEXT NOT NULL,
  nome TEXT NOT NULL,
  cognome TEXT NOT NULL,
  codiceFiscale TEXT NOT NULL,
  datadinascita TEXT NOT NULL,
  luogodinascita TEXT NOT NULL,
  indirizzo TEXT NOT NULL,
  comune TEXT NOT NULL,
  provincia TEXT NOT NULL,
  regione TEXT NOT NULL,
  nazione TEXT NOT NULL,
  telefono TEXT NOT NULL,
  cellulare TEXT NOT NULL,
  email TEXT NOT NULL
);

CREATE TABLE studiomedico (
  idst TEXT PRIMARY KEY NOT NULL,
  telefono TEXT NOT NULL,
  email TEXT NOT NULL,
  orari TEXT NOT NULL,
  indirizzo TEXT NOT NULL
);

CREATE TABLE studioPersonale (
  idst TEXT NOT NULL,
  idpe TEXT NOT NULL
);

CREATE TABLE personale (
  idpe TEXT PRIMARY KEY NOT NULL,
  tipo TEXT NOT NULL,
  nome TEXT NOT NULL,
  cognome TEXT NOT NULL,
  codiceFiscale TEXT NOT NULL,
  datadinascita TEXT NOT NULL,
  luogodinascita TEXT NOT NULL,
  indirizzo TEXT NOT NULL,
  comune TEXT NOT NULL,
  provincia TEXT NOT NULL,
  regione TEXT NOT NULL,
  nazione TEXT NOT NULL,
  telefono TEXT NOT NULL,
  cellulare TEXT NOT NULL,
  email TEXT NOT NULL
);

CREATE TABLE operatori (
  idop TEXT PRIMARY KEY NOT NULL,
  idpe TEXT NOT NULL
);

CREATE TABLE vaccini (
  idv TEXT PRIMARY KEY NOT NULL,
  tipo TEXT NOT NULL,
  malattia TEXT NOT NULL,
  casaFarmaceutica TEXT NOT NULL
);

CREATE TABLE vacciniPazienti (
  idv TEXT NOT NULL,
  idp TEXT NOT NULL,
  data TEXT NOT NULL,
  datascadenza TEXT NOT NULL,
  lotto TEXT NOT NULL,
  dataproduzione TEXT NOT NULL,
  dose TEXT NOT NULL
);

CREATE TABLE strutture (
  ids TEXT PRIMARY KEY NOT NULL,
  indirizzo TEXT NOT NULL,
  massimali INTEGER NOT NULL,
  email TEXT NOT NULL
);

CREATE TABLE turni (
  idtu TEXT PRIMARY KEY NOT NULL,
  idop TEXT NOT NULL,
  ids TEXT NOT NULL,
  data TEXT NOT NULL,
  ora TEXT NOT NULL
);

CREATE TABLE orelavorate (
  idop TEXT NOT NULL,
  ids TEXT NOT NULL,
  data TEXT NOT NULL,
  ora TEXT NOT NULL
);

CREATE TABLE spedizioni (
  ids TEXT NOT NULL,
  datap TEXT NOT NULL,
  datac TEXT NOT NULL,
  quantita INTEGER NOT NULL
);

CREATE TABLE scorte (
  ids TEXT NOT NULL,
  quantita INTEGER NOT NULL
);

CREATE TABLE vaccinoCovid (
  idp TEXT NOT NULL,
  idop TEXT NOT NULL,
  ids TEXT NOT NULL,
  data TEXT NOT NULL,
  ora TEXT NOT NULL,
  lotto TEXT NOT NULL,
  dataproduzione TEXT NOT NULL,
  dose TEXT NOT NULL
);

CREATE TABLE prenotazioniCovid (
  idpr TEXT PRIMARY KEY NOT NULL,
  idp TEXT NOT NULL,
  ids TEXT NOT NULL,
  data TEXT NOT NULL,
  ora TEXT NOT NULL
);

CREATE TABLE effettiCollaterali (
  idp TEXT NOT NULL,
  data TEXT NOT NULL,
  descrizione TEXT NOT NULL
);

CREATE TABLE orari (
  id TEXT NOT NULL,
  orario TEXT NOT NULL,
  giorno INTEGER NOT NULL
);

ALTER TABLE operatori ADD FOREIGN KEY (idpe) REFERENCES personale (idpe);

ALTER TABLE studioPersonale ADD FOREIGN KEY (idpe) REFERENCES personale (idpe);

ALTER TABLE studioPersonale ADD FOREIGN KEY (idst) REFERENCES studiomedico (idst);

ALTER TABLE personale ADD FOREIGN KEY (idpe) REFERENCES pazienti (idpe);

ALTER TABLE vacciniPazienti ADD FOREIGN KEY (idv) REFERENCES vaccini (idv);

ALTER TABLE vacciniPazienti ADD FOREIGN KEY (idp) REFERENCES pazienti (idp);

ALTER TABLE turni ADD FOREIGN KEY (idop) REFERENCES operatori (idop);

ALTER TABLE turni ADD FOREIGN KEY (ids) REFERENCES strutture (ids);

ALTER TABLE orelavorate ADD FOREIGN KEY (idop) REFERENCES operatori (idop);

ALTER TABLE orelavorate ADD FOREIGN KEY (ids) REFERENCES strutture (ids);

ALTER TABLE spedizioni ADD FOREIGN KEY (ids) REFERENCES strutture (ids);

ALTER TABLE scorte ADD FOREIGN KEY (ids) REFERENCES strutture (ids);

ALTER TABLE vaccinoCovid ADD FOREIGN KEY (idp) REFERENCES pazienti (idp);

ALTER TABLE vaccinoCovid ADD FOREIGN KEY (idp) REFERENCES personale (idpe);

ALTER TABLE vaccinoCovid ADD FOREIGN KEY (idop) REFERENCES operatori (idop);

ALTER TABLE vaccinoCovid ADD FOREIGN KEY (ids) REFERENCES strutture (ids);

ALTER TABLE prenotazioniCovid ADD FOREIGN KEY (idp) REFERENCES pazienti (idp);

ALTER TABLE effettiCollaterali ADD FOREIGN KEY (idp) REFERENCES pazienti (idp);

ALTER TABLE orari ADD FOREIGN KEY (id) REFERENCES strutture (ids);

ALTER TABLE orari ADD FOREIGN KEY (id) REFERENCES studiomedico (idst);

ALTER TABLE prenotazioniCovid ADD FOREIGN KEY (ids) REFERENCES strutture (ids);

                        DROP TABLE IF EXISTS pazienti;
                        CREATE TABLE pazienti (
                          idp TEXT PRIMARY KEY NOT NULL,
                          idpe TEXT,
                          nome TEXT NOT NULL,
                          cognome TEXT NOT NULL,
                          codiceFiscale TEXT NOT NULL,
                          datadinascita TEXT NOT NULL,
                          luogodinascita TEXT NOT NULL,
                          indirizzo TEXT NOT NULL,
                          telefono TEXT NOT NULL,
                          cellulare TEXT NOT NULL,
                          email TEXT NOT NULL
                        );

                        DROP TABLE IF EXISTS studiomedico;
                        CREATE TABLE studiomedico (
                          idst TEXT PRIMARY KEY NOT NULL,
                          telefono TEXT NOT NULL,
                          email TEXT NOT NULL,
                          indirizzo TEXT NOT NULL
                        );

                        DROP TABLE IF EXISTS personale;
                        CREATE TABLE personale (
                          idpe TEXT PRIMARY KEY NOT NULL,
                          tipo TEXT NOT NULL,
                          nome TEXT NOT NULL,
                          cognome TEXT NOT NULL,
                          codiceFiscale TEXT NOT NULL,
                          datadinascita TEXT NOT NULL,
                          luogodinascita TEXT NOT NULL,
                          indirizzo TEXT NOT NULL,
                          cellulare TEXT NOT NULL,
                          email TEXT NOT NULL
                        );

                        DROP TABLE IF EXISTS studioPersonale;
                        CREATE TABLE studioPersonale (
                          idst TEXT NOT NULL,
                          idpe TEXT NOT NULL,
                          FOREIGN KEY (idst) REFERENCES studiomedico(idst) ON DELETE CASCADE,
                          FOREIGN KEY (idpe) REFERENCES personale(idpe) ON DELETE CASCADE
                        );

                        DROP TABLE IF EXISTS operatori;
                        CREATE TABLE operatori (
                          idop TEXT PRIMARY KEY NOT NULL,
                          idpe TEXT NOT NULL,
                          FOREIGN KEY (idpe) REFERENCES personale(idpe) ON DELETE CASCADE
                        );

                        DROP TABLE IF EXISTS vaccini;
                        CREATE TABLE vaccini (
                          idv TEXT PRIMARY KEY NOT NULL,
                          tipo TEXT NOT NULL,
                          malattia TEXT NOT NULL,
                          casaFarmaceutica TEXT NOT NULL
                        );

                        DROP TABLE IF EXISTS vacciniPazienti;
                        CREATE TABLE vacciniPazienti (
                          idrVP TEXT PRIMARY KEY NOT NULL,
                          idv TEXT NOT NULL,
                          idp TEXT NOT NULL,
                          data TEXT NOT NULL,
                          datascadenza TEXT NOT NULL,
                          lotto TEXT NOT NULL,
                          dataproduzione TEXT NOT NULL,
                          dose TEXT NOT NULL,
                          FOREIGN KEY (idp) REFERENCES pazienti(idp) ON DELETE CASCADE,
                          FOREIGN KEY (idv) REFERENCES vaccini(idv) ON DELETE CASCADE
                        );

                        DROP TABLE IF EXISTS strutture;
                        CREATE TABLE strutture (
                          ids TEXT PRIMARY KEY NOT NULL,
                          indirizzo TEXT NOT NULL,
                          massimali INTEGER NOT NULL,
                          email TEXT NOT NULL
                        );

                        DROP TABLE IF EXISTS turni;
                        CREATE TABLE turni (
                          idtu TEXT PRIMARY KEY NOT NULL,
                          idop TEXT NOT NULL,
                          ids TEXT NOT NULL,
                          giorno TEXT NOT NULL,
                          start TEXT NOT NULL,
                          end TEXT NOT NULL,
                          FOREIGN KEY (ids) REFERENCES strutture(ids) ON DELETE CASCADE,
                          FOREIGN KEY (idop) REFERENCES operatori(idop) ON DELETE CASCADE
                        );

                        DROP TABLE IF EXISTS orelavorate;
                        CREATE TABLE orelavorate (
                          idop TEXT NOT NULL,
                          ids TEXT NOT NULL,
                          data TEXT NOT NULL,
                          ora TEXT NOT NULL,
                          FOREIGN KEY (ids) REFERENCES strutture(ids) ON DELETE CASCADE,
                          FOREIGN KEY (idop) REFERENCES operatori(idop) ON DELETE CASCADE
                        );

                        DROP TABLE IF EXISTS spedizioni;
                        CREATE TABLE spedizioni (
                          ids TEXT NOT NULL,
                          datap TEXT NOT NULL,
                          datac TEXT NOT NULL,
                          quantita INTEGER NOT NULL,
                          FOREIGN KEY (ids) REFERENCES strutture(ids) ON DELETE CASCADE
                        );

                        DROP TABLE IF EXISTS scorte;
                        CREATE TABLE scorte (
                          ids TEXT NOT NULL,
                          quantita INTEGER NOT NULL,
                          FOREIGN KEY (ids) REFERENCES strutture(ids) ON DELETE CASCADE
                        );

                        DROP TABLE IF EXISTS vaccinoCovid;
                        CREATE TABLE vaccinoCovid (
                          idp TEXT NOT NULL,
                          idop TEXT NOT NULL,
                          ids TEXT NOT NULL,
                          data TEXT NOT NULL,
                          ora TEXT NOT NULL,
                          lotto TEXT NOT NULL,
                          dataproduzione TEXT NOT NULL,
                          dose TEXT NOT NULL,
                          FOREIGN KEY (ids) REFERENCES strutture(ids) ON DELETE CASCADE,
                          FOREIGN KEY (idop) REFERENCES operatori(idop) ON DELETE CASCADE,
                          FOREIGN KEY (idp) REFERENCES personale(idpe) ON DELETE CASCADE,
                          FOREIGN KEY (idp) REFERENCES pazienti(idp) ON DELETE CASCADE
                        );

                        DROP TABLE IF EXISTS prenotazioniCovid;
                        CREATE TABLE prenotazioniCovid (
                          idpr TEXT PRIMARY KEY NOT NULL,
                          idp TEXT NOT NULL,
                          ids TEXT NOT NULL,
                          data TEXT NOT NULL,
                          ora TEXT NOT NULL,
                          FOREIGN KEY (ids) REFERENCES strutture(ids) ON DELETE CASCADE,
                          FOREIGN KEY (idp) REFERENCES pazienti(idp) ON DELETE CASCADE
                        );

                        DROP TABLE IF EXISTS accertamento;
                        CREATE TABLE accertamento (
                          idac TEXT PRIMARY KEY NOT NULL,
                          idp TEXT NOT NULL,
                          idop TEXT NOT NULL,
                          ids TEXT NOT NULL,
                          data TEXT NOT NULL,
                          FOREIGN KEY (idp) REFERENCES pazienti(idp) ON DELETE CASCADE,
                          FOREIGN KEY (idop) REFERENCES operatori(idop) ON DELETE CASCADE,
                          FOREIGN KEY (ids) REFERENCES strutture(ids) ON DELETE CASCADE
                        );  

                        DROP TABLE IF EXISTS effettiCollaterali;
                        CREATE TABLE effettiCollaterali (
                          idec TEXT PRIMARY KEY NOT NULL,
                          idac TEXT NOT NULL,
                          descrizione TEXT NOT NULL,
                          FOREIGN KEY (idac) REFERENCES accertamento(idac) ON DELETE CASCADE
                        );

                        DROP TABLE IF EXISTS orari;
                        CREATE TABLE orari (
                          id TEXT NOT NULL,
                          orario TEXT NOT NULL,
                          giorno INTEGER NOT NULL,
                          FOREIGN KEY (id) REFERENCES studiomedico(idst) ON DELETE CASCADE,
                          FOREIGN KEY (id) REFERENCES strutture(ids) ON DELETE CASCADE
                        );

                        DROP TABLE IF EXISTS InAttesa;
                        CREATE TABLE InAttesa (
                          idp TEXT NOT NULL,
                          data TEXT NOT NULL,
                          ora TEXT NOT NULL,
                          FOREIGN KEY (idp) REFERENCES pazienti(idp) ON DELETE CASCADE
                        );